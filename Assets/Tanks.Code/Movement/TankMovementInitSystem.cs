﻿namespace Tanks.Movement {
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Systems;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TankMovementInitSystem))]
    public sealed class TankMovementInitSystem : UpdateSystem {
        private Filter tanksNoMove;

        public override void OnAwake() {
            tanksNoMove = World.Filter.With<Tank>().Without<MoveDirection>().Build();
        }

        public override void OnUpdate(float deltaTime) {
            foreach (Entity ent in tanksNoMove) {
                ent.AddComponent<MoveDirection>();
            }
        }
    }
}