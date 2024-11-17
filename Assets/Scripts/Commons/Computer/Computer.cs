using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using UnityEngine;

public class Computer : MonoBehaviour
{
    private bool isViewing =false;
    private bool inCollision =false;

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
        InteractComputer();
    }
    private void OnTriggerStay(Collider other)
    {        
        if(other.CompareTag(Tags.Player) && !isViewing)
        {
            UIManager.Instance.ShowPanel(UIPanelTypeEnum.Interactive);
            inCollision = true;
        }
    }

    private void InteractComputer()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCollision)
        {
            if (isViewing)
            {
                UIManager.Instance.HidePanel(UIPanelTypeEnum.Computer);
                GameManager.GetGameManager().SetEnablePlayerInput(true);
            }
            else
            {
                UIManager.Instance.ShowPanelComputer();
                GameManager.GetGameManager().SetEnablePlayerInput(false);
            }
            isViewing = !isViewing;
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
        return name switch
        {
            "PrintSanchezReport" => _PrintSanchezReport,
            "PrintMartinezReports" => _PrintMartinezReports,
            "PrintBossReports" => _PrintBossReports,
            _ => false,
        };
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
