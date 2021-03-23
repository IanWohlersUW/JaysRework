using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void Awake()
    {
        // Use PlayerPrefs instead of an unlocker
        // Unlocker.InitializeUnlocker();
    }

    public void Play()
    {
        Debug.Log("Level Select");
        // Load level select scene
        // fader.FadeToLevel("World_1");
    }

    public void Settings()
    {
        Debug.Log("Settings");
        // fader.FadeToLevel("Settings");
    }
    public void Menu()
    {
        Debug.Log("Menu");
        // SceneManager.LoadScene("MainMenu");
    }
    public void Mute()
    {
        Debug.Log("Mute!");
        AudioListener.volume = AudioListener.volume > 0 ? 0 : 1;
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
