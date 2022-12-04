using System.Collections;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private Vector3 position1;
    private Vector3 position2;
    private bool busy = false;
    private bool atStart = true; // value is only valid at endpoints, i.e. when the moving object is not busy
    [SerializeField] private Vector2 shift;
    [SerializeField] private float moveDuration;
    [SerializeField] private float pauseAtPosition1; // time for which the object does not move after reaching position 1
    [SerializeField] private float pauseAtPosition2;

    private void Awake()
    {
        position1 = transform.position;
        position2 = transform.position + new Vector3(shift.x, shift.y, 0);
    }

    void FixedUpdate()
    {
        if (!busy)
        {
            if (atStart) StartCoroutine(MoveAhead());
            else StartCoroutine(MoveBack());
        }
    }
    IEnumerator MoveAhead()
    {
        busy = true;
        atStart = false;
        float time = 0;
        while (time < moveDuration)
        {
            time += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(position1, position2, time / moveDuration);
            yield return new WaitForFixedUpdate();
        }
        transform.position = position2;
        yield return new WaitForSeconds(pauseAtPosition2);
        busy = false;
    }
    IEnumerator MoveBack()
    {
        busy = true;
        float time = 0;
        while (time < moveDuration)
        {
            time += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(position2, position1, time / moveDuration);
            yield return new WaitForFixedUpdate();
        }
        transform.position = position1;
        atStart = true;
        yield return new WaitForSeconds(pauseAtPosition1);
        busy = false;
    }
}
