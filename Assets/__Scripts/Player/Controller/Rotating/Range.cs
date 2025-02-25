
namespace Player.Controller.Rotating
{
    [System.Serializable]
    public class Range
    {
        public float min = 0;
        public float max = 1;

        public Range(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }
}

