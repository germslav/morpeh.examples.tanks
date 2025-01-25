namespace Tanks.Bases {
    using System;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Providers;
    using Teams;
    using UnityEngine;

    [AddComponentMenu("Tanks/TeamBase")]
    public sealed class TeamBaseProvider : MonoProvider<TeamBase> {
        public TeamProvider team;

        protected override void Initialize() {
            base.Initialize();
            var stash = Entity.GetWorld().GetStash<InTeam>();

            stash.Add(Entity).team = team.Entity;
        }
    }

    [Serializable]
    public struct TeamBase : IComponent {
        public TeamBaseView view;
    }
}