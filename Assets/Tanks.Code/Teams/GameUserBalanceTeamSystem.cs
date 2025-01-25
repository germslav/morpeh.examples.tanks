namespace Tanks.Teams {
    using GameInput;
    using Scellecs.Morpeh;

    public sealed class GameUserBalanceTeamSystem : ISystem
    {
        private Filter nonTeamUsers;
        private Filter teams;
        private Stash<Team> _teamStash;
        private Stash<InTeam> _inTeamStash;
        public World World { get; set; }

        public void OnAwake() {
            teams = World.Filter.With<Team>().Build();
            nonTeamUsers = World.Filter.With<GameUser>().Without<InTeam>().Build();

            _teamStash = World.GetStash<Team>();
            _inTeamStash = World.GetStash<InTeam>();
        }

        public void OnUpdate(float deltaTime) {
            if (nonTeamUsers.IsEmpty() || teams.IsEmpty()) {
                return;
            }

            foreach (Entity ent in nonTeamUsers) {
                Entity weakTeamEntity = GetWeakTeam();
                ref Team weakTeam = ref _teamStash.Get(weakTeamEntity);
                _inTeamStash.Set(ent, new InTeam {
                        team = weakTeamEntity,
                });

                weakTeam.userCount++;
            }
        }

        private Entity GetWeakTeam() {
            Entity weakTeam = default;
            var weakTeamUserCount = int.MaxValue;

            foreach (Entity entity in teams) {
                ref Team team = ref _teamStash.Get(entity);
                if (team.userCount < weakTeamUserCount) {
                    weakTeam = entity;
                    weakTeamUserCount = team.userCount;
                }
            }

            return weakTeam;
        }

        public void Dispose() {}
    }
}