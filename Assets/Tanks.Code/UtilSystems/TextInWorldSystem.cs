namespace Tanks.UtilSystems {
    using System;
    using System.Collections.Generic;
    using Helpers;
    using Scellecs.Morpeh;

    using TriInspector;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    [CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TextInWorldSystem))]
    public sealed class TextInWorldSystem : ILateSystem
    {
        private readonly Stack<TextMesh> textMeshes = new Stack<TextMesh>(8);
        private Filter requests;
        private Filter texts;
        private Stash<TextInWorld> _textInWorlds;

        public World World { get; set; }

        public void OnAwake() {
            texts = World.Filter.With<TextInWorld>().Build();
            requests = World.Filter.With<Request>().Build();

            _textInWorlds = World.GetStash<TextInWorld>();
        }

        public void OnUpdate(float deltaTime) {
            ProcessTexts(deltaTime);
            ProcessRequests();
        }

        private void ProcessTexts(in float deltaTime) {
            foreach (Entity entity in texts) {
                ref TextInWorld text = ref _textInWorlds.Get(entity);
                text.timeToDestroy -= deltaTime;

                if (text.timeToDestroy > 0) {
                    text.mesh.transform.position += deltaTime * text.velocity;
                    continue;
                }

                text.renderer.forceRenderingOff = true;
                text.mesh.font = null;
                textMeshes.Push(text.mesh);
                _textInWorlds.Remove(entity);
            }
        }

        private void ProcessRequests() {
            Stash<Request> cache = World.GetStash<Request>();
            foreach (Entity entity in requests) {
                SpawnTextInWorld(cache.Get(entity));
            }

            
            requests.RemoveComponentForAll<Request>(World);
        }

        private void SpawnTextInWorld(in Request request) {
            TextMesh textMesh;
            Renderer renderer;

            if (textMeshes.Count > 0) {
                textMesh = textMeshes.Pop();
                renderer = textMesh.GetComponent<Renderer>();
                renderer.forceRenderingOff = false;
            } else {
                var gameObject = new GameObject("TextInWorld");
                renderer = gameObject.AddComponent<MeshRenderer>();
                textMesh = gameObject.AddComponent<TextMesh>();
            }

            renderer.sortingOrder = 1000;
            textMesh.transform.position = request.start;
            textMesh.alignment = TextAlignment.Center;
            textMesh.anchor = TextAnchor.MiddleCenter;
            textMesh.characterSize = request.charSize;
            textMesh.fontSize = request.fontSize;
            textMesh.font = request.font;
            textMesh.color = request.color;
            textMesh.text = request.text;

            var ent = World.CreateEntity();
            _textInWorlds.Set(ent, new TextInWorld {
                    mesh = textMesh,
                    renderer = renderer,
                    timeToDestroy = request.duration,
                    velocity = request.velocity,
            });
        }


        public void Dispose() {
            foreach (TextMesh textMesh in textMeshes) {
                if (textMesh != null) {
                    GameObject.Destroy(textMesh.gameObject);
                }
            }

            textMeshes.Clear();
        }

        [Serializable]
        public struct Request : IComponent {
            [Required] public Font font;
            public int fontSize;
            public float charSize;
            public Color color;

            [Space] public Vector3 start;
            public string text;
            public Vector3 velocity;
            [Min(0.5f)] public float duration;
        }

        private struct TextInWorld : IComponent {
            public TextMesh mesh;
            public Renderer renderer;
            public float timeToDestroy;
            public Vector3 velocity;
        }
    }
}

namespace Helpers
{
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;
    using Scellecs.Morpeh;

    public static class FilterHelperExtensions
    {
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveComponentForAll<T>(this Filter filter, World world)
                where T : struct, IComponent
        {
            Stash<T> stash = world.GetStash<T>();
            foreach (Entity ent in filter)
            {
                stash.Remove(ent);
            }
        }

        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveAllEntities(this Filter filter, World world)
        {
            foreach (Entity ent in filter)
            {
                world.RemoveEntity(ent);
            }
        }
    }
}

namespace Helpers
{
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;
    using Scellecs.Morpeh;
    public static class StashHelperExtensions
    {
        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AddOrGet<T>(this Stash<T> stash, Entity entity)
                 where T : struct, IComponent
        {
            var world = entity.GetWorld();//.ThreadSafetyCheck()

#if MORPEH_DEBUG
            if (entity.IsNullOrDisposed()) {
                throw new Exception($"[MORPEH] You are trying Add on null or disposed entity");
            }
#endif
            var c = stash.Get(entity, out bool exist);

            if (!exist)
            {
                return ref stash.Add(entity);
            }

#if MORPEH_DEBUG
            MLogger.LogError($"You're trying to add on entity {entity.entityId.id} a component that already exists! Use Get or Set instead!");
#endif
            return ref stash.Get(entity);
        }
    }
}


namespace Helpers
{
    using System;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;
    using Scellecs.Morpeh;

    public static class EntityHelperExtensions
    {
        [PublicAPI]
        [Obsolete("Use " + nameof(EntityExtensions.IsNullOrDisposed))]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Exists(this Entity entity)
        {
            return entity != null && !entity.IsDisposed();
        }

        [PublicAPI]
        [Obsolete("Use " + nameof(AddOrGet) + " instead")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T GetOrCreate<T>(this Entity entity)
                where T : struct, IComponent
        {
            if (entity.Has<T>())
            {
                return ref entity.GetComponent<T>();
            }

            return ref entity.AddComponent<T>();
        }

        [PublicAPI]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T AddOrGet<T>(this Entity entity, World world)
                where T : struct, IComponent
        {
#if MORPEH_DEBUG
            if (entity.IsNullOrDisposed()) {
                throw new System.Exception($"[MORPEH] You are trying {nameof(AddOrGet)} on null or disposed entity {entity.entityId.id}");
            }
#endif

            return ref world.GetStash<T>().AddOrGet(entity);
        }
    }
}
