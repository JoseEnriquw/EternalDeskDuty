using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : MonoBehaviour
{
    private bool isViewing = false;
    private bool inCollision = false;
    Computer _computer;
   [SerializeField] private GameObject ButtonMartinez;
    [SerializeField] private GameObject ButtonSanchez;
    [SerializeField] private GameObject ButtonBoss;
    void Start()
    {
        _computer = FindObjectOfType<Computer>();
        Reports.HasPrintBossReports = false;
        Reports.HasPrintMartinezReports = false;
        Reports.HasPrintSanchezReport = false;

        Reports.DeliverySanchezReport = false;
        Reports.DeliveryMartinezReports = false;
        Reports.DeliveryBossReports = false;

    }

    // Update is called once per frame
    void Update()
    {
        InteractPrinter();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tags.Player) && !isViewing)
        {
            UIManager.Instance.ShowPanel(UIPanelTypeEnum.Interactive);
            inCollision = true;
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
    private void InteractPrinter()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCollision)
        {
            if (isViewing)
            {
                UIManager.Instance.HidePanel(UIPanelTypeEnum.Printer);
                GameManager.GetGameManager().SetEnablePlayerInput(true);
            }
            else
            {
                ButtonMartinez.SetActive(Reports._PrintMartinezReports);
                ButtonSanchez.SetActive(Reports._PrintSanchezReport);
                ButtonBoss.SetActive(Reports._PrintBossReports);
                UIManager.Instance.ShowPanelPrinter();
                GameManager.GetGameManager().SetEnablePlayerInput(false);
            }
            isViewing = !isViewing;
        }
    }

    public void GrabMartinezReport(bool grab)
    {
        Reports.HasPrintMartinezReports = grab;
        Reports._PrintMartinezReports = !grab;
    }
    public void GrabSanchezReport(bool grab)
    {
        Reports.HasPrintSanchezReport = grab;
        Reports._PrintSanchezReport = !grab;
    }
    public void GrabBossReport(bool grab)
    {
        Reports.HasPrintBossReports = grab;
        Reports._PrintBossReports = !grab;
    }
}
