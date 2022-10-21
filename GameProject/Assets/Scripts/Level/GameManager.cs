using System.Collections;
using System.Collections.Generic;
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
    }

    public void SetNewColor(PrismPiece prismPiece)
    {
        environmentController.SetNewColor(prismPiece);
    }
}
