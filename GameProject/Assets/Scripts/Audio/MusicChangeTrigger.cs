using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{

    public AudioClip newBGM;
    private AudioController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<AudioController>();
    }

    // Update is called once per frame
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(newBGM != null)
                controller.ChangeBGM(newBGM);
        }
    }
}
