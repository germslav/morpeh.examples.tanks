using UnityEngine;

using Scellecs.Morpeh.Systems;
using Scellecs.Morpeh;
using Tanks.Teams;
using Tanks.Collisions;
using Tanks.Healthcare;
using Tanks.Bases;
using Tanks.GameInput;
using Tanks.Movement;
using Tanks.Scores;
using Tanks.UtilSystems;
using Tanks.Walls;
using Tanks.Weapons;
using Tanks;

public class Startup : MonoBehaviour
{
    private World world;

    private void Awake()
    {
        // Получаем или создаем мир
        this.world = World.Default ?? World.Create();

        //CreateUser();

        // Добавляем ваши системы
        var systemsGroup = this.world.CreateSystemsGroup();

        systemsGroup.AddSystem(new GameUserBalanceTeamSystem());
        systemsGroup.AddSystem(new TeamBaseInitSystem());
        systemsGroup.AddSystem(new TeamBaseDestroySystem());
        systemsGroup.AddSystem(new CollisionCleanSystem());
        systemsGroup.AddSystem(new GameInputSystem());
        systemsGroup.AddSystem(new GameUserTankCreateSystem());
        systemsGroup.AddSystem(new DamageCleanSystem());
        systemsGroup.AddSystem(new DamageSystem());
        systemsGroup.AddSystem(new DamageTextSystem());
        systemsGroup.AddSystem(new TankDestroySystem());
        systemsGroup.AddSystem(new TankMovementInitSystem());
        systemsGroup.AddSystem(new MovementSystem());
        systemsGroup.AddSystem(new TeamUserIdSystem());
        systemsGroup.AddSystem(new UserMovementSystem());
        systemsGroup.AddSystem(new ScoreSystem());
        systemsGroup.AddSystem(new WinDetectSystem());
        systemsGroup.AddSystem(new TextInWorldSystem());
        systemsGroup.AddSystem(new WallDestroySystem());
        systemsGroup.AddSystem(new BulletHitSystem());
        systemsGroup.AddSystem(new BulletWeaponSystem());
        systemsGroup.AddSystem(new UserBulletWeaponSystem());
        systemsGroup.AddSystem(new PhysicsUpdateSystem());
        systemsGroup.AddSystem(new CollisionInitSystem());

        this.world.AddSystemsGroup(order: 0, systemsGroup);
    }

    private void CreateUser()
    {
        Entity userEntity = world.CreateEntity();

        world.GetStash<GameUser>().Add(userEntity);
    }

    private void Update()
    {
        // Обновление мира
        this.world.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        // Фиксированное обновление мира
        this.world.FixedUpdate(Time.fixedDeltaTime);
    }

    private void OnDestroy()
    {
        // Уничтожаем мир при завершении
        this.world.Dispose();
    }
}
