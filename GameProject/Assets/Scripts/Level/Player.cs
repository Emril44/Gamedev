using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private GameManager gameManager;
    void PutPrism(PrismPiece prismPiece)
    {
        gameManager.SetNewColor(prismPiece);
    }
}
