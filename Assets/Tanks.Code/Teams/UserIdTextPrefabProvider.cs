namespace Tanks.Teams
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Providers;
    using System;
    using UnityEngine;

    [Serializable]
    public struct UserIdTextPrefab : IComponent
    {
        public TextMesh go;
    }

    public sealed class UserIdTextPrefabProvider : MonoProvider<UserIdTextPrefab>
    {

    }
}
