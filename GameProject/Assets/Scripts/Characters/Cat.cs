using System;
using UnityEngine;

// Script for the Cat NPC that implements the fireproof effect granting behaviour
public class Cat : NPC
{
    [Serializable]
    private struct FireproofGrant
    {
        [field: SerializeField] public Dialogue Dialogue { get; private set; }
        [field: SerializeField] public float Time { get; private set; }
    }

    [SerializeField] private FireproofGrant[] fireproofGrants;
    private Action[] grantDelegates;

    private void OnEnable()
    {
        grantDelegates = new Action[fireproofGrants.Length];
        for (int i = 0; i < fireproofGrants.Length; i++)
        {
            int iCurrent = i; // so that the lambda, when called, refers to the value of i at this step and not at the final step
            grantDelegates[i] = () =>
            {
                PlayerInteraction.Instance.ApplyFireproof(fireproofGrants[iCurrent].Time);
            };
            fireproofGrants[i].Dialogue.onDialogueEnd += grantDelegates[i];
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < fireproofGrants.Length; i++)
        {
            fireproofGrants[i].Dialogue.onDialogueEnd -= grantDelegates[i];
        }

    }
}
