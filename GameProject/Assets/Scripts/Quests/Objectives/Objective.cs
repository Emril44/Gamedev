using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class Objective : ScriptableObject
{
    public event Action onUpdate;
    public event Action onComplete;
    [Header("Basic separator is '+'")]
    public char separator = '+';
    [TextArea(1, 5)]
    [SerializeField] protected string message;

    private void Awake()
    {
        SceneManager.sceneUnloaded += (Scene scene) =>
        {
            onUpdate = null;
            onComplete = null;
        };
    }

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
