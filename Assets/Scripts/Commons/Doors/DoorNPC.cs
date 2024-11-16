using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNPC : MonoBehaviour
{
    public bool open = false;
    public float smooth = 1.0f;
    public float doorOpenAngle = -90.0f;
    public float doorCloseAngle = 0.0f;
    public string npcTag = "NPC"; // Tag del objeto que abrir� la puerta autom�ticamente
    public AudioSource audioSource;
    public AudioClip openDoorSound;
    public AudioClip closeDoorSound;

    private Transform doorTransform;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Obt�n el componente hijo 'Door'
        doorTransform = transform.Find("Door");
        if (doorTransform == null)
        {
            Debug.LogError("No se encontr� el objeto hijo 'Door'.");
        }
    }

    void Update()
    {
        // Movimiento suave de la puerta
        if (doorTransform != null)
        {
            float targetAngle = open ? doorOpenAngle : doorCloseAngle;
            Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
            doorTransform.localRotation = Quaternion.Slerp(doorTransform.localRotation, targetRotation, Time.deltaTime * smooth);
        }
    }

    void ToggleDoor(bool shouldOpen)
    {
        // Solo cambia el estado si es diferente del actual
        if (open != shouldOpen)
        {
            open = shouldOpen;

            // Selecciona el sonido adecuado
            audioSource.clip = open ? openDoorSound : closeDoorSound;
            audioSource.Play();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Abre la puerta al detectar el objeto con el Tag espec�fico en el rango
        if (other.CompareTag(npcTag))
        {
            ToggleDoor(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Cierra la puerta cuando el objeto con el Tag espec�fico sale del rango
        if (other.CompareTag(npcTag))
        {
            ToggleDoor(false);
        }
    }
}
