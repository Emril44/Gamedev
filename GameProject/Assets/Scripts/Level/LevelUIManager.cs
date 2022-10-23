using UnityEngine;

public class LevelUIManager : MonoBehaviour
{
    public static LevelUIManager Instance { get; private set; }
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


    public void ShowPauseScreen() //aka Menu Options
    {
        //...
    }

    public void ShowSaveScreen() //SavesNew Canvas
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

    public void ShowAlbumScreen()
    {
        //...
    }

    public void ShowQuitScreen()
    {
        //...
    }

    //HUD stuff
    //NPC dialogs
}
