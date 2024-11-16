using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.Interfaces;
using Assets.Scripts.Commons.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cafetera : MonoBehaviour, IObserverConsequence
{
    public bool HasCoffee;
    [SerializeField] private bool interacted= false;
    void Start()
    {
        GameManager.GetGameManager().GetDecisionsManager().SubscribeObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player) && !interacted)
        {
            UIManager.Instance.ShowPanelIndicationsAnAddIndications("Presione \"E\" para interactuar!");
            if (Input.GetKeyDown(KeyCode.E))
            {
                UIManager.Instance.ShowPanelQuestionsAnswersAndAsignQuestionsAnswers("Make coffee?", "Yes", "No", ActionEnum.AnswerQuestion, ActionEnum.OptionB);
                interacted = true;
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Me aleje de la cafetera");
            
        }
    }

    public void UpdateConsequence(ConsequenceEnum consecuencia)
    {
        switch (consecuencia)
        {
            case ConsequenceEnum.ConsequenceA:
                HasCoffee = true;
                Debug.Log("Hicimos cafe");
                break;
            case ConsequenceEnum.ConsequenceB:
                HasCoffee = false;
                Debug.Log("No hicimos cafe");
                break;
            default:
                break;
        }
    }
}
