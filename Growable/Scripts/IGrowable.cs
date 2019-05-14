
namespace EtienneDx.Growable
{
    public interface IGrowable
    {
        /// <summary>
        /// The maximum growth level
        /// </summary>
        float MaxGrowthLevel { get; }

        /// <summary>
        /// Set the growth of the Growable to a certain level, between 0 and MaxGrowthLevel
        /// </summary>
        void SetGrowthLevel(float growthLevel);

        /// <summary>
        /// Returns the current growth level, between 0 and MaxGrowthLevel
        /// </summary>
        float CurrentGrowthLevel { get; }

        /// <summary>
        /// Can the Growable grow in current conditions?
        /// </summary>
        bool CanGrow();

        /// <summary>
        /// The time for the growable to grow
        /// </summary>
        float GrowthLength { get; }
    }
}