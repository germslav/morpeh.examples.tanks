namespace Tanks.Weapons {
    using GameInput;
    using Scellecs.Morpeh;

    using UnityEngine.InputSystem;

    public sealed class UserBulletWeaponSystem : ISystem
    {
        public World World { get; set; }

        Filter _filter;
        Stash<BulletWeapon> _bulletWeapons;
        Stash<ControlledByUser> _controlled;
        Stash<GameUser> _users;

        public void Dispose()
        {

        }

        public void OnAwake()
        {
            _filter = World.Filter.With<BulletWeapon>().With<ControlledByUser>().Build();

            _bulletWeapons = World.GetStash<BulletWeapon>();
            _controlled = World.GetStash<ControlledByUser>();
            _users = World.GetStash<GameUser>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var ent in _filter)
            {
                ref var weapon = ref _bulletWeapons.Get(ent);
                ref var controller = ref _controlled.Get(ent);
                ref var user = ref _users.Get(controller.user);

                InputActionPhase actionPhase = user.inputActionsMap.FindAction("Fire").phase;
                weapon.shoot = actionPhase == InputActionPhase.Started || actionPhase == InputActionPhase.Performed;
            }
        }
    }
}