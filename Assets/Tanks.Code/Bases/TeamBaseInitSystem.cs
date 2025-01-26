namespace Tanks.Bases {
    using Scellecs.Morpeh;

    using Teams;
    using UnityEngine;

    public sealed class TeamBaseInitSystem : ISystem 
    {
        private Filter filter;

        public World World { get; set ; }

        public void OnAwake() {
            filter = World.Filter.With<TeamBase>().With<InTeam>().Without<InitializedMarker>().Build();
        }

        public void OnUpdate(float deltaTime) {
            foreach (Entity ent in filter) {
                Entity teamEntity = ent.GetComponent<InTeam>().team;
                Color teamColor = teamEntity.GetComponent<Team>().color;
                ent.GetComponent<TeamBase>().view.SetColor(teamColor);
                ent.AddComponent<InitializedMarker>();
            }
        }

        public void Dispose()
        {
        }

        private struct InitializedMarker : IComponent { }
    }
}