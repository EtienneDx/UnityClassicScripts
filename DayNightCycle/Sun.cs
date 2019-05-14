using UnityEngine;

namespace EtienneDx.DayNightCycle
{
    public class Sun : MonoBehaviour, IDayNightCycle
    {
        [SerializeField]
        private float dayDuration = 600;

        [SerializeField]
        private float nightDuration = 600;

        [SerializeField]
        private float rotationOffset = -90;

        [SerializeField]
        private float initialTime = 300;

        /// <summary>
        /// The total duration of a cycle (day + night)
        /// </summary>
        public float DayLength
        {
            get
            {
                return dayDuration + nightDuration;
            }
        }

        /// <summary>
        /// Is it currently day?
        /// </summary>
        public bool IsDay
        {
            get
            {
                return initialTime + Time.time % DayLength < dayDuration;
            }
        }

        /// <summary>
        /// Is it currently night?
        /// </summary>
        public bool IsNight
        {
            get
            {
                return !IsDay;
            }
        }

        private void Update()
        {
            float currentTime = initialTime + Time.time % DayLength;
            if(currentTime < dayDuration)
            {
                float percent = currentTime / dayDuration;
                transform.localRotation = Quaternion.Euler(0, rotationOffset + percent * 180, 0);// Quaternion.AngleAxis(percent * 180, transform.up);
            }
            else
            {
                float percent = (currentTime - dayDuration) / nightDuration;
                transform.localRotation = Quaternion.Euler(0, rotationOffset + 180 + percent * 180, 0);
                //transform.rotation = Quaternion.AngleAxis(180 + percent * 180, transform.up);
            }
        }
    }
}
