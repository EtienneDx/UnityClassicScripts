/**********************Defines*********************/
// Commenting and uncommenting these defines enable or disable certain functionnalities
// and compatibility with other assets.
// /!\ If one file allow a certain define, all the other files and assets should too /!\

// Multiple childrens models
#define USE_MULTIPLE_CHILDREN

/*******************************************/


using EtienneDx.Utils;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EtienneDx.Growable
{
    /// <summary>
    /// More complex growable, allow multiple models and a range of maximum growth
    /// </summary>
    public class Growable : BasicGrowable
    {
#if USE_MULTIPLE_CHILDREN
        public override void Start()
        {
            if (useMultipleChildren)
                childrenPerGrowth.OrderBy((LimitGameObject lgo) => lgo.minLimit);
            base.Start();
        }
#endif

        public RangeFloat maxGrowthLevel;
        float? _maxGrowthLevel = null;
        public override float MaxGrowthLevel
        {
            get
            {
                return (float)(_maxGrowthLevel ?? (_maxGrowthLevel = maxGrowthLevel.Random));//if no max choosen, choose one
            }
        }

#if USE_MULTIPLE_CHILDREN
        public bool useMultipleChildren = false;
        /// <summary>
        /// The list of objects to use depending on the growth 
        /// </summary>
        public List<LimitGameObject> childrenPerGrowth;
        int actualId = -1;

        public override void SetGrowthLevel(float growthLevel)
        {
            base.SetGrowthLevel(growthLevel);

            if (!useMultipleChildren) return;

            LimitGameObject lgo = childrenPerGrowth.Last((LimitGameObject lgo2) => growthLevel / MaxGrowthLevel >= lgo2.minLimit) ?? childrenPerGrowth.Last();
            if (childrenPerGrowth.IndexOf(lgo) != actualId)//only change the children if the model has changed
            {
                actualId = childrenPerGrowth.IndexOf(lgo);
                //remove old model
                Transform child = transform.Find("MODEL");
                if (child != null)
                    Destroy(child.gameObject);
                var go = Instantiate(lgo.children, transform, false);
                go.transform.localScale = lgo.children.transform.localScale;//ensure no change of children scale
                go.name = "MODEL";
            }
        }

#endif
    }
}