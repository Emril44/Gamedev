using UnityEngine;

public class CamMovement : MonoBehaviour
{
    private Camera mainCam;
    private GameObject player;
    [SerializeField] private float scalingSpeed;

    void Start()
    {
        mainCam = GetComponent<Camera>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void LateUpdate()
    {
        CamScale();
        CamUpdate();
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
}
