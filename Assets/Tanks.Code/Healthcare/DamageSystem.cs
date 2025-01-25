namespace Tanks.Healthcare
{
    using Scellecs.Morpeh;

    public sealed class DamageSystem : ISystem
    {
        public World World { get ; set; }

        Filter _damagedHealth;
        Stash<Health> _healths;
        Stash<DamageEvent> _damageEvents;
        Stash<IsDeadMarker> _deadMarkers;

        public void OnAwake()
        {
            _damagedHealth = World.Filter.With<Health>().With<DamageEvent>().Build();

            _healths = World.GetStash<Health>();
            _damageEvents = World.GetStash<DamageEvent>();
            _deadMarkers = World.GetStash<IsDeadMarker>();
        }

        public void Dispose() {}

        public void OnUpdate(float deltaTime)
        {
            foreach(var ent in _damagedHealth)
            {
                var damage = _damageEvents.Get(ent);
                var health = _healths.Get(ent);

                if (damage.amount <= 0)
                {
                    return;
                }

                health.health -= damage.amount;
                if (health.health > 0)
                {
                    return;
                }

                _deadMarkers.Add(ent);
            }
        }
    }
}