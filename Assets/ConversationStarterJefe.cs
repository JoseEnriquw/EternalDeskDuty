using Assets.Scripts.Commons.GameManager;
using DialogueEditor;
using JetBrains.Annotations;
using UnityEngine;

public class ConversationStarterJefe : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    [SerializeField] private float waitSecondsRestart;
    [SerializeField] Transform tp;
    Kate _kate;
    ComputerBoss _ComputerBoss;
    Jefe _jefe;
    private void Start()
    {
        _kate = FindObjectOfType<Kate>();
        _ComputerBoss = FindObjectOfType<ComputerBoss>();
        _jefe = FindObjectOfType<Jefe>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_ComputerBoss.lookingcomputer && _jefe.isSit)
        {
            GameManager.GetGameManager().SetEnablePlayerInput(false);
            ConversationManager.Instance.StartConversation(myConversation);
            if (Reports.HasPrintMartinezReports || Reports.HasPrintSanchezReport || Reports.HasPrintBossReports)
                ConversationManager.Instance.SetBool("tieneInforme", true);
            ConversationManager.Instance.SetBool("SanchezReport", Reports.HasPrintSanchezReport);
            ConversationManager.Instance.SetBool("MartinezReport", Reports.HasPrintMartinezReports);
            ConversationManager.Instance.SetBool("BossReport", Reports.HasPrintBossReports);
            ConversationManager.Instance.SetBool("deliveryreport", Reports.DeliveryBossReports);
            ConversationManager.Instance.SetBool("KateBirthday", _kate.Cake);

        }
    }

    public void ReiniciarEscena()
    {        
        GameManager.GetGameManager().RestartScene(waitSecondsRestart);
    }
    public void Teleport()
    {
        GameManager.GetGameManager().ChagePlayerPosition(tp);
    }

    public void deliverBossreport(bool deliver)
    {
        Reports.DeliveryBossReports = deliver;
    }
}
