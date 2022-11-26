using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DialogueTrigger());
    }
    
    IEnumerator DialogueTrigger()
    {
        yield return new WaitForSeconds(1.2f);
        GetComponent<DialogueTrigger>().TriggerDialogue();
    }
}
