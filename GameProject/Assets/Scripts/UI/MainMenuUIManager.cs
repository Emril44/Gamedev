using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{   
    public static MainMenuUIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    
    public void ShowNewGameScreen() //SavesNew Canvas
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void ShowLoadScreen() //SavesLoad Canvas
    {
        //...
    }

    public void ShowSettingsScreen()
    {
        //...
    }

    public void ShowAboutScreen()
    {
        //...
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
