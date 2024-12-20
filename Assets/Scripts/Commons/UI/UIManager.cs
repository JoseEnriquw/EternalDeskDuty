﻿using Assets.Scripts.Commons.Enums;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Commons.UI
{
    public class UIManager : MonoBehaviour
    {
        private static UIManager instance;
        public static UIManager Instance => instance;
        private UIPanelTypeEnum? currentPanel = null;

        // Panel de Indicaciones
        [Header("Panel de Indicaciones")]
        [SerializeField] private GameObject panelIndications;
        [SerializeField] private TextMeshProUGUI txtIndications;

        // Panel de Preguntas y Respuestas
        [Header("Panel de Preguntas y Respuestas")]
        [SerializeField] private GameObject panelQuestionsAnswers;
        [SerializeField] private TextMeshProUGUI txtQuestion, txtAnswerA, txtAnswerB;
        [SerializeField] private Button buttonAnswerA, buttonAnswerB;

        [Header("Panel Fichero")]
        [SerializeField] private GameObject PanelFichero;
        [Header("Panel Interactive")]
        [SerializeField] private GameObject PanelInteractive;
        [Header("Panel Computadora")]
        [SerializeField] private GameObject PanelComputer;
        [Header("Panel Impresora")]
        [SerializeField] private GameObject PanelPrinter;
        [Header("Panel Fridge")]
        [SerializeField] private GameObject PanelFridge;
        [Header("Panel Board")]
        [SerializeField] private GameObject PanelBoard;
        [Header("Panel ComputerBoss")]
        [SerializeField] private GameObject PanelComputerBoss;

        private Dictionary<UIPanelTypeEnum, GameObject> panels;
        private bool panelsLocked = false;

        private void Awake()
        {
            // Singleton Pattern
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            // Inicializar el diccionario de paneles
            panels = new Dictionary<UIPanelTypeEnum, GameObject>
            {
                { UIPanelTypeEnum.Indications, panelIndications },
                { UIPanelTypeEnum.QuestionsAnswers, panelQuestionsAnswers },
                { UIPanelTypeEnum.Fichero, PanelFichero },
                { UIPanelTypeEnum.Interactive, PanelInteractive },               
                { UIPanelTypeEnum.Computer, PanelComputer },
                { UIPanelTypeEnum.Printer, PanelPrinter },
                { UIPanelTypeEnum.Fridge, PanelFridge },
                { UIPanelTypeEnum.Board, PanelBoard },
                 { UIPanelTypeEnum.ComputerBoss,PanelComputerBoss },
            };
            GameManager.GameManager.GetGameManager().OnPanelLockStateChanged += SetPanelLockState;
        }

        private void SetPanelLockState(bool locked)
        {
            panelsLocked = locked;

            if (panelsLocked)
            {
                // Hide all panels except Indications and Interactive
                foreach (var panelEntry in panels)
                {
                    if (panelEntry.Key != UIPanelTypeEnum.Indications && panelEntry.Key != UIPanelTypeEnum.Interactive)
                    {
                        panelEntry.Value.SetActive(false);
                    }
                }
            }
        }

        public void ShowPanel(UIPanelTypeEnum typePanel)
        {
            if (typePanel == UIPanelTypeEnum.Indications)
            {
                bool otherPanelsActive = panels.Any(x=> x.Key != UIPanelTypeEnum.Interactive && x.Key != UIPanelTypeEnum.Interactive && x.Value.activeSelf);

                if (currentPanel == UIPanelTypeEnum.Interactive || currentPanel== UIPanelTypeEnum.Indications || !otherPanelsActive)
                {
                    if (!panelIndications.activeSelf)
                    {
                        panelIndications.SetActive(true);
                    }
                }
            }
            else
            {
                if(panelsLocked) return;
                var isValid = panels.TryGetValue(currentPanel.GetValueOrDefault(), out GameObject panelToHide);
                var isCurrentPanel = currentPanel == typePanel;
                if (!isValid || !isCurrentPanel || !panelToHide.activeSelf)
                {
                    if(!isCurrentPanel && isValid) panelToHide.SetActive(false);

                    if (panels.TryGetValue(typePanel, out GameObject panelToShow))
                    {
                        panelToShow.SetActive(true);
                        currentPanel=typePanel;
                    }
                    else
                    {
                        Debug.LogError($"El panel {typePanel} no se encontró en el diccionario de paneles.");
                    }
                }
            }
        }

        public void HidePanel(UIPanelTypeEnum typePanel)
        {
            if (panels.TryGetValue(typePanel, out GameObject panelToShow))
            {
                panelToShow.SetActive(false);
            }
            else
            {
                Debug.LogError($"El panel {typePanel} no se encontró en el diccionario de paneles.");
            }
        }

        // Métodos específicos opcionales
        public void ShowPanelIndicationsAnAddIndications(string text)
        {
            ShowPanel(UIPanelTypeEnum.Indications);
            AsignTextIndications(text);
        }

        public void ShowPanelQuestionsAnswersAndAsignQuestionsAnswers(string question, string answerA, string answerB, ActionEnum actionA, ActionEnum actionB)
        {
            GameManager.GameManager.GetGameManager().SetEnablePlayerInput(false);
            ShowPanel(UIPanelTypeEnum.QuestionsAnswers);

            AsignQuestionsAnswers(question, answerA, answerB);
            InicialiceEventsButtons(actionA, actionB);
        }

        public void ShowPanelFicheros()
        {           
            GameManager.GameManager.GetGameManager().SetEnablePlayerInput(false);            
            ShowPanel(UIPanelTypeEnum.Fichero);
           
        }
        public void ShowPanelComputer()
        {
            GameManager.GameManager.GetGameManager().SetEnablePlayerInput(false);
            ShowPanel(UIPanelTypeEnum.Computer);

        }
        public void ShowPanelFridge()
        {
            GameManager.GameManager.GetGameManager().SetEnablePlayerInput(false);
            ShowPanel(UIPanelTypeEnum.Fridge);

        }
        
        public void ShowPanelComputerBoss()
        {
            GameManager.GameManager.GetGameManager().SetEnablePlayerInput(false);
            ShowPanel(UIPanelTypeEnum.ComputerBoss);

        }
        public void ShowPanelBoard()
        {
            GameManager.GameManager.GetGameManager().SetEnablePlayerInput(false);
            ShowPanel(UIPanelTypeEnum.Board);

        }
        public void ShowPanelPrinter()
        {
            Cursor.lockState = CursorLockMode.None;
            GameManager.GameManager.GetGameManager().SetEnablePlayerInput(false);
            ShowPanel(UIPanelTypeEnum.Printer);

        }

        public void AsignTextIndications(string text)
        {
            if (txtIndications != null)
            {
                txtIndications.text = text;
            }
            else
            {
                Debug.LogError("El campo 'textoIndicaciones' no está asignado en el UIManager.");
            }
        }

        private void InicialiceEventsButtons(ActionEnum actionA, ActionEnum actionB)
        {
            if (buttonAnswerA != null)
            {
                buttonAnswerA.onClick.RemoveAllListeners();
                buttonAnswerA.onClick.AddListener(()=>OnSelectedAnswer(actionA));
            }
            else
            {
                Debug.LogError("El botón 'botonRespuestaA' no está asignado en el UIManager.");
            }

            if (buttonAnswerB != null)
            {
                buttonAnswerB.onClick.RemoveAllListeners();
                buttonAnswerB.onClick.AddListener(()=>OnSelectedAnswer(actionB));
            }
            else
            {
                Debug.LogError("El botón 'botonRespuestaB' no está asignado en el UIManager.");
            }
        }

        public void AsignQuestionsAnswers(string question, string answerA, string answerB)
        {
            if (txtQuestion != null && buttonAnswerA != null && buttonAnswerB != null)
            {
                txtQuestion.text = question;
                txtAnswerA.text  = answerA;
                txtAnswerB.text = answerB;
            }
            else
            {
                Debug.LogError("Uno o más campos de texto en el panel de preguntas y respuestas no están asignados.");
            }
        } 

        private void OnSelectedAnswer(ActionEnum action)
        {
            GameManager.GameManager.GetGameManager().GetDecisionsManager().ExecuteAction(action);
            GameManager.GameManager.GetGameManager().SetEnablePlayerInput(true);
        }   
    }
}
