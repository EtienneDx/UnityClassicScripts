using System;
using UnityEngine;

namespace EtienneDx.SpawnManager.Test
{
    public class Spawnable : MonoBehaviour, ISpawnableEntity
    {
        [SerializeField]
        private float despawnAfter = 10;

        private float startTime = 0;

        public bool IsDead
        {
            get
            {
                return startTime + despawnAfter < Time.time;
            }
        }

        // We don't care about being linked or not
        public void LinkToRegion() { }
        public void UnlinkToRegion()
        {
            Destroy(gameObject);
        }

        public void SetRespawn(bool value)
        {
            // do nothing, we don't want any automatic respawn ability 
            // (this function is only called by the spawn manager with false as an argument, 
            // to disable the eventual existing behavior)
        }

        private void Start()
        {
            startTime = Time.time;
        }
    }
}
