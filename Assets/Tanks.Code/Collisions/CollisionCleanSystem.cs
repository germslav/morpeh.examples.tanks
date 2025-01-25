namespace Tanks.Collisions {
    using Scellecs.Morpeh;

    using UnityEngine;

    public sealed class CollisionCleanSystem : ISystem
    {
        public World World { get ; set; }

        Filter _filter;

        public void Dispose()
        {
            _filter = World.Filter.With<CollisionEvent>().Build();
        }

        public void OnAwake()
        {

        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var ent in _filter)
            {
                World.RemoveEntity(ent);
            }
        }
    }
}