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
 

    void Start()
    {
        printsource = GetComponent<AudioSource>();
        Reports._PrintSanchezReport = false;
        Reports._PrintMartinezReports = false;
        Reports._PrintBossReports = false;
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
        Reports._PrintSanchezReport = print;
    }

    public void PrintMartinezReports(bool print)
    {
        Reports._PrintMartinezReports = print;
    }

    public void PrintBossReports(bool print)
    {
        Reports._PrintBossReports = print;
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
