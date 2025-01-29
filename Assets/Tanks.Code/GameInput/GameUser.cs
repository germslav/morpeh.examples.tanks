namespace Tanks.GameInput {
    using System;
    using Scellecs.Morpeh;
    using UnityEngine.InputSystem;
    using UnityEngine.InputSystem.Users;

    [Serializable]
    public struct GameUser : IComponent, IDisposable {
        public InputActions InputActions;
        public InputActionMap inputActionsMap;
        public InputUser user;
        public int id;

        public void Dispose() {
            InputActions?.Disable();

            if (!user.valid) {
                return;
            }

            user.UnpairDevicesAndRemoveUser();
        }
    }
}