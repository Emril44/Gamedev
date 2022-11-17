using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteraction : MonoBehaviour
{
    public bool Controllable;
    [SerializeField] private Collider2D bodyCollider;
    private bool isGrabbing = false;
    private bool nearLever = false;
    private bool nearDialogue = false;
    private GameObject leverGO;
    private GameObject dialogueGO;
    private Transform oldParent;
    private GameObject grabbedObject;
    [SerializeField] private int health = 3;
    [SerializeField] private float undamageableTime = 0.65f;
    private bool damageable = true;

    private Rigidbody2D rb;
    private Animator animator;
    private const string DAMAGE_NAME = "Player_Damage";
    private const string DEATH_NAME = "Player_Death";

    public static PlayerInteraction Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        Controllable = true;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Damage"))) 
        {
            GetDamaged();
        }
    }

    void PutPrism(PrismShard prismShard)
    {
        EnvironmentManager.Instance.SetNewColor(prismShard.getColor());
    }

    void AddSpark(GameObject spark)
    {
        DataManager.Instance.AddSpark();
        Destroy(spark);
    }
    
    private void GetDamaged()
    {
        if (damageable)
        {
            damageable = false;
            health--;
            animator.Play(DAMAGE_NAME);
            StartCoroutine(Undamageable());
        }
        if(health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    IEnumerator Undamageable()
    {
        yield return new WaitForSeconds(undamageableTime);
        damageable = true;
    }

    private IEnumerator Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        animator.Play(DEATH_NAME);
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
        DataManager.Instance.Die();
    }

    private void Update()
    {
        if (Controllable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (isGrabbing)
                {
                    Drop();
                }
                else
                {
                    Grab();
                }
            }
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (nearLever)
                {
                    leverGO.GetComponent<Lever>().Toggle();
                }
                else if (nearDialogue)
                {
                    DialogueTrigger trigger = dialogueGO.GetComponent<DialogueTrigger>();
                    Controllable = false;
                    GetComponent<PlayerMovement>().Controllable = false;
                    void reenable()
                    {
                        Controllable = true;
                        GetComponent<PlayerMovement>().Controllable = true;
                        trigger.Dialogue.onDialogueEnd -= reenable;
                    };
                    trigger.Dialogue.onDialogueEnd += reenable;
                    trigger.TriggerDialogue(dialogueGO);
                }
            }
            if (Input.GetKeyDown(KeyCode.L))
            {
                if (nearLever)
                {
                    var lever = leverGO.GetComponent<Lever>();
                    if (lever.SupportsLookOn)
                    {
                        Camera.main.gameObject.GetComponent<CamMovement>().LookOnGates(lever.LookPosition());
                    }
                }
            }
        }
    }

    public void Drop()
    {
        if (isGrabbing)
        {
            isGrabbing = false;
            grabbedObject.transform.localPosition = new Vector3(transform.localScale.x + 0.3f, 0.2f);
            grabbedObject.transform.parent = oldParent;
            grabbedObject.GetComponent<Rigidbody2D>().simulated = true;
            grabbedObject.GetComponent<Collider2D>().enabled = true;
            grabbedObject = null;
        }
    }

    public GameObject GetGrabbed()
    {
        return grabbedObject;
    }

    private void Grab()
    {
        var colliders = new List<Collider2D>();
        var filter = new ContactFilter2D
        {
            useTriggers = true
        };
        bodyCollider.OverlapCollider(filter, colliders);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Grabbable"))
            {
                grabbedObject = collider.gameObject;
                oldParent = grabbedObject.transform.parent;
                grabbedObject.transform.SetParent(transform);
                grabbedObject.GetComponent<Collider2D>().enabled = false;
                grabbedObject.GetComponent<Rigidbody2D>().simulated = false;
                grabbedObject.transform.localPosition = Vector3.zero;
                collider.attachedRigidbody.velocity = Vector2.zero;
                isGrabbing = true;
                break;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Prism":
                PutPrism(other.gameObject.GetComponent<PrismShard>());
                break;
            case "Spark":
                AddSpark(other.gameObject);
                break;
            case "Water":
                StopAllCoroutines();
                damageable = true;
                StartCoroutine(SetInWater(true));
                break;
            case "Lever":
                nearLever = true;
                leverGO = other.gameObject;
                break;
            case "DialogueTrigger":
                nearDialogue = true;
                dialogueGO = other.gameObject;
                break;
            case "SparkDoor":
                other.gameObject.GetComponent<SparkDoor>().Open();
                break;
            case "DeadlyDamage":
                StartCoroutine(Die());
                break;
            default:
                //Debug.Log("No interaction with " + other.gameObject.tag);
                break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Water":
                StopAllCoroutines();
                StartCoroutine(SetInWater(false));
                break;
            case "Lever":
                nearLever = false;
                leverGO = null;
                break;
            case "DialogueTrigger":
                nearDialogue = false;
                dialogueGO = null;
                break;
        }
    }

    IEnumerator SetInWater(bool isInWater)
    {
        yield return new WaitForSeconds(0.1f);
        gameObject.GetComponent<PlayerMovement>().SetInWater(isInWater);
    }
}
