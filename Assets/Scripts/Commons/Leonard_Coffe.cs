using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum LeonardStatesEnum { Walking, Siting, Drinking , Idle, isTyping }
public class Leonard_Coffe : MonoBehaviour
{
    
    [SerializeField] private List<Transform> wayPoints = new List<Transform>();
    [SerializeField] private bool isWalking;
    [SerializeField] private bool isSit =false;
    [SerializeField] private bool isDrinkingCoffe;
    [SerializeField] private bool isIdle;
    [SerializeField] private bool isTyping;
    [SerializeField] private int wayPointIndex;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;    
    [SerializeField] private bool isLoop = true;   
    [SerializeField] private bool GotoBathroom =false;
    [SerializeField] private float waitTime = 60f;
    [SerializeField] private AudioClip callsound;
    private AudioSource audioSource;

    private Transform sitPoint;

    [Header("Sit Position")]
    public Transform sitPosition;

    private Vector3 previousPosition;
    private float movingDifference;   
    Animator animator;
    Rigidbody rb;
    [SerializeField] private LeonardStatesEnum currentState;
    private bool routineStarted = false;
    void Start()
    {       
        wayPointIndex = 0;        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = callsound;
        previousPosition = new Vector3(rb.transform.position.x, 0.0f, rb.transform.position.z);
        currentState = LeonardStatesEnum.Idle;
        StartCoroutine(StartRoutineAfterDelay());
    }
    private IEnumerator StartRoutineAfterDelay()
    {
        // Espera el tiempo configurado
        yield return new WaitForSeconds(waitTime);

        // Una vez que termina la espera, comienza la rutina
        routineStarted = true;
        currentState = LeonardStatesEnum.Walking;
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case LeonardStatesEnum.Walking:
                WaypointMovement();
                if (!animator.GetBool(nameof(isWalking)))
                {
                    CleanAnimationState();
                    animator.SetBool(nameof(isWalking), true);
                   
                }                    
                break;
            case LeonardStatesEnum.Siting:
                if (!animator.GetBool(nameof(isSit)))
                {
                    CleanAnimationState();
                    animator.SetBool(nameof(isSit), true);
                }
                
                break;
            case LeonardStatesEnum.Drinking:
                if (!animator.GetBool(nameof(isDrinkingCoffe)))
                {
                    CleanAnimationState();
                    animator.SetBool(nameof(isDrinkingCoffe), true);
                }                
                break;
            case LeonardStatesEnum.Idle:
                if (!animator.GetBool(nameof(isIdle)))
                {
                    CleanAnimationState();
                    animator.SetBool(nameof(isIdle), true);
                }                
                break;
            case LeonardStatesEnum.isTyping:
                if (!animator.GetBool(nameof(isTyping)))
                {
                    CleanAnimationState();
                    animator.SetBool(nameof(isTyping), true);
                    
                }
                break;
            default:
                CleanAnimationState();
                animator.SetBool(nameof(isIdle), true);
                break;
        }
        
    }

    private void CleanAnimationState()
    {
        animator.SetBool(nameof(isWalking), false);
        animator.SetBool(nameof(isSit), false);
        animator.SetBool(nameof(isDrinkingCoffe), false);
        animator.SetBool(nameof(isIdle), false);
        animator.SetBool(nameof(isTyping), false);
        isWalking = false;
        isSit = false;
        isDrinkingCoffe = false;
        isIdle = false;
        isTyping = false;
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
            case "Cafetera":
                handleCafetera(otherObject);
                break;
            case "Sit":
               handleDesk(otherObject);
                break;
            case "Llamado":
                handleCall(otherObject);
                break;
            default:
                break;
        }
        

    }
    private void handleCafetera(GameObject otherObject)
    {
        if (otherObject.GetComponent<Cafetera>().HasCoffee)
        {
            currentState = LeonardStatesEnum.Drinking;
            movingDifference = 0.0f;
            GotoBathroom = true;
            isDrinkingCoffe = true;
            wayPointIndex++;
            Invoke("IsWaiting", 7.0f);
            Debug.Log("should wait here...");
        }
    }

    private void handleDesk(GameObject otherObject)
    {
        Debug.Log("should sit here...");
        sitPoint = otherObject.transform.Find("sitposition");
        Sit();
        if (GotoBathroom)
            Invoke("GotoBath", 5.0f);
    }
    private void handleCall(GameObject otherObject)
    {
        Debug.Log("should talk to the phone here...");
        currentState = LeonardStatesEnum.Idle;
        audioSource.Play();
        GotoBathroom = false;
        Invoke("IsWaiting", 7.0f);
        Debug.Log("should wait here...");
    }
    public void IsWaiting()
    {      
        currentState = LeonardStatesEnum.Walking;
    }
    public void GotoBath()
    {      
        currentState = LeonardStatesEnum.Walking;       
    }
    private void Sit()
    { 
        if (!isSit)
        {         
          
            transform.position = sitPoint.position;
            transform.rotation = sitPoint.rotation;           
            currentState = LeonardStatesEnum.isTyping;
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
