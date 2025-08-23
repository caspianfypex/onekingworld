using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdManager : MonoBehaviour
{

    void Awake()
    {
        GameMonetize.OnResumeGame += ResumeGame;
        GameMonetize.OnPauseGame += PauseGame;
    }

    void OnDestroy()
    {
        GameMonetize.OnResumeGame -= ResumeGame;
        GameMonetize.OnPauseGame -= PauseGame;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
    }


}
