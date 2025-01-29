using Scellecs.Morpeh;
using Tanks.GameInput;
using UnityEngine;
using UnityEngine.InputSystem;

public sealed class GameInputSystem : ISystem
{
    private int _userCounter;
    private Filter _users;
    private Stash<GameUser> _gameUsers;

    private InputActions _inputActions; // Основной InputActions

    public World World { get; set; }

    public void OnAwake()
    {
        _gameUsers = World.GetStash<GameUser>().AsDisposable();
        _users = World.Filter.With<GameUser>().Build();
        _userCounter = 0;

        // Создаём общий InputActions
        _inputActions = new InputActions();
        _inputActions.Tank.Enable(); // Включаем управление для Player1
        _inputActions.Tank1.Enable(); // Включаем управление для Player2

        // Добавляем игроков с разными ActionMap
        AddKeyboardPlayer("Player 1", _inputActions.Tank);
        AddKeyboardPlayer("Player 2", _inputActions.Tank1);
    }

    public void OnUpdate(float deltaTime)
    {
        InputSystem.Update(); // Обновляем систему ввода

        // Проверяем ввод для каждого игрока
        foreach (var entity in _users)
        {
            ref var user = ref _gameUsers.Get(entity);
        }
    }

    public void Dispose()
    {
        _gameUsers.Dispose();
    }

    private void AddKeyboardPlayer(string playerName, InputActionMap actionMap)
    {
        Entity userEntity = World.CreateEntity();
        ref GameUser user = ref _gameUsers.Add(userEntity);
        user.id = ++_userCounter;
        user.InputActions = _inputActions;
        user.inputActionsMap = actionMap;       // Уникальный ActionMap для игрока

        Debug.Log($"{playerName} added with controls: {actionMap.name}");
    }
}
