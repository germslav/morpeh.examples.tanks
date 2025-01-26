namespace Tanks.GameInput {
    using System;
    using Scellecs.Morpeh;

    using UnityEngine;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.Controls;
    using UnityEngine.InputSystem.LowLevel;
    using UnityEngine.InputSystem.Users;

    public sealed class GameInputSystem : ISystem {
        private Action<InputControl, InputEventPtr> unpairedDeviceUsedDelegate;

        private int _userCounter;
        private Filter _users;
        private Stash<GameUser> _gameUsers;

        public World World { get; set; }

        public void OnAwake() {
            _gameUsers = World.GetStash<GameUser>().AsDisposable();
            _users = World.Filter.With<GameUser>().Build();
            _userCounter = 0;

            unpairedDeviceUsedDelegate = OnUnpairedDeviceUsed;
            ++InputUser.listenForUnpairedDeviceActivity;
            InputUser.onUnpairedDeviceUsed += unpairedDeviceUsedDelegate;
        }

        public void OnUpdate(float deltaTime) {
            InputSystem.Update();
        }

        public void Dispose() {
            InputUser.onUnpairedDeviceUsed -= unpairedDeviceUsedDelegate;
            --InputUser.listenForUnpairedDeviceActivity;

            _gameUsers.Dispose();
        }

        private void OnUnpairedDeviceUsed(InputControl control, InputEventPtr eventPtr) {
            if (!(control is ButtonControl)) {
                return;
            }

            var actions = new InputActions();
            if (!actions.CommonScheme.SupportsDevice(control.device)) {
                return;
            }

            Entity userEntity = World.CreateEntity();
            ref GameUser user = ref _gameUsers.Add(userEntity);
            user.id = ++_userCounter;
            user.device = control.device;
            Debug.Log($"{user.device} (Id={user.id.ToString()}) connected!");

            user.inputActions = actions;
            user.inputActions.Enable();

            user.user = InputUser.PerformPairingWithDevice(control.device);
            user.user.ActivateControlScheme(actions.CommonScheme);
            user.user.AssociateActionsWithUser(actions);
        }
    }
}