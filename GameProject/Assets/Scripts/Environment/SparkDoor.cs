using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class SparkDoor : MonoBehaviour
{
    [SerializeField] private int sparksToPass;
    public void Open()
    {
        if (SavesManager.Instance.SparksAmount >= sparksToPass)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(OpenDoor());
        }
        else
        {
            Debug.Log("Not enough sparks: " + SavesManager.Instance.sparksAmount + "/" + sparksToPass);
        }
    }

    IEnumerator OpenDoor()
    {
        float time = 0;
        Color alphaColor = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0);
        Vector3 scale0 = new Vector3(0, transform.localScale.y, transform.localScale.z);
        while (time < 4)
        {
            time += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = Color.Lerp(GetComponent<SpriteRenderer>().color, alphaColor, time / 200);
            transform.localScale = Vector3.Lerp(transform.localScale, scale0, time / 200);
            yield return null;
        }
        Destroy(this);
    }

}
