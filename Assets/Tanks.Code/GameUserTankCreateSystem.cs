namespace Tanks {
    using GameInput;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Providers;

    using System.Linq;
    using Teams;
    using UnityEngine;

    public sealed class GameUserTankCreateSystem : ISystem {
        private Filter filter;

        private Stash<UserWithTank> _userWithTanks;
        private Stash<InTeam> _inTeams;
        private Stash<ControlledByUser> _controlledByUser;
        private Stash<Team> _teams;
        private Stash<TankPrefabs> _tankPrefabs;

        public World World { get; set; }

        public void OnAwake() {
            filter = World.Filter.With<GameUser>().Without<UserWithTank>().Build();

            _userWithTanks = World.GetStash<UserWithTank>();
            _inTeams = World.GetStash<InTeam>();
            _controlledByUser = World.GetStash<ControlledByUser>();
            _teams = World.GetStash<Team>();
            _tankPrefabs = World.GetStash<TankPrefabs>();
        }

        public void OnUpdate(float deltaTime) {
            foreach (Entity userEntity in filter) {
                SpawnTankForUser(userEntity, out Entity tankEntity, out Transform tankTransform);

                Entity teamEntity = _inTeams.Get(userEntity, out bool isInTeam).team;
                if (isInTeam) {
                    AttachTankToTeam(tankEntity, tankTransform, teamEntity);
                } else {
                    Debug.LogError("User without team!");
                }
            }
        }

        private void SpawnTankForUser(Entity userEntity, out Entity tankEntity, out Transform tankTransform) 
        {
            var prefabs = _tankPrefabs.data.First().Data;
            int tankIndex = Random.Range(0, prefabs.Length);
            EntityProvider tankPrefab = prefabs[tankIndex];
            EntityProvider tankInstance = GameObject.Instantiate(tankPrefab);
            tankEntity = tankInstance.Entity;
            tankTransform = tankInstance.transform;
            _userWithTanks.Add(userEntity).tank = tankEntity;
            _controlledByUser.Add(tankEntity).user = userEntity;
        }

        private void AttachTankToTeam(Entity tankEntity, Transform tankTransform, Entity teamEntity) {
            ref Team team = ref _teams.Get(teamEntity);
            int spawnIndex = Random.Range(0, team.spawns.Length);
            Transform spawn = team.spawns[spawnIndex];
            tankTransform.position = spawn.position;
            tankTransform.rotation = spawn.rotation;
            _inTeams.Add(tankEntity).team = teamEntity;
        }

        public void Dispose()
        {
        }
    }
}