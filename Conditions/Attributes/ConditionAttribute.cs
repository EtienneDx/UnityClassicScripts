using System;

namespace EtienneDx.Conditions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ConditionAttribute : Attribute
    {
        public string conditionName;

        public ConditionAttribute(string conditionName)
        {
            this.conditionName = conditionName;
        }
    }
}
