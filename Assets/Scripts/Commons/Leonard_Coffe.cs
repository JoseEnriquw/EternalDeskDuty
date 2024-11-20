using Assets.Scripts.Commons;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

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
    private bool Entry = false;

    [Header("Audios")]
    [SerializeField] private AudioClip callsound;
    [SerializeField] private AudioClip goodmorning;
    [SerializeField] private AudioClip thereiscoffee;
    [SerializeField] private AudioClip thereisNOcoffee;


    private AudioSource callSource, goodmorningsource, thereiscoffeesource, thereisNOcoffeesource;

    private Transform sitPoint;
    [Header("Sit Position")]
    public Transform sitPosition;

    private Vector3 previousPosition;
    private float movingDifference;   
    Animator animator;
    Rigidbody rb;
    CallZone callzone;
    public bool _ClienteEspecial = false;

    [SerializeField] private LeonardStatesEnum currentState;
    private bool routineStarted = false;
    void Start()
    {       
        wayPointIndex = 0;        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        callzone = FindObjectOfType<CallZone>();        

        callSource = GetComponents<AudioSource>()[0];   
        goodmorningsource = GetComponents<AudioSource>()[1];
        thereiscoffeesource = GetComponents<AudioSource>()[2];
        thereisNOcoffeesource = GetComponents<AudioSource>()[3];

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
        if (waitTime <= CustomTimer.Instance.GetTimer() && !Entry)
        {
            Entry = true;
            currentState = LeonardStatesEnum.Walking;
        }

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
            case Tags.Cafetera:
                handleCafetera(otherObject);
                break;
            case Tags.Sit:
               handleDesk(otherObject);
                break;
            case Tags.Llamado:
                handleCall(otherObject);
                break;
            case Tags.GoodMorning:
                handleGoodMorning(otherObject);
                break;
            default:
                break;
        }
        

    }

    private void handleGoodMorning(GameObject otherObject)
    {
        Debug.Log("should say good morning here...");
        goodmorningsource.PlayOneShot(goodmorning);
        UIManager.Instance.ShowPanelIndicationsAnAddIndications("Leonard: 'Good Morning!'");
        Invoke("HideUI", 2.0f);
    }

    private void handleCafetera(GameObject otherObject)
    {
        if (otherObject.GetComponent<Cafetera>().HasCoffee)
        {
            thereiscoffeesource.PlayOneShot(thereiscoffee);
            UIManager.Instance.ShowPanelIndicationsAnAddIndications("Leonard: 'Thank God! There is Coffee!'");
            Invoke("HideUI", 2.0f);
            currentState = LeonardStatesEnum.Drinking;
            movingDifference = 0.0f;
            GotoBathroom = true;
            isDrinkingCoffe = true;
            wayPointIndex++;
            Invoke("IsWaiting", 7.0f);
            Debug.Log("should wait here...");
        }
        else
        {
            thereisNOcoffeesource.PlayOneShot(thereisNOcoffee);
            UIManager.Instance.ShowPanelIndicationsAnAddIndications("Leonard: 'Always the same here! The first to get in has to do the coffee Brian! '");
            Invoke("HideUI", 2.0f);
        }
    }

    private void handleDesk(GameObject otherObject)
    {
        Debug.Log("should sit here...");
        sitPoint = otherObject.transform.Find("sitposition");
        Sit();
        if (GotoBathroom)
            Invoke("GotoBath", 20.0f);
    }
    private void handleCall(GameObject otherObject)
    {
        Debug.Log("should talk to the phone here...");
        currentState = LeonardStatesEnum.Idle;
        if (callzone.isPlayerInZone)
        {
            callSource.PlayOneShot(callsound);
            UIManager.Instance.ShowPanelIndicationsAnAddIndications("Leonard: 'Hi!, yes... yess... I promise I will delivery the rest by tomorrow! I'm so sorry Mr Jiménez. Please dont tell my boss!'");
            Invoke("HideUI", 10.0f);
            _ClienteEspecial = true;
        }
        
        GotoBathroom = false;
        Invoke("IsWaiting", 10.0f);
        Debug.Log("should wait here...");
    }
    public void HideUI()
    {
        UIManager.Instance.HidePanel(UIPanelTypeEnum.Indications);
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
