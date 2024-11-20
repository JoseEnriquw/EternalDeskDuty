using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour
{
    public float delay = 5.0f;
    public AudioSource audioSource;

    private bool isPlayerInRange = false; // Verifica si el jugador está cerca
    private bool isRadioOn = false; // Estado de la radio (encendida/apagada)
    private bool hasAutoStarted = false; // Controla si la canción ya comenzó automáticamente

    void Start()
    {
        // Inicia la reproducción automática después del retraso configurado
        StartCoroutine(AutoStartRadio());
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleRadio();
        }
    }

    void ToggleRadio()
    {
        // Cambia el estado de la radio
        isRadioOn = !isRadioOn;

        if (isRadioOn)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
                Debug.Log("Radio encendida manualmente.");
            }
        }
        else
        {
            audioSource.Stop();
            Debug.Log("Radio apagada.");
        }
    }

    IEnumerator AutoStartRadio()
    {
        // Espera el tiempo definido en 'delay'
        yield return new WaitForSeconds(delay);

        // Inicia la canción automáticamente si la radio está apagada
        if (!isRadioOn)
        {
            isRadioOn = true;
            hasAutoStarted = true; // Marca que la canción se inició automáticamente
            audioSource.Play();
            //Debug.Log("Radio encendida automáticamente");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Activa el rango si el jugador está cerca
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            UIManager.Instance.ShowPanel(UIPanelTypeEnum.Interactive);
            //Debug.Log("Jugador cerca de la radio. Presiona 'E' para encender o apagar.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Desactiva el rango si el jugador se aleja
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            UIManager.Instance.HidePanel(UIPanelTypeEnum.Interactive);
            //Debug.Log("Jugador fuera del rango de la radio.");
        }
    }
}
