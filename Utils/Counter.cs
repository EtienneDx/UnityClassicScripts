
namespace EtienneDx.Utils
{
    public class Counter
    {
        public int stepsBeforeReset = 10;

        private int steps = 0;
        private readonly bool autoReset = true;

        public Counter(int stepsBeforeReady, bool autoReset = true)
        {
            stepsBeforeReset = stepsBeforeReady;
            Reset();
            this.autoReset = autoReset;
        }

        public int StepsLeft
        {
            get { return stepsBeforeReset - steps; }
        }

        public int StepsDone
        {
            get { return steps; }
        }

        public float Progress
        {
            get
            {
                return (float)steps / stepsBeforeReset;
            }
        }

        public bool Ready
        {
            get
            {
                steps++;
                if (steps >= stepsBeforeReset)
                {
                    if (autoReset)
                    {
                        Reset();
                    }

                    return true;
                }
                return false;
            }
        }

        public void Reset()
        {
            steps = 0;
        }
    }
}