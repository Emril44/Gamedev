using System;
using UnityEngine;
public abstract class Objective : ScriptableObject
{
    public event Action onUpdate;
    public event Action onComplete;
    [Header("Basic separator is '+'")]
    public char separator = '+';
    [TextArea(1, 5)]
    [SerializeField] protected string message;

    protected void Complete()
    {
        SetActive(false);
        Update();
        onComplete?.Invoke();
    }

    protected void Update()
    {
        onUpdate?.Invoke();
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
