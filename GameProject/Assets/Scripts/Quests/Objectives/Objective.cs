using System;
using UnityEngine;
public abstract class Objective : ScriptableObject
{
    public event Action onComplete;

    [SerializeField] protected string message;

    protected void Complete()
    {
        SetActive(false);
        onComplete?.Invoke();
    }

    public abstract void SetActive(bool active);

    public virtual string GetMessage()
    {
        return message;
    }
}
