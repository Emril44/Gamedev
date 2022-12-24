using System.Collections.Generic;
using UnityEngine;

// Script that observes whether an object is overlapping with other physical objects from some layers, used for handling stuck-in-the-ground situations
public class OverlapHandler : MonoBehaviour
{
    new private Collider2D collider;
    [SerializeField] private LayerMask overlapMask;
    [SerializeField] private int countToIgnore = 0;
    public bool Overlapping { get; private set; }
    public List<Collider2D> OverlapList { get; private set; }

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        Overlapping = IsOverlapping();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer & overlapMask.value) != 0)
        {
            Overlapping = IsOverlapping(); // only update overlap state when it might change
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if ((1 << other.gameObject.layer & overlapMask.value) != 0)
        {
            Overlapping = IsOverlapping(); // only update overlap state when it might change
        }
    }

    private bool IsOverlapping()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D
        {
            useTriggers = false,
            useLayerMask = true,
            layerMask = overlapMask,
        };
        collider.OverlapCollider(filter, colliders);
        OverlapList = colliders;
        return colliders.Count > countToIgnore;
    }
}
