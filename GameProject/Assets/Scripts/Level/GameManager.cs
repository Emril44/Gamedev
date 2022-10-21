using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EnvironmentController environmentController;

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
        environmentController.SetNewColor(prismFragment.getColor());
    }

    public void AddSparks(int sparksAmount)
    {
        PlayerPrefs.SetInt("Sparks", PlayerPrefs.GetInt("Sparks") + sparksAmount);
    }
}
