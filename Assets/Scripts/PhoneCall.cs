﻿using Assets.Scripts.Commons;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using DialogueEditor;
using UnityEngine;

namespace Assets.Scripts
{
    public class PhoneCall : MonoBehaviour
    {
        [SerializeField] private float waitTime;
        [SerializeField] private float waitTimeToRestart;
        [SerializeField] private NPCConversation myConversation;
        private AudioSource audioSource;
        private bool isCalling;
        private bool answered;
        private bool inCollision;
        private int Loop1=0;
        private bool inLoop = false;
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            isCalling = false;
            answered = false;
            inCollision=false;
        }

        private void Update()
        {
            if (waitTime <= CustomTimer.Instance.GetTimer())
            {
                if (!isCalling)
                {
                    audioSource.Play();
                    isCalling = true;
                    GameManager.GetGameManager().LockPanels();
                }
                AnsweringPhone();
            }
        }

        private void AnsweringPhone()
        {
            if (Input.GetKeyDown(KeyCode.E) && inCollision)
            {
                if (GameManager.GetGameManager().GetRestartCount() > 0)
                    inLoop = true;

                if (GameManager.GetGameManager().GetRestartCount() >1)
                    Loop1 = GameManager.GetGameManager().GetRestartCount();

                answered = true;
                inCollision = false;
                audioSource.Stop();
                UIManager.Instance.HidePanel(UIPanelTypeEnum.Interactive);
                GameManager.GetGameManager().SetEnablePlayerInput(false);
                UIManager.Instance.ShowPanel(UIPanelTypeEnum.Telefono);
                ConversationManager.Instance.StartConversation(myConversation);
                ConversationManager.Instance.SetBool("inLoop", inLoop);
                ConversationManager.Instance.SetInt("Loop1", Loop1);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Tags.Player) && isCalling && !answered)
            {
                UIManager.Instance.ShowPanel(UIPanelTypeEnum.Interactive);
                inCollision = true;
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Player) && isCalling)
            {
                UIManager.Instance.HidePanel(UIPanelTypeEnum.Interactive);
                inCollision = false;
            }
        }

        public void RestartScene()
        {
            GameManager.GetGameManager().RestartScene(waitTimeToRestart);
        }
    }
}
