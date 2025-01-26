namespace Tanks.Teams {
    using Scellecs.Morpeh;

    using UnityEngine;
    using UtilSystems;
    using static Tanks.UtilSystems.TextInWorldSystem;

    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(WinDetectSystem))]
    public sealed class WinDetectSystem : ISystem {
        public float winTimeScale = 0.1f;
        public Request winMessageRequest;

        private Filter nonLosingTeams;
        private Filter winMarkersFilter;
        private Stash<Team> _teams;
        private Stash<Request> _requests;
        private Stash<WinMarker> _winMarkers;

        public World World { get; set; }

        public void OnAwake() {
            nonLosingTeams = World.Filter.With<Team>().Without<LosingTeamMarker>().Build();
            winMarkersFilter = World.Filter.With<WinMarker>().Build();

            _winMarkers = World.GetStash<WinMarker>();
            _requests = World.GetStash<Request>();
            _teams = World.GetStash<Team>();
        }

        public void OnUpdate(float deltaTime) {
            if (!winMarkersFilter.IsEmpty()) {
                return;
            }

            if (nonLosingTeams.IsEmpty() || nonLosingTeams.GetLengthSlow() > 1) {
                return;
            }

            ref Team winTeam = ref _teams.Get(nonLosingTeams.First());
            var text = $"Team {winTeam.name} wins!";
            Debug.Log(text);

            Request request = winMessageRequest;
            request.color = winTeam.color;
            request.text = text;

            var textReq = World.CreateEntity();
            _requests.Add(textReq);

            Time.timeScale = winTimeScale;

            var winMakers = World.CreateEntity();
            _winMarkers.Add(winMakers);
        }

        public struct WinMarker : IComponent { }


        public void Dispose()
        {

        }
    }
}