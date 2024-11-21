using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum jefeStatesEnum { Walking, Siting, Drinking , Idle, isTyping , Talking}
public class Jefe : MonoBehaviour
{
    
    [SerializeField] private List<Transform> wayPoints = new List<Transform>();
    [SerializeField] private bool isWalking;
    public bool isSit =true;
    [SerializeField] private bool isTalking;
    [SerializeField] private bool isIdle;
    [SerializeField] private bool isTyping;
    [SerializeField] private int wayPointIndex;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;    
    [SerializeField] private bool isLoop = true;   
    [SerializeField] private bool GotoBathroom =false;
    [SerializeField] private float waitTime = 60f;
    public bool BossisGone = false;
    public AudioSource audioSource;

    private Transform sitPoint;

    [Header("Sit Position")]
    public Transform sitPosition;

    private Vector3 previousPosition;
    private float movingDifference;   
    Animator animator;
    Rigidbody rb;
    [SerializeField] private jefeStatesEnum currentState;
    private bool routineStarted = false;
    ComputerBoss _ComputerBoss;
    void Start()
    {
        _ComputerBoss = FindObjectOfType<ComputerBoss>();
        
        wayPointIndex = 0;        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
       
        previousPosition = new Vector3(rb.transform.position.x, 0.0f, rb.transform.position.z);
        currentState = jefeStatesEnum.Siting;
        StartCoroutine(StartRoutineAfterDelay());
    }
    private IEnumerator StartRoutineAfterDelay()
    {
        // Espera el tiempo configurado
        yield return new WaitForSeconds(waitTime);

        // Una vez que termina la espera, comienza la rutina
        routineStarted = true;
        currentState = jefeStatesEnum.Siting;
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case jefeStatesEnum.Walking:
                BossisGone = true;
                WaypointMovement();
                if (!animator.GetBool(nameof(isWalking)))
                {
                    CleanAnimationState();
                    animator.SetBool(nameof(isWalking), true);
                   
                }                    
                break;
            case jefeStatesEnum.Siting:
                BossisGone=false;
                if (!animator.GetBool(nameof(isSit)))
                {
                    CleanAnimationState();
                    isSit = true;
                    animator.SetBool(nameof(isSit), true);
                }
                
                break;
            case jefeStatesEnum.Talking:
                BossisGone = true;
                if (!animator.GetBool(nameof(isTalking)))
                {
                    CleanAnimationState();
                    animator.SetBool(nameof(isTalking), true);
                }
                break;
            //case LeonardStatesEnum.Idle:
            //    if (!animator.GetBool(nameof(isIdle)))
            //    {
            //        CleanAnimationState();
            //        animator.SetBool(nameof(isIdle), true);
            //    }                
            //    break;
            case jefeStatesEnum.isTyping:
                BossisGone = false;
                if (!animator.GetBool(nameof(isSit)))
                {
                    CleanAnimationState();
                    isSit = true;
                    animator.SetBool(nameof(isSit), true);

                }
                break;
            default:
                CleanAnimationState();
                //animator.SetBool(nameof(isIdle), true);
                break;
        }
        
    }

    private void CleanAnimationState()
    {
        animator.SetBool(nameof(isWalking), false);
        animator.SetBool(nameof(isSit), false);
        animator.SetBool(nameof(isTalking), false);
        //animator.SetBool(nameof(isIdle), false);
        //animator.SetBool(nameof(isTyping), false);
        isWalking = false;
        isSit = false;
        isTalking = false;
        //isIdle = false;
        //isTyping = false;
    }

    private void WaypointMovement()
    {
        previousPosition = new Vector3(rb.transform.position.x, 0.0f, rb.transform.position.z);
        // ingresa siempre que tengamos mas de 1 waypoint al comienzo
        if (wayPointIndex < wayPoints.Count)
        {   // mueve el personaje hacia el waypoint
            rb.transform.position = Vector3.MoveTowards(rb.transform.position, wayPoints[wayPointIndex].position, Time.deltaTime * moveSpeed);

            // obtiene vector direccion (hacia el waypoint)
            Vector3 direction = wayPoints[wayPointIndex].position - rb.transform.position;

            // calcula el el angulo de rotacion y aplica rotacion con LERP
            Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
            rb.transform.rotation = Quaternion.Lerp(rb.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // calcula la distancia hacia siguiente waypoint
            float distance = Vector3.Distance(rb.transform.position, wayPoints[wayPointIndex].position);

            // llego al siguiente waypoint?
            if (distance <= 0.05f)
            {
                // random activo?
                
                    // incrementa el contador de waypoint
                    wayPointIndex++;

                    // modo bucle y el contador llego al final del waypoint list
                    if (isLoop && wayPointIndex >= wayPoints.Count)
                    {
                        // comenzar desde el primer elemento del array
                        wayPointIndex = 0;
                    }
               

            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject otherObject = other.gameObject;
        string tag = otherObject.tag;
        switch (tag)
        {
            case Tags.Waiting:
                currentState = jefeStatesEnum.Talking;
                Invoke("IsWaiting", 10.0f);
                break;
            case Tags.Sit:
               handleDesk(otherObject);
                break;
            case Tags.Llamado:
                handleCall(otherObject);
                break;
            case Tags.Player:
                handlePlayer();
                break;
            default:
                break;
        }
        

    }
    private void handlePlayer()
    {
        if (_ComputerBoss.lookingcomputer)
        {
            //Debug.Log("ME ENGANCHO");
            audioSource.Play();
            UIManager.Instance.HidePanel(UIPanelTypeEnum.ComputerBoss);
            UIManager.Instance.HidePanel(UIPanelTypeEnum.Interactive);
            GameManager.GetGameManager().RestartScene(5);
        }
    }

    private void handleDesk(GameObject otherObject)
    {
        
        sitPoint = otherObject.transform.Find("sitposition");
        Sit();
        //if (GotoBathroom)
        //    Invoke("GotoBath", 5.0f);
    }
    private void handleCall(GameObject otherObject)
    {
        //Debug.Log("should talk to the phone here...");
        //currentState = LeonardStatesEnum.Idle;
        //audioSource.Play();
        //GotoBathroom = false;
        //Invoke("IsWaiting", 7.0f);
        //Debug.Log("should wait here...");
    }
    public void IsWaiting()
    {      
        currentState = jefeStatesEnum.Walking;
    }
   
    private void Sit()
    {
        if (!isSit)
        {

            transform.position = sitPoint.position;            
            transform.rotation = sitPoint.rotation;
            currentState = jefeStatesEnum.isTyping;
            if (rb != null)
            {
                rb.isKinematic = true;
            }
        }
        else
        {
            Debug.LogWarning("No se asignó un Animator al NPC.");
        }
    }
}
