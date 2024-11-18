using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeBoard : MonoBehaviour
{
    private bool isViewing = false;
    private bool inCollision = false;
    void Start()
    {
        
    }

    void Update()
    {
        InteractBoard();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tags.Player) && !isViewing)
        {
            UIManager.Instance.ShowPanel(UIPanelTypeEnum.Interactive);
            inCollision = true;
        }
    }

    private void InteractBoard()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCollision)
        {
            if (isViewing)
            {

                UIManager.Instance.HidePanel(UIPanelTypeEnum.Fridge);
                GameManager.GetGameManager().SetEnablePlayerInput(true);
            }
            else
            {
               
                UIManager.Instance.ShowPanelBoard();
                GameManager.GetGameManager().SetEnablePlayerInput(false);
            }
            isViewing = !isViewing;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            UIManager.Instance.HidePanel(UIPanelTypeEnum.Interactive);
            inCollision = false;
        }
    }

}
