using Assets.Scripts.Commons.GameManager;
using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationStarterJefe : MonoBehaviour
{
    [SerializeField] private NPCConversation myConversation;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GetGameManager().SetEnablePlayerInput(false);
            Cursor.lockState = CursorLockMode.None;
            ConversationManager.Instance.StartConversation(myConversation);

           // GameManager.GetGameManager().SetEnablePlayerInput(true);
        }
    }
}
