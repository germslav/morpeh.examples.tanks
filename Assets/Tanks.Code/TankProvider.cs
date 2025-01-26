﻿namespace Tanks
{
    using System;
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Providers;
    using Tanks.UtilSystems;
    using UnityEngine;

    [AddComponentMenu("Tanks/Tank")]
    [RequireComponent(typeof(Rigidbody2D))]
    public class TankProvider : MonoProvider<Tank> { }

    [Serializable]
    public struct Tank : IComponent {
        public TankConfig config;
        public Rigidbody2D body;
        public Vector2 userTextOffset;
    }
}

