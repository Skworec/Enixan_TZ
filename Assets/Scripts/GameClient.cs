using Assets.Scripts.Managers;
using Assets.Scripts.Managers.Interfaces;
using System;
using System.Collections.Generic;

public class GameClient : ISystem
{
    private static object _sync = new object();
    private static IDictionary<Type, ISystem> _systems;

    private static GameClient _Instance;
    public static GameClient Instance
    {
        get
        {
            if (_Instance == null)
            {
                lock (_sync)
                {
                    _Instance = new GameClient();
                }
            }
            return _Instance;
        }
    }

    public void Dispose()
    {
        foreach (var system in _systems)
            system.Value.Dispose();
    }

    public void Init()
    {
        _systems = new Dictionary<Type, ISystem>();

        _systems.Add(typeof(ISettingsManager), new SettingsManager());
        _systems.Add(typeof(IUIManager), new UIManager());
        _systems.Add(typeof(IPlaneManager), new PlaneManager());
        _systems.Add(typeof(ICameraManager), new CameraManager());
        _systems.Add(typeof(IBuildManager), new BuildManager());

        foreach (ISystem system in _systems.Values)
        {
            system.Init();
        }
        Get<IUIManager>().Camera.GetComponent<gridOverlay>().enabled = true;
    }

    public void Update()
    {
        foreach (var system in _systems)
            system.Value.Update();
    }

    public static T Get<T>()
    {
        try
        {
            return (T)_systems[typeof(T)];
        }
        catch (KeyNotFoundException)
        {
            throw new Exception("System " + typeof(T) + " isn't registered!");
        }
    }
}