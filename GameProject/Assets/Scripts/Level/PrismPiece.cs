using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrismPiece : MonoBehaviour
{
    [SerializeField] private GameManager.KeyColor keyColor;

    public GameManager.KeyColor getColor
    {
        get { return keyColor; }
    }
}
