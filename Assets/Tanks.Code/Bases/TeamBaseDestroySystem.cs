namespace Tanks.Bases {
    using Healthcare;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Systems;
    using Teams;
    using UnityEngine;

    public sealed class TeamBaseDestroySystem : ISystem {
        public Color destroyedColor = Color.black;

        private Filter destroyedBases;

        private Stash<InTeam> _inTeam;
        private Stash<LosingTeamMarker> _losingTeamMarker;
        private Stash<TeamBase> _teamBases;

        public World World { get; set; }

        public void Dispose()
        {
        }

        public void OnAwake() {
            destroyedBases = World.Filter.With<TeamBase>().With<InTeam>().With<IsDeadMarker>().Build();

            _inTeam = World.GetStash<InTeam>();
            _losingTeamMarker = World.GetStash<LosingTeamMarker>();
            _teamBases = World.GetStash<TeamBase>();
        }

        public void OnUpdate(float deltaTime) {
            foreach (Entity ent in destroyedBases) {
                var teamEnt = _inTeam.Get(ent).team;

                if (_losingTeamMarker.Has(teamEnt)) {
                    continue;
                }

                _losingTeamMarker.Set(teamEnt);

                _teamBases.Get(ent).view.SetColor(destroyedColor);
            }
        }
    }
}