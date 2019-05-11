using System;

namespace Assets.EtienneDx.ConfigFile.Attributes
{
    public class ConfigNameAttribute : Attribute
    {
        public string name;

        public ConfigNameAttribute(string name)
        {
            this.name = name;
        }
    }
}
