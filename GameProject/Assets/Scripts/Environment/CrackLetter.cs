using System;
using UnityEngine;

public class CrackLetter : MonoBehaviour
{
    [Range(1, 3)]
    [SerializeField] private int day;
    [NonSerialized] public bool Touched;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Touched) return;
            Touched = true;
            DataManager.Instance.TouchLetter(day);
        }
    }
}
