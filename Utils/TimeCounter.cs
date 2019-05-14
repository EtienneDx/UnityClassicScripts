using UnityEngine;

namespace EtienneDx.Utils
{
    public class TimeCounter
    {
        private float start;
        private readonly float length;

        public bool Ready
        {
            get
            {
                return Time.time > start + length;
            }
        }

        public float Progress
        {
            get
            {
                return Ready || length == 0 ? 1 : (Time.time - start) / length;
            }
        }

        public float TimeLeft
        {
            get
            {
                return start + length - Time.time;
            }
        }

        public TimeCounter(float length)
        {
            this.length = length;
            if (length != 0)
            {
                Reset();
            }
        }

        public void Reset()
        {
            start = Time.time;
        }
    }
}