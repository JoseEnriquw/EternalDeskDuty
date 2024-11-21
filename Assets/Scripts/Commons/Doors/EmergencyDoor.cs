using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmergencyDoor : MonoBehaviour
{

    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Restart();
        }
    }

    void Restart()
    {
        UIManager.Instance.HidePanel(UIPanelTypeEnum.Interactive);
        GameManager.GetGameManager().RestartScene(1f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            UIManager.Instance.ShowPanel(UIPanelTypeEnum.Interactive);
            playerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            UIManager.Instance.HidePanel(UIPanelTypeEnum.Interactive);
            playerInRange = false;
        }
    }
}
