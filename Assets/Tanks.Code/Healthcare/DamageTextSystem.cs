namespace Tanks.Healthcare {
    using Scellecs.Morpeh;
    using System.Linq;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;
    using UtilSystems;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class DamageTextSystem : ISystem
    {
        private const string FORMAT = "0.#";

        private Filter damageEvents;
        private Stash<DamageEvent> _stashDamages;
        private Stash<Health> _healthies;
        private Stash<IsDeadMarker> _deadMarkers;
        private Stash<Request> _requests;
        private Stash<TextRequestData> _data;

        public World World { get; set; }

        public void OnAwake() {
            damageEvents = World.Filter.With<DamageEvent>().With<Health>().Build();

            _stashDamages = World.GetStash<DamageEvent>();
            _healthies = World.GetStash<Health>();
            _deadMarkers = World.GetStash<IsDeadMarker>();
            _requests = World.GetStash<Request>();
            _data = World.GetStash<TextRequestData>();
        }

        public void OnUpdate(float deltaTime) {
            CreateNewTexts();
        }

        private void CreateNewTexts() {
            foreach (Entity entity in damageEvents) {
                ref DamageEvent damage = ref _stashDamages.Get(entity);
                if (!damage.hitPosition.HasValue) {
                    continue;
                }

                string text;
                if (_deadMarkers.Has(entity)) 
                {
                    text = "IsDead";
                } 
                else 
                {
                    ref Health health = ref _healthies.Get(entity);
                    text = $"{health.health.ToString(FORMAT)}HP (-{damage.amount.ToString(FORMAT)})";
                }

                SpawnTextInWorld(damage.hitPosition.Value, text);
            }
        }

        private void SpawnTextInWorld(in Vector3 hitPosition, string text) {
            Request request = _data.data.First().Request;
            request.start = hitPosition;
            request.text = text;

            _requests.Add(World.CreateEntity(), in request);
        }

        public void Dispose()
        {

        }
    }
}