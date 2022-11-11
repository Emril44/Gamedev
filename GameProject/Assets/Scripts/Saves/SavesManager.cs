using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavesManager : MonoBehaviour
{
    public int sparksAmount { get; private set; }
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
        sparksAmount = 0;
    }

    public void Save()
    {
        
    }

    public void AddSpark()
    {
        sparksAmount ++;
    }

    public void Die()
    {
        //
    }
}
