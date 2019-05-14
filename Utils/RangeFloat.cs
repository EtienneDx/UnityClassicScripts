
namespace EtienneDx.Utils
{
    [System.Serializable]
    public class RangeFloat
    {
        public float min;

        public float max;

        public float Random
        {
            get
            {
                return UnityEngine.Random.Range(min, max);
            }
        }
    }
}