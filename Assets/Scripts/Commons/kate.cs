using Assets.Scripts.Commons;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using DialogueEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public enum KateStatesEnum { Walking, Siting, Drinking , Idle, isTyping }
public class Kate : MonoBehaviour
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
    private bool favorresponse = false;
    [SerializeField] private NPCConversation myConversation;   
    private bool firstConversation= false;
    public bool IknowKateBirthday = false;
    public bool SaidHappyBirthday = false;
    public bool Cake =false;
    private bool inLoop = false;
    private int Morethan1;

    private Vector3 previousPosition;
    private float movingDifference;   
    Animator animator;
    Rigidbody rb;

    private Transform sitPoint;
    [Header("Sit Position")]
    public Transform sitPosition;


    [SerializeField] private KateStatesEnum currentState;
    private bool routineStarted = false;
    void Start()
    {       
        wayPointIndex = 0;        
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();       

        previousPosition = new Vector3(rb.transform.position.x, 0.0f, rb.transform.position.z);
        currentState = KateStatesEnum.Idle;       
        
    }
   
    // Update is called once per frame
    void Update()
    {      

        switch (currentState)
        {
            case KateStatesEnum.Walking:
                WaypointMovement();
                if (!animator.GetBool(nameof(isWalking)))
                {
                    CleanAnimationState();
                    animator.SetBool(nameof(isWalking), true);
                   
                }                    
                break;
            case KateStatesEnum.Siting:
                if (!animator.GetBool(nameof(isSit)))
                {
                    CleanAnimationState();
                    animator.SetBool(nameof(isSit), true);
                }
                
                break;
            case KateStatesEnum.Drinking:
                if (!animator.GetBool(nameof(isDrinkingCoffe)))
                {
                    CleanAnimationState();
                    animator.SetBool(nameof(isDrinkingCoffe), true);
                }                
                break;
            case KateStatesEnum.Idle:
                if (!animator.GetBool(nameof(isIdle)))
                {
                    CleanAnimationState();
                    animator.SetBool(nameof(isIdle), true);
                }                
                break;
            case KateStatesEnum.isTyping:
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
       // isSit = false;
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
            case Tags.Player:
                handlefirstencounter();
                break;
            case Tags.Waiting:
                currentState = KateStatesEnum.Idle;
                Invoke("IsWaiting", 10.0f);
                break;
            case Tags.Sit:
                handleDesk(otherObject);
                break;
            default:
                break;
        }


    }

    private void handlefirstencounter()
    {
        if (GameManager.GetGameManager().GetRestartCount() > 0)
            inLoop = true;

        if (GameManager.GetGameManager().GetRestartCount() > 3)
            Morethan1 = GameManager.GetGameManager().GetRestartCount();

        if (!firstConversation)
        {
            currentState = KateStatesEnum.Idle;
            GameManager.GetGameManager().SetEnablePlayerInput(false);
            Cursor.lockState = CursorLockMode.None;
            ConversationManager.Instance.StartConversation(myConversation);
            ConversationManager.Instance.SetBool("FirstInteraction", true);
            ConversationManager.Instance.SetBool("inLoop", inLoop);
            ConversationManager.Instance.SetInt("Morethan1", Morethan1);
            firstConversation =true;
        }
        else
        {
            currentState = KateStatesEnum.Idle;
            GameManager.GetGameManager().SetEnablePlayerInput(false);
            Cursor.lockState = CursorLockMode.None;
            ConversationManager.Instance.StartConversation(myConversation);
            ConversationManager.Instance.SetBool("FirstInteraction", false);
            ConversationManager.Instance.SetBool("BadReaction", !favorresponse);
            ConversationManager.Instance.SetBool("Birthday", IknowKateBirthday);
            ConversationManager.Instance.SetBool("birthdaysaluteDone", SaidHappyBirthday);
            if(Reports.HasPrintMartinezReports || Reports.HasPrintSanchezReport)
                ConversationManager.Instance.SetBool("Havereports", true);
            ConversationManager.Instance.SetBool("SanchezReport", Reports.HasPrintSanchezReport);
            ConversationManager.Instance.SetBool("MartinezReport", Reports.HasPrintMartinezReports);
            ConversationManager.Instance.SetBool("deliveryreport", Reports.DeliverySanchezReport);
            ConversationManager.Instance.SetBool("inLoop", inLoop);
            ConversationManager.Instance.SetInt("Morethan1", Morethan1);


        }

        GameManager.GetGameManager().ChagePlayerRotation(gameObject.transform);
    }
    private void handleDesk(GameObject otherObject)
    {
        Debug.Log("should sit here...");
        sitPoint = otherObject.transform.Find("sitposition");
        Sit();
        
    }
    private void Sit()
    {
        if (!isSit)
        {

            transform.position = sitPoint.position;
            transform.rotation = sitPoint.rotation;
            currentState = KateStatesEnum.isTyping;
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            isSit = true;
        }
        else
        {
            Debug.LogWarning("No se asignó un Animator al NPC.");
        }
    }
    public void HideUI()
    {
        UIManager.Instance.HidePanel(UIPanelTypeEnum.Indications);
    }
    public void IsWaiting()
    {   
        if(!isSit)
            currentState = KateStatesEnum.Walking;
    }

    public void FavorResponse(bool response)
    {
        favorresponse = response;
    }
 
    public void reademailbirthday(bool read)
    {
        IknowKateBirthday = read;
    }

    public void SaidHP(bool said)
    {
        SaidHappyBirthday=said;
    }

    public void CanEatCake(bool can)
    {
        Cake = can;
    }

    public void deliverSanchesreport(bool deliver)
    {
        Reports.DeliverySanchezReport = deliver;
    }
   
}
