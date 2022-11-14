using System.Collections;
using UnityEngine;
using Cinemachine;

public class CamMovement : MonoBehaviour
{
    [SerializeField] private float scalingSpeed;
    
    public void LookOnGates(CinemachineVirtualCamera vcam)
    {
        StopAllCoroutines();
        StartCoroutine(LookOnGatesCoroutine(vcam));
    }

    IEnumerator LookOnGatesCoroutine(CinemachineVirtualCamera vcam)
    {
        vcam.Priority = 15;
        yield return new WaitForSeconds(1.6f);
        yield return new WaitForSeconds(2f);
        vcam.Priority = 5;
    }
}
