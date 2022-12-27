using UnityEngine;

public class AmbientPlayer : MonoBehaviour
{
    [SerializeField] private string soundName;
    [SerializeField] private float minDistance = 1;
    [SerializeField] private float maxDistance = 10;
    private AudioController.Sound sound;
    private AudioSource source;
    private GameObject player;

    void Start()
    {
        sound = AudioController.Instance.GetSound(soundName);
        if (sound == null)
        {
            Debug.LogError("Illegal sound " + soundName + " on ambient player on game object " + gameObject.name);
        }
        else
        {
            source = gameObject.AddComponent<AudioSource>();
            source.loop = true;
            source.clip = sound.clip;
            source.spatialBlend = 1f;
            source.volume = sound.volume;
            source.minDistance = minDistance;
            source.maxDistance = maxDistance;
            source.rolloffMode = AudioRolloffMode.Linear;
            player = GameObject.FindWithTag("Player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!source.isPlaying && Vector3.Distance(transform.position, player.transform.position) < sound.maxDistance) source.Play();
        else if (source.isPlaying && Vector3.Distance(transform.position, player.transform.position) > sound.maxDistance) source.Pause();
    }
}
