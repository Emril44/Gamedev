using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
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
    
    public enum KeyColor
    {
        Red,
        Green,
        Yellow,
        Blue
        //...
    }

    public void SetNewColor(PrismFragment prismFragment)
    {
        EnvironmentController.Instance.SetNewColor(prismFragment.getColor());
    }

    public void AddSparks(int sparksAmount)
    {
        PlayerPrefs.SetInt("Sparks", PlayerPrefs.GetInt("Sparks") + sparksAmount);
    }
}
