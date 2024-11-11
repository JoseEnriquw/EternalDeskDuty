using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDoorController : MonoBehaviour
{
    public bool open = false;
    public float smooth = 1.0f;
    public float doorOpenAngle = -90.0f;
    public float doorCloseAngle = 0.0f;
    public AudioSource audioSource;
    public AudioClip openDoorSound;
    public AudioClip closeDoorSound;

    private bool playerInRange = false; // Para verificar si el jugador está cerca
    private Transform doorTransform; // Referencia al objeto hijo 'Door'

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Obtén el componente hijo 'Door' (suponiendo que es un hijo directo)
        doorTransform = transform.Find("Door");
        if (doorTransform == null)
        {
            Debug.LogError("No se encontró el objeto hijo 'Door'.");
        }
    }

    void Update()
    {
        // Verifica si el jugador toca en la puerta y presiona "E"
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleDoor();
        }

        // Movimiento suave de la puerta solo si 'Door' existe
        if (doorTransform != null)
        {
            float targetAngle = open ? doorOpenAngle : doorCloseAngle;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            doorTransform.localRotation = Quaternion.Slerp(doorTransform.localRotation, targetRotation, Time.deltaTime * smooth);
        }
    }

    void ToggleDoor()
    {
        // Cambia el estado de la puerta
        open = !open;

        // Selecciona el sonido adecuado
        audioSource.clip = open ? openDoorSound : closeDoorSound;
        audioSource.Play();
    }

    void OnTriggerEnter(Collider other)
    {
        // Activa el rango si el jugador está cerca
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Desactiva el rango cuando el jugador se aleja
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
