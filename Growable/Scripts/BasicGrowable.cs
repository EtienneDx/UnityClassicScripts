using System;
using UnityEngine;

namespace EtienneDx.Growable
{
    /// <summary>
    /// Base class for growables, only able to linearly grow an object with a single model
    /// </summary>
    public class BasicGrowable : MonoBehaviour, IGrowable
    {
        public float growthLength;
        public float GrowthLength { get { return growthLength; } }

        public float CurrentGrowthLevel { get; private set; }

        public virtual float MaxGrowthLevel { get { return 1; } }

        Vector3 initScale;// == target scale

        public virtual void Start()
        {
            initScale = transform.localScale;
            SetGrowthLevel(0);
        }

        private void Update()
        {
            if (CanGrow())
                SetGrowthLevel(CurrentGrowthLevel + Time.deltaTime / GrowthLength);
        }

        public virtual bool CanGrow()
        {
            return CurrentGrowthLevel < MaxGrowthLevel;//only grow if not at full size
        }

        public virtual void SetGrowthLevel(float growthLevel)
        {
            transform.localScale = initScale * growthLevel;
            CurrentGrowthLevel = growthLevel;
        }
    }
}