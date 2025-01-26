namespace Tanks.Weapons
{
    using Collisions;
    using Scellecs.Morpeh;
    using Unity.IL2CPP.CompilerServices;
    using UnityEngine;

    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class BulletWeaponSystem : IFixedSystem
    {
        public World World { get; set; }

        Filter _filter;
        Stash<BulletWeapon> _bulletWeapons;
        Stash<Tank> _tanks;
        Stash<Bullet> _bullets;
        Stash<CanCollide> _canCollides;

        public void Dispose()
        {

        }

        public void OnAwake()
        {
            _filter = World.Filter.With<BulletWeapon>().With<Tank>().Build();

            _bullets = World.GetStash<Bullet>();
            _tanks = World.GetStash<Tank>();
            _bulletWeapons = World.GetStash<BulletWeapon>();
            _canCollides = World.GetStash<CanCollide>();
        }

        public void OnUpdate(float deltaTime)
        {
            foreach (var ent in _filter)
            {
                ref var weapon = ref _bulletWeapons.Get(ent);
                ref var tank = ref _tanks.Get(ent);

                if (!weapon.shoot)
                {
                    return;
                }

                if (Time.time - weapon.lastShotTime < weapon.config.reloadTime)
                {
                    return;
                }

                CreateBullet(ent, weapon, tank);
                weapon.lastShotTime = Time.time;
            }
        }

        private void CreateBullet(Entity ent, in BulletWeapon weapon, in Tank tank)
        {
            Rigidbody2D bulletBody = GameObject.Instantiate(weapon.config.bulletConfig.prefab,
                                                 tank.body.position,
                                                 Quaternion.identity);

            IgnoreSelfCollisions(bulletBody.GetComponent<Collider2D>(), ent);
            bulletBody.gameObject.SetActive(true);
            bulletBody.rotation = tank.body.rotation;
            bulletBody.velocity = Quaternion.Euler(0f, 0f, bulletBody.rotation)
                                  * Vector3.up
                                  * weapon.config.bulletSpeed;

            var newEnt = World.CreateEntity();
            _bullets.Set(newEnt, new Bullet
            {
                body = bulletBody,
                config = weapon.config.bulletConfig,
                shooter = ent,
            });
        }

        private void IgnoreSelfCollisions(Collider2D bulletCollider, Entity ent)
        {
            ref CanCollide canCollide = ref _canCollides.Get(ent, out bool hasCanCollide);
            if (bulletCollider == null || !hasCanCollide)
            {
                return;
            }

            foreach (Collider2D selfCollider in canCollide.detector.colliders)
            {
                Physics2D.IgnoreCollision(selfCollider, bulletCollider);
            }
        }
    }
}