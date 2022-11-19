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

    public int SavesAmount()
    {
        return 0;
    }
    
    public Save Autosave()
    {
        return new Save(2345, 3, 11);
    }

    public Save[] Saves()
    {
        return new Save[] { new Save(1114, 2, 5), new Save(1722, 4, 27) };
    }
    
    public void Load()
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
