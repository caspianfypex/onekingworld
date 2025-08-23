using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject settingsMenu;
    public Slider musicSlider;
    public Slider sfxSlider;
    public AudioMixer musicMixer;
    public AudioMixer SFXMixer;
    public GameObject pauseMenu;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            Debug.Log("sa");
            float value;
            bool a = musicMixer.GetFloat("musicVolume", out value);
            musicSlider.value = value;
            float value2;
            bool a2 = SFXMixer.GetFloat("SFXVolume", out value2);
            sfxSlider.value = value2;
            PlayerPrefs.SetFloat("musicVolume", value);
            PlayerPrefs.SetFloat("sfxVolume", value2);
        }
        else
        {
            Debug.Log("as");
            musicMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("musicVolume"));
            SFXMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("sfxVolume"));
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }
    }

    public static void MainMenuSceneLoad()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public static void GameSceneLoad()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }


    public static void QuitGame()
    {
        Application.Quit();
    }

    public void SetMusicSound(float f)
    {
        PlayerPrefs.SetFloat("musicVolume", f);
        musicMixer.SetFloat("musicVolume", f);
    }

    public void SetSFXSound(float f)
    {
        PlayerPrefs.SetFloat("sfxVolume", f);
        SFXMixer.SetFloat("SFXVolume", f);
    }

    public void OpenMainMenu()
    {
        if (settingsMenu)
        {
            settingsMenu.SetActive(false);
        }
        mainMenu.SetActive(true);
    }

    public void OpenSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
}
