
namespace EtienneDx.Utils
{
    interface IDefine
    {
        string Name { get; set; }
    }

    struct Define : IDefine
    {
        public string Name { get; set; }

        public string definition;

        public bool enabled;

        public Define(string name = "", string definition = "", bool enabled = false)
        {
            Name = name;
            this.definition = definition;
            this.enabled = enabled;
        }
    }

    struct DefineSeparation : IDefine
    {
        public string Name { get; set; }

        public DefineSeparation(string name)
        {
            Name = name;
        }
    }
}