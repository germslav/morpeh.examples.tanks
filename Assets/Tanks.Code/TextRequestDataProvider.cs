namespace Tanks
{
    using Scellecs.Morpeh;
    using Scellecs.Morpeh.Providers;
    using System;
    using Tanks.UtilSystems;
    using Unity.IL2CPP.CompilerServices;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public class TextRequestDataProvider : MonoProvider<TextRequestData> { }

    [Serializable]
    public struct TextRequestData : IComponent
    {
        public Request Request;
    }
}

