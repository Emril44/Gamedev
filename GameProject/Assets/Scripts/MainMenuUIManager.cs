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
    
    public void ShowSavesMenu()
    {
        //...
    }

    public void ShowStartGameMenu()
    {
        //...
    }

    public void ShowSettingsMenu()
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
