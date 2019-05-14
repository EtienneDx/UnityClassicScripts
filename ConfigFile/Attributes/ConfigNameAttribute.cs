using System;

namespace EtienneDx.ConfigFile
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
