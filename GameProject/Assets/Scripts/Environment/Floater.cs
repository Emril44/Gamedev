using System.Collections;
using UnityEngine;

public class Floater : MonoBehaviour
{
    private static float GLOBAL_FLOAT_UP = 0.13f;
    private static float GLOBAL_FLOAT_TIME = 0.75f;
    private Vector3 originalPos;
    private Vector3 otherPos;
    private void Awake()
    {
        originalPos = transform.position;
        otherPos = originalPos + Vector3.up * GLOBAL_FLOAT_UP;
    }

    private void OnEnable()
    {
        StartCoroutine(Animate());
    }

    private IEnumerator Animate()
    {
        while (true)
        {
            yield return FloatUp();
            yield return FloatDown();
        }
    }

    private IEnumerator FloatUp()
    {
        float time = 0;
        while (time < GLOBAL_FLOAT_TIME)
        {
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(originalPos, otherPos, time / GLOBAL_FLOAT_TIME);
        }
        transform.position = otherPos;
    }

    private IEnumerator FloatDown()
    {
        float time = 0;
        while (time < GLOBAL_FLOAT_TIME)
        {
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
            transform.position = Vector3.Lerp(otherPos, originalPos, time / GLOBAL_FLOAT_TIME);
        }
        transform.position = originalPos;
    }
}
