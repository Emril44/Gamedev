using System.Collections;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    private Camera mainCam;
    private GameObject player;
    [SerializeField] private float scalingSpeed;
    public bool onPlayer { get; private set; } = true;
    public static CamMovement Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        mainCam = GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        if (onPlayer)
        {
            CamScale();
            CamUpdate();
        }
    }
    
    void CamUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
    
    //TODO: fix speed in build
    void CamScale()
    {
        if (mainCam.orthographicSize > 4)
        {
            mainCam.orthographicSize -= (0.021875f * scalingSpeed);
        }
        else
        {
            mainCam.orthographicSize = 4;
        }
    }

    public void LookOnGates(Vector3 gatesPosition)
    {
        onPlayer = false;
        StopAllCoroutines();
        StartCoroutine(LookOnGatesCoroutine(gatesPosition));
    }

    IEnumerator LookOnGatesCoroutine(Vector3 gatesPosition)
    {
        float time = 0;
        while (Mathf.Abs(transform.position.x - gatesPosition.x) > 0.01f)
        {
            time += Time.deltaTime/72;
            transform.position = Vector3.Lerp(transform.position, gatesPosition, time);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            yield return null;
        }
        transform.position = gatesPosition;
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        yield return new WaitForSeconds(1.4f);
        time = 0;
        while (Mathf.Abs(transform.position.x - player.transform.position.x) > 0.01f)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, player.transform.position, time/24);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            yield return null;
        }
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        onPlayer = true;
    }
}
