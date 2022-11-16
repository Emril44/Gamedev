using System.Collections;
using UnityEngine;

public abstract class Lever : MonoBehaviour
{
    [SerializeField] private GameObject lever;
    [SerializeField] private float resetDelay;
    protected bool isOn = false;
    protected bool isLocked = false;

    private Quaternion[] leverRotation =
    {
        Quaternion.Euler(0, 0, 40),
        Quaternion.Euler(0,0,-40)
    };

    public void Toggle()
    {
        if (isLocked) return;
        StopAllCoroutines();
        isOn = !isOn;
        StartCoroutine(DoAction());
        StartCoroutine(MoveLever());
        if (isOn) StartCoroutine(ResetState());
    }

    IEnumerator ResetState()
    {
        yield return new WaitForSeconds(resetDelay);
        Toggle();
    }

    protected abstract IEnumerator DoAction();

    public abstract Vector3 LookPosition();


    IEnumerator MoveLever()
    {
        float time = 0;
        while(time < 1)
        {
            time += Time.deltaTime;
            if (isOn)
            {
                lever.transform.rotation = Quaternion.Lerp(lever.transform.rotation, leverRotation[0], time/50);
            }
            else
            {
                lever.transform.rotation = Quaternion.Lerp(lever.transform.rotation, leverRotation[1], time/50);
            }
            yield return null;
        }
        if (isOn)
        {
            lever.transform.rotation = leverRotation[0];
        }
        else
        {
            lever.transform.rotation = leverRotation[1];
        }
    }
}
