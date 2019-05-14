
namespace EtienneDx.Utils
{
    [System.Serializable]
    public struct NamedBool
    {
        public string name;

        public bool value;

        public NamedBool(string name, bool value)
        {
            this.name = name;
            this.value = value;
        }
    }
}