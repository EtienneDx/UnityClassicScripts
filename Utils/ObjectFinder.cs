using UnityEngine;

namespace EtienneDx.Utils
{
    public class ObjectFinder<T> where T : Component
    {
        private T val;

        public T Value
        {
            get
            {
                return val ?? (val = Object.FindObjectOfType<T>());
            }
        }

        public static implicit operator T(ObjectFinder<T> obj)
        {
            return obj.Value;
        }
    }
}