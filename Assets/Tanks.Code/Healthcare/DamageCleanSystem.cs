namespace Tanks.Healthcare {
    using Scellecs.Morpeh;

    public sealed class DamageCleanSystem : ILateSystem 
    {
        private Filter filter;
        private Stash<DamageEvent> _stash;

        public World World { get; set; }

        public void OnAwake() {
            filter = World.Filter.With<DamageEvent>().Build();
            _stash = World.GetStash<DamageEvent>();
        }

        public void OnUpdate(float deltaTime) {
            foreach (Entity ent in filter) {
                _stash.Remove(ent);
            }
        }

        public void Dispose()
        {

        }
    }
}