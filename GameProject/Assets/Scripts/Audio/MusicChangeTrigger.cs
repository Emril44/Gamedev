using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{

    public AudioClip newBGM;

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(newBGM != null)
                AudioController.Instance.ChangeBGM(newBGM);
        }
    }
}
