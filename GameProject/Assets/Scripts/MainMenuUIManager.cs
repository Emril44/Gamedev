using UnityEngine;

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
        //...
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

    public void ShowQuitScreen()
    {
        //...
    }

    
    public void ExitGame()
    {
        //...
    }

    public void StartGame()
    {
        //...
    }
}
