using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoredObstacle : MonoBehaviour
{
    [SerializeField] private GameManager.KeyColor keyColor;

    public GameManager.KeyColor getColor
    {
        get { return keyColor; }
    }
}
