using System.Collections;
using UnityEngine;

public class CamMovement : MonoBehaviour
{
    private Camera mainCam;
    private GameObject player;
    [SerializeField] private float scalingSpeed;

    private bool onPlayer = true;

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
        while (time < 1)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, gatesPosition, time/45);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            yield return null;
        }
        transform.position = gatesPosition;
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);
        yield return new WaitForSeconds(1.4f);
        time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, player.transform.position, time/20);
            transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            yield return null;
        }
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        onPlayer = true;
    }
}
