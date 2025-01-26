using Scellecs.Morpeh;
using Scellecs.Morpeh.Providers;
using System;
using Tanks.UtilSystems;
using Unity.IL2CPP.CompilerServices;
using UnityEditor;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
public sealed class MessagesProvider : MonoProvider<MessagesData> 
{

}

[Serializable]
public struct MessagesData : IComponent
{
    public Request DamageTextMessage;
    public Request KillMessage;
}