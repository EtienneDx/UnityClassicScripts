using UnityEngine;

namespace EtienneDx.SpawnManager
{
    [System.Serializable]
    public partial class SpawnableEntity
    {
        [Tooltip("The entity must contain a monoBehaviour extending the 'ISpawnableEntity' interface")]
        public GameObject entity;

        [Range(0, 1)]
        public float rarity;
    }
}