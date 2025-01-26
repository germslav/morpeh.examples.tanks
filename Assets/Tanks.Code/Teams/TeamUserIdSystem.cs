using System.Runtime.InteropServices;

namespace Tanks.Teams
{
    using System;
    using System.Linq;
    using GameInput;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Systems;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class TeamUserIdSystem : ILateSystem 
    {
        private Filter tanksToDisplay;
        private Stash<UserIdText> _userIdTexts;
        private Stash<ControlledByUser> _controlledByUser;
        private Stash<GameUser> _gameUser;
        private Stash<Tank> _tanks;
        private Stash<InTeam> _inTeam;
        private Stash<Team> _teams;
        private Stash<UserIdTextPrefab> _textPrefabs;
        public World World { get; set; }

        public void OnAwake() {
            _userIdTexts = World.GetStash<UserIdText>().AsDisposable();
            _controlledByUser = World.GetStash<ControlledByUser>();
            _gameUser = World.GetStash<GameUser>().AsDisposable();
            _tanks = World.GetStash<Tank>();
            _inTeam = World.GetStash<InTeam>();
            _teams = World.GetStash<Team>();
            _textPrefabs = World.GetStash<UserIdTextPrefab>();

            tanksToDisplay = World.Filter.With<Tank>().With<ControlledByUser>().Without<UserIdText>().Build();
        }

        public void OnUpdate(float deltaTime) {
            foreach (Entity tankEntity in tanksToDisplay) {
                Entity userEntity = _controlledByUser.Get(tankEntity).user;
                if (World.IsDisposed(userEntity)) {
                    continue;
                }

                ref Tank tank = ref _tanks.Get(tankEntity);
                ref GameUser user = ref _gameUser.Get(userEntity);

                ref UserIdText userIdText = ref _userIdTexts.Add(tankEntity);

                userIdText.text = GameObject.Instantiate(_textPrefabs.data.First().go, tank.body.transform);
                userIdText.text.GetComponent<Renderer>().sortingOrder = 10;
                userIdText.text.transform.localPosition = tank.userTextOffset;
                userIdText.text.text = $"#{user.id.ToString()}";
                userIdText.text.color = Color.white;

                ref InTeam inTeam = ref _inTeam.Get(tankEntity, out bool isInTeam);
                if (isInTeam) {
                    userIdText.text.color = _teams.Get(inTeam.team).color;
                }
            }
        }

        public void Dispose() {}

        private struct UserIdText : IComponent, IDisposable {
            public TextMesh text;

            public readonly void Dispose() {
                if (text != null) {
                    GameObject.Destroy(text.gameObject);
                }
            }
        }
    }
}