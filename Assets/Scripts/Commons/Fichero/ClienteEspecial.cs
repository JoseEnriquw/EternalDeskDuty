using Assets.Scripts.Commons.GameManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClienteEspecial : MonoBehaviour
{
    [SerializeField] private GameObject defaultPanel;       
    [SerializeField] private GameObject specialPanel;
    [SerializeField] private GameObject background;
    [SerializeField] private bool callLisen;
    void Start()
    {
        // Player = FindObjectOfType<Player>();
    }

    public void OnButtonClick()
    {
        if (callLisen)   // Verifica el estado del booleano en GameManager
        {            
            specialPanel.SetActive(true);
            defaultPanel.SetActive(false);
            background.SetActive(true );
        }
        else
        {           
            defaultPanel.SetActive(true);
            specialPanel.SetActive(false);
            background.SetActive(true);
        }
    }
}
