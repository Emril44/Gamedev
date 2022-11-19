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
