using System;
using System.Linq;

namespace EtienneDx.Conditions
{
    [Serializable]
    public class Condition
    {
        public ConditionMode mode = ConditionMode.ALL;

        public Condition[] subConditions = new Condition[0];

        public string[] conditions = new string[0];

        public bool IsValid
        {
            get
            {
                if (conditions.Length == 0 && subConditions.Length == 0) return true;
                bool ret;
                if (subConditions.Length > 0)
                    ret = mode == ConditionMode.ALL ? subConditions.All(c => c.IsValid) : subConditions.Any(c => c.IsValid);
                else
                    ret = mode == ConditionMode.ALL;
                if (conditions.Length > 0)
                {
                    if (mode == ConditionMode.ALL)
                    {
                        ret &= conditions.All(c => ConditionList.IsValid(c));
                    }
                    else
                    {
                        ret |= conditions.Any(c => ConditionList.IsValid(c));
                    }
                }
                return ret;
            }
        }
    }

    public enum ConditionMode
    {
        ALL,
        ANY
    }
}
