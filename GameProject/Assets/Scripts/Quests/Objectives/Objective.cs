using System;
using UnityEngine;
public abstract class Objective : ScriptableObject
{
    public event Action onComplete;

    [field: SerializeField] public string Message { get; private set; }

    protected void Complete()
    {
        SetActive(false);
        onComplete?.Invoke();
    }

    public abstract void SetActive(bool active);
}
