namespace Tanks.Movement {
    using Scellecs.Morpeh;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TankMovementInitSystem : ISystem 
    {
        private Filter tanksNoMove;
        private Stash<MoveDirection> _moveDirections;

        public World World { get; set; }

        public void Dispose()
        {

        }

        public void OnAwake() 
        {
            tanksNoMove = World.Filter.With<Tank>().Without<MoveDirection>().Build();

            _moveDirections = World.GetStash<MoveDirection>();
        }

        public void OnUpdate(float deltaTime) 
        {
            foreach (Entity ent in tanksNoMove) 
            {
                _moveDirections.Add(ent);
            }
        }
    }
}