namespace Tanks {
    using Scellecs.Morpeh;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class PhysicsUpdateSystem : IFixedSystem {
        public World World { get; set; }

        public void OnAwake() { }

        public void OnUpdate(float deltaTime) {
            Simulate(deltaTime);
        }

        public static void Simulate(float dt) {
            Physics2D.Simulate(dt);
        }

        public void Dispose()
        {
        }
    }
}