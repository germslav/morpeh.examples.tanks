namespace Tanks.Scores {
    using GameInput;
    using Helpers;
    using Scellecs.Morpeh;
    using System.Linq;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;
    using UtilSystems;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class ScoreSystem : ILateSystem 
    {
        private Filter usersToInit;
        private Filter killEvents;

        private Stash<UserScores> _userScores;
        private Stash<ControlledByUser> _controlledByUsers;
        private Stash<Tank> _tanks;
        private Stash<Request> _requests;
        private Stash<MessagesData> _data;

        public World World { get; set; }

        public void Dispose()
        {
        }

        public void OnAwake() 
        {
            usersToInit = World.Filter.With<GameUser>().Without<UserScores>().Build();
            killEvents = World.Filter.With<OneMoreKillEvent>().With<ControlledByUser>().Build();

            _userScores = World.GetStash<UserScores>();
            _controlledByUsers = World.GetStash<ControlledByUser>();
            _tanks = World.GetStash<Tank>();
            _requests = World.GetStash<Request>();
            _data = World.GetStash<MessagesData>();
        }

        public void OnUpdate(float deltaTime) 
        {
            foreach (Entity entity in usersToInit) 
            {
                _userScores.Add(entity);
            }

            World.Commit();
            ScoreKills();
        }

        private void ScoreKills() {
            foreach (Entity entity in killEvents) {
                Entity userEntity = _controlledByUsers.Get(entity).user;
                ref UserScores scores = ref _userScores.Get(userEntity);
                scores.totalKills++;

                ref Tank tank = ref _tanks.Get(entity, out bool isTank);
                if (!isTank) {
                    Debug.LogError("Able to show kill messages only for Tanks!");
                    continue;
                }

                Request request = _data.data.First().KillMessage;
                request.start = tank.body.position;
                request.text = $"{scores.totalKills.ToString()} kills";

                _requests.Add(World.CreateEntity(), in request);
            }

            killEvents.RemoveComponentForAll<OneMoreKillEvent>(World);
        }
    }
}