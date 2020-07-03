using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class MainApp : MonoBehaviour
{
    public delegate void MainAppDelegate(object param);
    public event MainAppDelegate OnLevelWasLoadedEvent;

    public event Action LateUpdateEvent;
    public event Action FixedUpdateEvent;

    private static MainApp _Instance;
    public static MainApp Instance
    {
        get { return _Instance; }
        private set { _Instance = value; }
    }

    private bool _isLevelLoaded;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        _isLevelLoaded = true;

        if (Instance == this)
            GameClient.Instance.Init();
    }

    private void Update()
    {
        if (Instance == this && _isLevelLoaded)
            GameClient.Instance.Update();
    }

    private void LateUpdate()
    {
        if (Instance == this)
        {
            if (LateUpdateEvent != null)
                LateUpdateEvent();
        }
    }

    private void FixedUpdate()
    {
        if (Instance == this)
        {
            if (FixedUpdateEvent != null)
                FixedUpdateEvent();
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            GameClient.Instance.Dispose();
    }

    private void OnLevelWasLoaded(int index)
    {
        if (Instance == this)
        {
            if (OnLevelWasLoadedEvent != null)
                OnLevelWasLoadedEvent(index);
        }
    }

    public void ReloadScene()
    {
        _isLevelLoaded = false;
        GameClient.Instance.Dispose();
        StartCoroutine(LoadLevelAsync("Main"));
    }

    private IEnumerator LoadLevelAsync(string levelName)
    {
        AsyncOperation asyncOperation = Application.LoadLevelAsync(levelName);
        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        GameClient.Instance.Init();
        _isLevelLoaded = true;
    }

    private void ProcessCloseApp()
    {
        Debug.Log("Close");
        Application.Quit();
    }
}