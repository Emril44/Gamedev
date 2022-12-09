using System.Collections;
using UnityEngine;

public class GateLever : Lever
{
    [SerializeField] private GameObject gates;
    private Vector3 gatesPosition1;
    private Vector3 gatesPosition2;
    [SerializeField] private Vector2 gatesShift;
    [SerializeField] private float moveDuration;

    private void Awake()
    {
        gatesPosition1 = gates.transform.position;
        gatesPosition2 = gates.transform.position + new Vector3(gatesShift.x, gatesShift.y, 0);
    }

    public override Vector3 LookPosition()
    {
        return gatesPosition1;
    }

    protected override IEnumerator DoAction()
    {
        float time = 0;
        isLocked = true;
        while (time < moveDuration)
        {
            time += Time.fixedDeltaTime;
            if (isOn)
            {
                gates.transform.position = Vector3.Lerp(gatesPosition1, gatesPosition2, time/moveDuration);
            }
            else 
            { 
                gates.transform.position = Vector3.Lerp(gatesPosition2, gatesPosition1, time/moveDuration);
            }
            yield return new WaitForFixedUpdate();
        }
        if (isOn)
        {
            gates.transform.position = gatesPosition2;
        }
        else
        {
            gates.transform.position = gatesPosition1;
        }
        isLocked = false;
    }
}
