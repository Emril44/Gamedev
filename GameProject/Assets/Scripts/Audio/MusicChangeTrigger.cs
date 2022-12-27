using UnityEngine;

public class MusicChangeTrigger : MonoBehaviour
{

    [SerializeField] private AudioController.BGM newMusic;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            AudioController.Instance.ChangeBGM(newMusic);
        }
    }
}
