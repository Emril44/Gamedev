using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerInteraction : MonoBehaviour
{
    public Action onHealthUpdate;
    public Action onDeath;
    public bool Controllable = true;
    [SerializeField] private Collider2D bodyCollider;
    private bool isGrabbing = false;
    private bool nearLever = false;
    private bool nearDialogue = false;
    private bool nearMovable = false;
    private bool nearWaypoint = false;
    private GameObject leverGO;
    private GameObject dialogueGO;
    private GameObject movableGO;
    private Transform oldParent;
    private GameObject grabbedObject;
    [SerializeField] public int health { get; private set; } = 3;
    [SerializeField] private float undamageableTime = 0.65f;
    private float fireproofTime = 0f; // time left of being fireproof
    private bool damageable = true;
    [SerializeField] private Vector2 spawnLocation;
    private PlayerMovement movement;
    public bool CanSave { get; private set; }

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
        movement = GetComponent<PlayerMovement>();
        transform.localPosition = new Vector3(spawnLocation.x, spawnLocation.y, 0); // spawn at spawn location
        Debug.Log("spawn");
    }

    void PutPrism(PrismShard prismShard)
    {
        EnvironmentManager.Instance.SetNewColor(prismShard.getColor());
    }

    void AddSpark(GameObject spark)
    {
        DataManager.Instance.AddSpark();
        spark.SetActive(false);
    }
    
    private void GetDamaged(string source)
    {
        if (damageable)
        {
            damageable = false;
            health--;
            onHealthUpdate?.Invoke();
            animator.Play(DAMAGE_NAME);
            StartCoroutine(Undamageable());
        }
        if(health <= 0)
        {
            onDeath?.Invoke();
            StartCoroutine(Die(source));
        }
    }

    IEnumerator Undamageable()
    {
        yield return new WaitForSeconds(undamageableTime);
        damageable = true;
    }

    private IEnumerator Die(string source)
    {
        rb.bodyType = RigidbodyType2D.Static;
        animator.Play(DEATH_NAME);
        AnalyticsManager.Instance.DeathEvent(DataManager.Instance.day,source);
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Controllable = false;
        GetComponent<SpriteRenderer>().enabled = false; 
    }

    private void Update()
    {
        if (IsFireproof())
        {
            fireproofTime -= Time.deltaTime;
            if (fireproofTime < 0) fireproofTime = 0;
        }
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
                    Dialogue dialogue = trigger.GetCurrentDialogue();
                    Controllable = false;
                    movement.Controllable = false;
                    void reenable()
                    {
                        Controllable = true;
                        movement.Controllable = true;
                        dialogue.onDialogueEnd -= reenable;
                        if (trigger.tag != "DialogueTrigger") // for the rare case when further dialogue is impossible, i.e. finishing last main sequence and having no fallback dialogue
                        {
                            nearDialogue = false;
                            dialogueGO = null;
                        }
                    };
                    dialogue.onDialogueEnd += reenable;
                    trigger.TriggerDialogue();
                }
                else if (nearMovable)
                {
                    movableGO.GetComponent<MovableObject>().StartMove();
                    nearMovable = false;
                    movableGO = null;
                }
                else if (nearWaypoint)
                {
                    nearWaypoint = false;
                    movement.ResetParent();
                    transform.localPosition = new Vector3(spawnLocation.x, spawnLocation.y, 0); // teleport to spawn (i.e. to Monochrome)
                    EnvironmentManager.Instance.SetNewColor(PrismColor.Neutral); // reset color to avoid cheesing
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
            grabbedObject.GetComponent<MovableObject>().Grabbed = false;
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
                grabbedObject.GetComponent<MovableObject>().Grabbed = true;
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
            case "Lava":
                StopAllCoroutines();
                damageable = true;
                StartCoroutine(SetInLava(true));
                break;
            case "Lever":
                nearLever = true;
                leverGO = other.gameObject;
                break;
            case "DialogueTrigger":
                nearDialogue = true;
                dialogueGO = other.gameObject;
                break;
            case "Movable":
                nearMovable = true;
                movableGO = other.gameObject;
                break;
            case "Waypoint":
                nearWaypoint = true;
                break;
            case "SparkDoor":
                other.gameObject.GetComponent<SparkDoor>().Open();
                break;
            case "DeadlyDamage":
                health = 0;
                onHealthUpdate?.Invoke();
                StartCoroutine(Die(other.gameObject.name));
                break;
            case "SaveZone":
                CanSave = true;
                health = 3;
                onHealthUpdate?.Invoke();
                break;
            default:
                //Debug.Log("No interaction with " + other.gameObject.tag);
                break;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Lava":
                if (!IsFireproof()) GetDamaged(other.gameObject.name);
                break;
            case "Damage":
                GetDamaged(other.gameObject.name);
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
            case "Lava":
                StopAllCoroutines();
                StartCoroutine(SetInLava(false));
                break;
            case "Lever":
                nearLever = false;
                leverGO = null;
                break;
            case "DialogueTrigger":
                nearDialogue = false;
                dialogueGO = null;
                break;
            case "Movable":
                nearMovable = false;
                movableGO = null;
                break;
            case "Waypoint":
                nearWaypoint = false;
                break;
            case "SaveZone":
                CanSave = false;
                break;
        }
    }

    public void ApplyFireproof(float seconds)
    {
        fireproofTime = seconds;
    }

    public bool IsFireproof()
    {
        return fireproofTime > 0;
    }

    IEnumerator SetInWater(bool isInWater)
    {
        yield return new WaitForSeconds(0.1f);
        movement.SetInWater(isInWater);
    }

    IEnumerator SetInLava(bool isInLava)
    {
        yield return new WaitForSeconds(0.1f);
        movement.SetInWater(isInLava);
    }
}
