using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

// Implementation of capabilities of Black Circles, gatekeeper NPCs in Spark Mines, who open the gates only when the player has the needed amount of sparks and talks to them.
// Should only have two dialogue batches, for the guarding and letting-through states respectively
// Guarding batch should have 1 lore dialogue and a fallback dialogue reminding to gather more sparks
// Letting-through batch is expected to have 1 one-time dialogue, after playing which both the circle and the gate disappear
public class BlackCircle : NPC
{
    [SerializeField] private int sparksNeeded;
    [SerializeField] private GameObject gate;
    [SerializeField] private float fadeoutTime;
    private SpriteRenderer selfRenderer;
    private SpriteShapeRenderer gateRenderer;
    private Dialogue finalDialogue;

    override protected void Awake()
    {
        base.Awake();
        UpdateRequirements();
        DataManager.Instance.onSparksUpdate += UpdateRequirements;
        selfRenderer = gameObject.GetComponent<SpriteRenderer>();
        gateRenderer = gate.GetComponent<SpriteShapeRenderer>();
        finalDialogue = trigger.GetBatchAtIndex(1).dialogueList[0];
    }

    private void OnEnable()
    {
        finalDialogue.onDialogueEnd += Disappear;
    }

    private void OnDisable()
    {
        finalDialogue.onDialogueEnd -= Disappear;
    }

    private void UpdateRequirements()
    {
        if (DataManager.Instance.sparksAmount >= sparksNeeded)
        {
            trigger.SetBatchIndex(1);
        }
    }

    private void Disappear()
    {
        StartCoroutine(Fadeout());
    }

    IEnumerator Fadeout()
    {
        float time = 0;
        Color ogColor = selfRenderer.color;
        Color ogColorGate= gateRenderer.color;
        Color transparentColor = new Color(ogColor.r, ogColor.g, ogColor.b, 0);
        Color transparentColorGate = new Color(ogColorGate.r, ogColorGate.g, ogColorGate.b, 0);
        Vector3 ogScale = transform.localScale;
        Vector3 ogScaleGate = gate.transform.localScale;
        Vector3 scale0 = new Vector3(0, transform.localScale.y, transform.localScale.z);
        Vector3 scale0Gate = new Vector3(0, gate.transform.localScale.y, gate.transform.localScale.z);
        while (time < fadeoutTime)
        {
            time += Time.fixedDeltaTime;
            float factor = time / fadeoutTime;
            selfRenderer.color = Color.Lerp(ogColor, transparentColor, factor);
            transform.localScale = Vector3.Lerp(ogScale, scale0, factor);
            gateRenderer.color = Color.Lerp(ogColorGate, transparentColorGate, factor);
            gate.transform.localScale = Vector3.Lerp(ogScaleGate, scale0Gate, factor);
            yield return new WaitForFixedUpdate();
        }
        gameObject.SetActive(false);
        gate.SetActive(false);
    }
}
