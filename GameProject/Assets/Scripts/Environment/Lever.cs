using System.Collections;
using UnityEngine;

public abstract class Lever : MonoBehaviour
{
    [SerializeField] private GameObject lever;
    [SerializeField] private float resetDelay;
    [SerializeField] private bool supportsLookOn;
    protected bool isOn = false;
    protected bool isLocked = false;

    public bool SupportsLookOn => supportsLookOn;

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

        while (isOn ? Quaternion.Angle(lever.transform.localRotation, leverRotation[0]) > 0.1f : Quaternion.Angle(lever.transform.localRotation, leverRotation[1]) > 0.1f)
        {
            time += Time.deltaTime;
            if (isOn)
            {
                lever.transform.localRotation = Quaternion.Lerp(lever.transform.localRotation, leverRotation[0], time/50);
            }
            else
            {
                lever.transform.localRotation = Quaternion.Lerp(lever.transform.localRotation, leverRotation[1], time/50);
            }
            yield return null;
        }
        if (isOn)
        {
            lever.transform.localRotation = leverRotation[0];
        }
        else
        {
            lever.transform.localRotation = leverRotation[1];
        }
    }
}
