using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private GameObject lever;
    [SerializeField] private GameObject gates;
    private Vector3 gatesPosition1;
    [SerializeField] private Vector3 gatesPosition2;
    private bool isOn = false;

    private Quaternion[] leverRotation =
    {
        Quaternion.Euler(0, 0, 40),
        Quaternion.Euler(0,0,-40)
    };

    private void Awake()
    {
        gatesPosition1 = gates.transform.position;
    }

    public void Toggle()
    {
        StopAllCoroutines();
        isOn = !isOn;
        StartCoroutine(MoveGates());
        StartCoroutine(MoveLever());
    }

    IEnumerator MoveGates()
    {
        float time = 0;
        while (time < 1)
        {
            time += Time.deltaTime;
            if (isOn)
            {
                gates.transform.position = Vector3.Lerp(gates.transform.position, gatesPosition2, time/50);
            }
            else 
            { 
                gates.transform.position = Vector3.Lerp(gates.transform.position, gatesPosition1, time/50);
            }
            yield return null;
        }
        if (isOn)
        {
            gates.transform.position = gatesPosition2;
        }
        else
        {
            gates.transform.position = gatesPosition1;
        }
    }

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
