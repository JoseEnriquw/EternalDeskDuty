using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Fichero : MonoBehaviour
{
    private bool isViewing =false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {        
        string tag = other.tag;
        if(tag == "Player" && !isViewing)
        {
            UIManager.Instance.ShowPanelIndicationsAnAddIndications("Presiona E para Interactuar");
            if(Input.GetKeyDown(KeyCode.E))
            {
                isViewing = true;   
                UIManager.Instance.ShowPanelFicheros();
            }
        }
        else if(tag == "Player" && isViewing)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isViewing = false;
                UIManager.Instance.HidePanel(UIPanelTypeEnum.Fichero);
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        string tag = other.tag;
        if (tag == "Player")
        {
            UIManager.Instance.HidePanel(UIPanelTypeEnum.Indications);
        }
    }
}
