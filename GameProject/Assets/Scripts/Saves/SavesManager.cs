using UnityEngine;

public class SavesManager : MonoBehaviour
{
    public int SparksAmount { get; private set; }
    public int UnlockedColors { get; private set; }
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
    
    public void Load()
    {
        SparksAmount = 0;
        UnlockedColors = 0;
    }

    public void Save()
    {
        
    }

    public void AddSpark()
    {
        SparksAmount ++;
        if(SparksAmount == 3)
        {
            UnlockColor();
            SparksAmount = 0;
        }
    }

    public void UnlockColor()
    {
        UnlockedColors ++;
    }

    public void Die()
    {
        //
    }
}
