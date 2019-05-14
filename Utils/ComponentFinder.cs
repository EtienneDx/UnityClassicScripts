using UnityEngine;

namespace EtienneDx.Utils
{
    public class ComponentFinder<T> where T : Component
    {
        private MonoBehaviour b;

        private T val;

        public T Value
        {
            get
            {
                return val ?? (val = b.GetComponent<T>());
            }
        }

        public ComponentFinder(MonoBehaviour b)
        {
            this.b = b;
        }

        public static implicit operator T(ComponentFinder<T> obj)
        {
            return obj.Value;
        }
    }
}