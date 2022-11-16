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
            StartCoroutine(Undamageable());
        }
        if(health <= 0)
        {
            Die();
        }
    }

    IEnumerator Undamageable()
    {
        yield return new WaitForSeconds(undamageableTime);
        damageable = true;
    }

    private void Die()
    {
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
                    Camera.main.gameObject.GetComponent<CamMovement>().LookOnGates(leverGO.GetComponent<Lever>().LookPosition());
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
                Die();
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
