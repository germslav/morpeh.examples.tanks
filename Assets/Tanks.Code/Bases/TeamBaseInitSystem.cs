namespace Tanks.Bases {
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Systems;
    using Teams;
    using UnityEngine;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TeamBaseInitSystem))]
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
            throw new System.NotImplementedException();
        }

        private struct InitializedMarker : IComponent { }
    }
}