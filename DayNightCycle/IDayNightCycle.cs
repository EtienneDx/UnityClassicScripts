namespace EtienneDx.DayNightCycle
{
    public interface IDayNightCycle
    {
        float DayLength { get; }
        bool IsDay { get; }
        bool IsNight { get; }
    }
}