namespace Tanks.Movement {
    using Scellecs.Morpeh;

    using UnityEngine;
    using UnityEngine.EventSystems;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MovementSystem))]
    public sealed class MovementSystem : IFixedSystem
    {
        public World World { get; set; }

        Filter _tankFilter;
        Stash<Tank> _tanks;
        Stash<MoveDirection> _moveDirections;

        public void OnAwake()
        {
            _tankFilter = World.Filter.With<Tank>().Build();

            _tanks = World.GetStash<Tank>();
            _moveDirections = World.GetStash<MoveDirection>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (Entity tankEnt in _tankFilter)
            {
                ref var tank = ref _tanks.Get(tankEnt);
                ref var moveDir = ref _moveDirections.Get(tankEnt);

                Vector2 direction = moveDir.direction;
                Vector2 velocity = tank.config.speed * direction;
                tank.body.velocity = velocity;
                tank.body.angularVelocity = 0f;

                if (direction.sqrMagnitude <= 0f)
                {
                    return;
                }

                float angle = Vector2.SignedAngle(Vector2.up, direction);
                tank.body.rotation = angle;
            }
        }

        public void Dispose()
        {

        }
    }
}