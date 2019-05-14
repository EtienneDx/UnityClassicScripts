
namespace EtienneDx.Utils
{
    [System.Serializable]
    public struct NamedInt
    {
        public string name;

        public int value;

        public NamedInt(string name = "", int value = 0)
        {
            this.name = name;
            this.value = value;
        }
    }
}