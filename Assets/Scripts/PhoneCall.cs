using Assets.Scripts.Commons;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using DialogueEditor;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class PhoneCall : MonoBehaviour
    {
        [SerializeField] private float waitTime;
        [SerializeField] private float waitTimeToRestart;
        [SerializeField] private NPCConversation myConversation;
        [SerializeField] private float timeToAnswer;
        private AudioSource audioSource;
        private bool isCalling;
        private bool answered;
        private bool inCollision;
        private int Loop1=0;
        private bool inLoop = false;
        private float answerTimer;
        private bool isRunning;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            isCalling = false;
            answered = false;
            inCollision=false;
            answerTimer=0;
            isRunning = false;
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
                    isRunning = true;
                }
                AnsweringPhone();
                if (isRunning)
                {
                    if (answerTimer >= timeToAnswer && !answered)
                    {
                        RestartScene();
                    }
                    answerTimer += Time.deltaTime;
                }
            }
        }

        private void AnsweringPhone()
        {
            if (Input.GetKeyDown(KeyCode.E) && inCollision)
            {
                isRunning = false;
                if (GameManager.GetGameManager().GetRestartCount() > 0)
                    inLoop = true;
                    
                if (GameManager.GetGameManager().GetRestartCount() >1)
                    Loop1 = GameManager.GetGameManager().GetRestartCount();

                answered = true;
                inCollision = false;
                audioSource.Stop();
                UIManager.Instance.HidePanel(UIPanelTypeEnum.Interactive);
                GameManager.GetGameManager().SetEnablePlayerInput(false);
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

        public void MostrarTelefono()
        {
            StartCoroutine(EjecutarConDelay(2f, () => 
            { 
                UIManager.Instance.ShowPanel(UIPanelTypeEnum.Telefono);
                GameManager.GetGameManager().SetEnablePlayerInput(false);
            }));
        }

        IEnumerator EjecutarConDelay(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);

            // Código que se ejecuta después del delay
            Debug.Log($"Han pasado {seconds} segundos");
            action?.Invoke();
        }
    }
}
