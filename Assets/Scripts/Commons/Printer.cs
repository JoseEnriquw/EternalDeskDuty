using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Printer : MonoBehaviour
{
    private bool isViewing = false;
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
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        string tag = other.tag;
        if (tag == "Player" && !isViewing)
        {
            UIManager.Instance.ShowPanelIndicationsAnAddIndications("Presiona E para Interactuar");
            if (Input.GetKeyDown(KeyCode.E))
            {
                isViewing = true;
                ButtonMartinez.SetActive(Reports._PrintMartinezReports);
                ButtonSanchez.SetActive(Reports._PrintSanchezReport);
                ButtonBoss.SetActive(Reports._PrintBossReports);
                UIManager.Instance.ShowPanelPrinter();
            }
        }
        else if (tag == "Player" && isViewing)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isViewing = false;
                UIManager.Instance.HidePanel(UIPanelTypeEnum.Fichero);
            }
        }

    }

    public void GrabMartinezReport(bool grab)
    {
        Reports.HasPrintMartinezReports = grab;
    }
    public void GrabSanchezReport(bool grab)
    {
        Reports.HasPrintSanchezReport = grab;
    }
    public void GrabBossReport(bool grab)
    {
        Reports.HasPrintBossReports = grab;
    }
}
