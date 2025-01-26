namespace Tanks.Healthcare {
    using GameInput;
    using Scellecs.Morpeh;

    using Scores;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TankDestroySystem : ISystem
    {
        private Filter destroyedTanks;

        private Stash<ControlledByUser> _controlledByUsers;
        private Stash<UserWithTank> _userWithTanks;
        private Stash<Tank> _tanks;
        private Stash<DamageEvent> _damageEvents;
        private Stash<OneMoreKillEvent> _oneMoreKillEvents;

        public World World { get; set; }

        public void OnAwake()
        {
            destroyedTanks = World.Filter.With<Tank>().With<IsDeadMarker>().Build();

            _controlledByUsers = World.GetStash<ControlledByUser>();
            _userWithTanks = World.GetStash<UserWithTank>();
            _tanks = World.GetStash<Tank>();
            _damageEvents = World.GetStash<DamageEvent>();
            _oneMoreKillEvents = World.GetStash<OneMoreKillEvent>();
        }

        public void OnUpdate(float deltaTime) 
        {
            foreach (Entity ent in destroyedTanks) 
            {
                IncreaseStatForKiller(ent);

                ref ControlledByUser controlledByUser = ref _controlledByUsers.Get(ent, out bool isControlled);
                if (isControlled) 
                {
                    _userWithTanks.Remove(controlledByUser.user);
                }

                GameObject tankGo = _tanks.Get(ent).body.gameObject;
                World.RemoveEntity(ent);
                GameObject.Destroy(tankGo);
            }
        }

        private void IncreaseStatForKiller(Entity ent) 
        {
            ref DamageEvent damageEvent = ref _damageEvents.Get(ent, out bool isDamaged);
            if (isDamaged) 
            {
                _oneMoreKillEvents.Add(damageEvent.dealer);
            }
        }

        public void Dispose()
        {
        }
    }
}