namespace Tanks.Weapons {
    using Collisions;
    using Healthcare;
    using Scellecs.Morpeh;

    using Teams;
    using UnityEngine;

    public sealed class BulletHitSystem : ISystem
    {
        public World World { get; set; }

        private Filter _filter;
        private Stash<CollisionEvent> _collisions;
        private Stash<Bullet> _bullets;
        private Stash<DamageEvent> _damageEvents;

        public void Dispose()
        {

        }

        public void OnAwake()
        {
            _filter = World.Filter.With<CollisionEvent>().Build();

            _collisions = World.GetStash<CollisionEvent>();
            _bullets = World.GetStash<Bullet>();
            _damageEvents = World.GetStash<DamageEvent>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach(var ent in _filter)
            {
                ref var colEv = ref _collisions.Get(ent);
                Entity bulletEntity = colEv.first;

                if (bulletEntity.IsDisposed()) continue;

                Bullet bullet = _bullets.Get(bulletEntity, out bool isBullet);
                if (!isBullet)
                {
                    continue;
                }

                if (!colEv.second.IsDisposed() && colEv.second != null && !colEv.second.InSameTeam(bullet.shooter))
                {
                    _damageEvents.Set(colEv.second, new DamageEvent
                    {
                        hitPosition = colEv.collision?.GetContact(0).point,
                        amount = bullet.config.damage,
                        dealer = bullet.shooter,
                    });                
                }

                GameObject.Destroy(bullet.body.gameObject);
                _bullets.Remove(bulletEntity);
                World.RemoveEntity(bulletEntity);
            }
        }
    }
}