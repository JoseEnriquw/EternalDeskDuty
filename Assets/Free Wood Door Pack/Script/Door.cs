using Assets.Scripts.Commons.Constants;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool open = false;
    public float smooth = 1.0f;
    public float doorOpenAngle = -90.0f;
    public float doorCloseAngle = 0.0f;
    public bool controlledByPlayer = true; 
    public bool controlledByNPC = false;   

    public AudioSource audioSource;
    public AudioClip openDoorSound;
    public AudioClip closeDoorSound;

    private Transform doorTransform;
    private bool playerInRange = false; // Solo para control de jugador

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        doorTransform = transform.Find("Door");
        if (doorTransform == null)
        {
            Debug.LogError("No se encontró el objeto hijo 'Door'.");
        }
    }

    void Update()
    {
        if (controlledByPlayer && playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }

        if (doorTransform != null)
        {
            float targetAngle = open ? doorOpenAngle : doorCloseAngle;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            doorTransform.localRotation = Quaternion.Slerp(doorTransform.localRotation, targetRotation, Time.deltaTime * smooth);
        }
    }

    void ToggleDoor(bool? shouldOpen = null)
    {
        if (shouldOpen.HasValue)
        {
            if (open != shouldOpen.Value)
            {
                open = shouldOpen.Value;
                PlayAudio();
            }
        }
        else
        {
            open = !open;
            PlayAudio();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (controlledByPlayer && other.CompareTag(Tags.Player))
        {
            playerInRange = true;
        }

        if (controlledByNPC && other.CompareTag(Tags.Npc))
        {
            ToggleDoor(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (controlledByPlayer && other.CompareTag(Tags.Player))
        {
            playerInRange = false;
        }

        if (controlledByNPC && other.CompareTag(Tags.Npc))
        {
            ToggleDoor(false);
        }
    }

    private void PlayAudio()
    {
        audioSource.clip = open ? openDoorSound : closeDoorSound;
        audioSource.Play();
    }
}
