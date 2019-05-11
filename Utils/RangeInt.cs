[System.Serializable]
public class RangeInt
{
    public int min;

    public int max;

    public int Random
    {
        get
        {
            return UnityEngine.Random.Range(min, max);
        }
    }
}