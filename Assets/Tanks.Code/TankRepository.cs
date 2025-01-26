namespace Tanks {
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Providers;
    using System;
    using UnityEngine;

    [Serializable]
    public struct TankPrefabs : IComponent
    {
        public EntityProvider[] Data;
    }

    public sealed class TankRepository : MonoProvider<TankPrefabs> 
    { 
    }
}