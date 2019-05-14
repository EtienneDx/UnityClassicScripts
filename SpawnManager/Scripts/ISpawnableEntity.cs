
namespace EtienneDx.SpawnManager
{
    public interface ISpawnableEntity
    {
        /*
         * Used to check if we should remove the entity from the list of spawned entities or not.
         */
        bool IsDead { get; }

        /*
         * Determine if the entity will respawn after his death. The SpawnManager set it to false.
         */
        void SetRespawn(bool value);

        /*
         * Called when adding the entity from the list of spawned entities
         */
        void UnlinkToRegion();

        /*
         * Called when removing the entity from the list of spawned entities
         */
        void LinkToRegion();
    }
}