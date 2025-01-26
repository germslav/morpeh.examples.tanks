namespace Tanks.Walls {
    using Healthcare;
    using Scellecs.Morpeh;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class WallDestroySystem : ISystem 
    {
        private Filter destroyedWalls;

        private Stash<Wall> _walls;

        public World World { get; set; }

        public void OnAwake() {
            destroyedWalls = World.Filter.With<Wall>().With<IsDeadMarker>().Build();

            _walls = World.GetStash<Wall>();
        }

        public void OnUpdate(float deltaTime) {
            foreach (Entity ent in destroyedWalls) {
                GameObject wallGo = _walls.Get(ent).transform.gameObject;
                World.RemoveEntity(ent);
                GameObject.Destroy(wallGo);
            }
        }

        public void Dispose()
        {
        }
    }
}