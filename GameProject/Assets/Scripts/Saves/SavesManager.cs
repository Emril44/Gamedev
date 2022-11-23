using UnityEngine;

public class SavesManager : MonoBehaviour
{
    public static SavesManager Instance { get; private set; }
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

    public bool HasSaves()
    {
        if (Autosave() != null)
        {
            return true;
        }
        foreach(var save in Saves())
        {
            if (save != null)
            {
                return true;
            }
        }
        return false;
    }
    
    public Save Autosave()
    {
        return new Save(17252, 34, 247);
    }
    
    public Save[] Saves()
    {
        return new Save[] { null, null, new Save(1722, 4, 27) };
    }

    public void NewGame(int i)
    {
        
    }

    public void RemoveSave(int i)
    {
        
    }

    public void Load(int i)
    {
        // ...
    }

    public void Save()
    {
        // ...
    }

    public void Die()
    {
        //
    }
}
