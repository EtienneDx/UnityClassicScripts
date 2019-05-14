//All defines used in the script are stated here
#define UMMORPG

//#define SIMPLE_WEATHER

#define DAY_NIGHT_CYCLE

#if DAY_NIGHT_CYCLE
using EtienneDx.DayNightCycle;
using EtienneDx.Utils;
#endif
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace EtienneDx.SpawnManager
{
    public partial class SpawnManager : MonoBehaviour
    {
        #region Fields

        ///////////////////////Public////////////////////////

        [Header("Spawnable Entities")]
        [SerializeField]
        SpawnableEntity[] spawnableEntities = new SpawnableEntity[0];

        [Header("Region")]
        public float regionRadius;

        public int maximumEntitiesAmount = 15;

        public int spawnRate = 30;

        public Transform regionEntityParent;

        [Tooltip("This layerMask indicate the buildings under / over which an entity isn't allowed to spawn.\n" +
            "Eg. The buildings if they have an empty inside.")]
        public LayerMask buildings;

#if SIMPLE_WEATHER || DAY_NIGHT_CYCLE
        [Header("Spawn Conditions")]
#endif

#if DAY_NIGHT_CYCLE
        public bool spawnDuringNight = true;

        public bool spawnDuringDay = false;
#endif
#if SIMPLE_WEATHER
    public bool spawnDuringRain = true;

    public bool spawnDuringSun = false;
#endif
        ///////////////Private////////////////////

        private List<ISpawnableEntity> entities = new List<ISpawnableEntity>();

        private Counter deathCheckCounter = new Counter(10);

        private Counter spawnEntityCounter;

#if DAY_NIGHT_CYCLE
        private ObjectFinder<Sun> sun = new ObjectFinder<Sun>();
#endif
#if SIMPLE_WEATHER
    private ObjectFinder<WeatherManager> weatherManager = new ObjectFinder<WeatherManager>();
#endif

        #endregion

        #region Functions

        private void Start()
        {
            spawnEntityCounter = new Counter(spawnRate);
        }

        private void FixedUpdate()
        {
            if (deathCheckCounter.Ready)//We check deaths every 10 updates
                CheckDeaths();

            // TODO rework conditions
            if (
#if DAY_NIGHT_CYCLE
            (((spawnDuringNight && sun.Value.IsNight) ||
                (spawnDuringDay && sun.Value.IsDay))
#if SIMPLE_WEATHER
            &&
#endif
#endif
#if SIMPLE_WEATHER
            ((spawnDuringRain && weatherManager.Value.GetWeather() == Weather.RAIN) ||
            (spawnDuringSun && weatherManager.Value.GetWeather() == Weather.SUN))
#endif
#if DAY_NIGHT_CYCLE
            )
#endif
            &&
                spawnEntityCounter.Ready)//we check if we spawn a mob every "spawnRate" updates
            {
                SpawnEntity();
            }
        }

        private void SpawnEntity()
        {
            if (Random.Range(0, maximumEntitiesAmount) > entities.Count)//The more monsters there is the less chance there is to spawn a mob
            {
                float totalSpawnProbability = GetTotalProba();
                float entityChoiceValue = Random.Range(0, totalSpawnProbability);
                float min = 0;
                int i = 0;
                try
                {
                    while (min + spawnableEntities[i].rarity < entityChoiceValue)
                    {
                        min += spawnableEntities[i].rarity;
                        i++;
                    }
                    Vector3 p = GetRandomPosition();
                    if (p != Vector3.zero)
                    {
                        GameObject newEntity = Instantiate(spawnableEntities[i].entity, p, Quaternion.identity, regionEntityParent);
                        newEntity.name = spawnableEntities[i].entity.name;
                        newEntity.GetComponent<ISpawnableEntity>().SetRespawn(false);
                        entities.Add(newEntity.GetComponent<ISpawnableEntity>());
                        newEntity.GetComponent<ISpawnableEntity>().LinkToRegion();
                        //NetworkServer.Spawn(newEntity);
                        // had some problem about this, so I force it to true
                        foreach (var re in newEntity.GetComponentsInChildren<Renderer>())
                        {
                            re.enabled = true;
                        }
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Unexcpected reaction while spawning a mob, passing for this time\nWarning infos : i = " + i +
                        "\ntotalSpawnProba = " + totalSpawnProbability +
                        "\nmobChoiceValue = " + entityChoiceValue, this);
                    Debug.Log(e);
                }
            }
        }

        private Vector3 GetRandomPosition()
        {
            Vector2 v = Random.insideUnitCircle * regionRadius;
            Vector3 retV = new Vector3(v.x, 0, v.y) + transform.position;
            NavMeshHit hit = new NavMeshHit();
            int i = 0;
            while (i < 100 && (!NavMesh.SamplePosition(retV, out hit, 2, 1) || !IsSpawnable(hit.position)))
            {
                i++;
            }
            if (IsSpawnable(hit.position))
            {
                return hit.position;
            }
            return Vector3.zero;
        }

        private bool IsSpawnable(Vector3 position)
        {
            //Debug.DrawLine(position + Vector3.up * 50, position + Vector3.down * 20, Color.red, 3);
            if (Physics.Raycast(position + Vector3.up * 50, Vector3.down, 70, buildings.value) ||
                Vector3.Distance(transform.position, position) > regionRadius)
                return false;
            return true;
        }

        private float GetTotalProba()
        {
            float f = 0;
            foreach (SpawnableEntity m in spawnableEntities)
            {
                f += m.rarity;
            }
            return f;
        }

        private void CheckDeaths()
        {
            List<ISpawnableEntity> deadEntities = new List<ISpawnableEntity>();
            foreach (ISpawnableEntity e in entities)
            {
                if (e.IsDead)
                    deadEntities.Add(e);
            }
            foreach (ISpawnableEntity e in deadEntities)
                entities.Remove(e);
        }

        #endregion

        #region Gizmos

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, regionRadius);
        }

        #endregion
    }
}