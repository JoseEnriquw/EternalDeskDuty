using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Computer : MonoBehaviour
{
    private bool isViewing =false;

    [Header("Audios")]
    [SerializeField] private AudioClip _print;
    private AudioSource printsource;
    private bool _PrintSanchezReport =false;
    private bool _PrintMartinezReports = false;
    private bool _PrintBossReports = false;

    void Start()
    {
        printsource = GetComponent<AudioSource>();
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
                UIManager.Instance.ShowPanelComputer();
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

    public void PrintSound()
    {
        printsource.PlayOneShot(_print);
    }

    public void PrintSanchezReport(bool print)
    {
        _PrintSanchezReport = print;
    }

    public void PrintMartinezReports(bool print)
    {
        _PrintMartinezReports = print;
    }

    public void PrintBossReports(bool print)
    {
        _PrintBossReports = print;
    }

    public bool Deliveryreport(string name)
    {
        switch (name)
        {
            case "PrintSanchezReport":
                return _PrintSanchezReport;
            case "PrintMartinezReports":
                return _PrintMartinezReports;
            case "PrintBossReports":
                return _PrintBossReports;
            default: return false;
                
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
