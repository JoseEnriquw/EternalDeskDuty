using Assets.Scripts.Commons.GameManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClienteEspecial : MonoBehaviour
{
    [SerializeField] private GameObject defaultPanel;       
    [SerializeField] private GameObject specialPanel;
    [SerializeField] private GameObject background;
    [SerializeField] private bool callLisen = false;
    Leonard_Coffe _lisencall;
    void Start()
    {
        _lisencall = FindObjectOfType<Leonard_Coffe>();
    }
     public void SetCallLisen(bool lisen)
    {
        callLisen=lisen;
    }

    public void OnButtonClick()
    {
        callLisen=_lisencall._ClienteEspecial;

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
