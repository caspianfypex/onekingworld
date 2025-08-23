using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class GameMonetize : MonoBehaviour
{
    public static GameMonetize Instance;

    public string GAME_ID = "YOUR_GAME_ID_HERE";

    public static Action OnResumeGame;
    public static Action OnPauseGame;

    [DllImport("__Internal")]
    private static extern void InitApi(string gameKey);

    [DllImport("__Internal")]
    private static extern void ShowBanner();

    void Awake()
    {
        if (GameMonetize.Instance == null)
            GameMonetize.Instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);

        Init();
    }

    void Init()
    {
        try
        {
            InitApi(GAME_ID);
        }
        catch (EntryPointNotFoundException e)
        {
            Debug.LogWarning("Initialization failed. Make sure you are running a WebGL build in a browser");
        }
    }
    
    internal void ShowAd()
    {
        try
        {
            ShowBanner();
        }
        catch (EntryPointNotFoundException e)
        {
            Debug.LogWarning("ShowBanner failed. Make sure you are running a WebGL build in a browser");
        }
    }

    /// <summary>
    /// Resume the game, this method is used when an ad has been showed
    /// </summary>
    void ResumeGame()
    {
       if (OnResumeGame != null) OnResumeGame();
    }

    /// <summary>
    /// Pause the game, this method is used when we show an ad
    /// </summary>
    void PauseGame()
    {
        if(OnPauseGame != null) OnPauseGame();
    }
}
