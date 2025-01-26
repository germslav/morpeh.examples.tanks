namespace Tanks.Collisions {
    using Bases;
    using Scellecs.Morpeh;

    using UnityEngine;
    using Walls;
    using Weapons;

    public sealed class CollisionInitSystem : IFixedSystem {
        private Filter bases;
        private Filter bullets;
        private Filter tanks;
        private Filter walls;

        private Stash<TeamBase> _baseStashes;
        private Stash<Bullet> _bullets;
        private Stash<Wall> _walls;
        private Stash<Tank> _tank;
        private Stash<CanCollide> _canCollide;

        public World World { get; set; }

        public void OnAwake() {
            tanks = World.Filter.With<Tank>().Without<CanCollide>().Build();
            bullets = World.Filter.With<Bullet>().Without<CanCollide>().Build();
            walls = World.Filter.With<Wall>().Without<CanCollide>().Build();
            bases = World.Filter.With<TeamBase>().Without<CanCollide>().Build();

            _baseStashes = World.GetStash<TeamBase>();
            _bullets = World.GetStash<Bullet>();
            _walls = World.GetStash<Wall>();
            _tank = World.GetStash<Tank>();
            _canCollide = World.GetStash<CanCollide>();
        }

        public void OnUpdate(float deltaTime) {
            ProcessTanks();
            ProcessBullets();
            ProcessWalls();
            ProcessBases();
        }

        private void ProcessTanks() {
            foreach (Entity ent in tanks) {
                ref Tank tank = ref _tank.Get(ent);

                MakeCanCollide(ent, tank.body.gameObject);
            }
        }

        private void ProcessBullets() {
            foreach (Entity ent in bullets) {
                ref Bullet bullet = ref _bullets.Get(ent);
                MakeCanCollide(ent, bullet.body.gameObject);
            }
        }

        private void ProcessWalls() {
            foreach (Entity ent in walls) {
                ref Wall wall = ref _walls.Get(ent);
                MakeCanCollide(ent, wall.transform.gameObject);
            }
        }

        private void ProcessBases() {
            foreach (Entity ent in bases) {
                ref TeamBase teamBase = ref _baseStashes.Get(ent);
                MakeCanCollide(ent, teamBase.view.gameObject);
            }
        }

        private void MakeCanCollide(Entity entity, GameObject gameObject) {
            ref CanCollide canCollide = ref _canCollide.Add(entity);
            canCollide.detector = gameObject.AddComponent<CollisionDetector>();
            canCollide.detector.Init(World);
            canCollide.detector.listener = entity;
        }

        public void Dispose()
        {
        }
    }
}