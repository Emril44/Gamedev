using System;
using UnityEngine;
public abstract class Objective : ScriptableObject
{
    public event Action onComplete;
    [Header("Basic separator is '+'")]
    public char separator = '+';
    [TextArea(1, 5)]
    [SerializeField] protected string message;

    protected void Complete()
    {
        SetActive(false);
        onComplete?.Invoke();
    }

    public abstract void SetActive(bool active);

    public string Message()
    {
        return message;
    }
    public virtual string LocalizedMessage()
    {
        return message.Split(separator)[PlayerPrefs.GetInt("Language")];
    }
}
