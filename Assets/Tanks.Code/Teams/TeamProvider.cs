﻿namespace Tanks.Teams {
    using System;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Providers;
    using UnityEngine;

    public sealed class TeamProvider : MonoProvider<Team> { }

    [Serializable]
    public struct Team : IComponent {
        public string name;
        public Color color;
        public int userCount;
        public Transform[] spawns;
    }
}