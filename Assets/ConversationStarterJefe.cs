using Assets.Scripts.Commons.GameManager;
using DialogueEditor;
using UnityEngine;

public class ConversationStarterJefe : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;
    [SerializeField] private float waitSecondsRestart;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetGameManager().SetEnablePlayerInput(false);
            ConversationManager.Instance.StartConversation(myConversation);
        }
    }

    public void ReiniciarEscena()
    {        
        GameManager.GetGameManager().RestartScene(waitSecondsRestart);
    }
    
}
