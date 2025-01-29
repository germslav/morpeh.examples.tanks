namespace Tanks.Movement {
    using GameInput;
    using Scellecs.Morpeh;

    using UnityEngine;

    public sealed class UserMovementSystem : ISystem //<MoveDirection, ControlledByUser> {
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<MoveDirection> _moveDirections;
        private Stash<ControlledByUser> _controlled;
        private Stash<GameUser> _gameUsers;

        public void OnAwake()
        {
            _filter = World.Filter.With<MoveDirection>().With<ControlledByUser>().Build();
            _moveDirections = World.GetStash<MoveDirection>();
            _controlled = World.GetStash<ControlledByUser>();
            _gameUsers = World.GetStash<GameUser>();
        }

        public void Dispose()
        {
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var ent in _filter)
            {
                ref var controlled = ref _controlled.Get(ent);
                ref var direction = ref _moveDirections.Get(ent);
                ref var user = ref _gameUsers.Get(controlled.user);

                direction.direction = user.inputActionsMap.FindAction("Movement").ReadValue<Vector2>();
            }
        }
    }
}