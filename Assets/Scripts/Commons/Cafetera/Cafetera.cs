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
    private bool inCollision=false;
    void Start()
    {
        GameManager.GetGameManager().GetDecisionsManager().SubscribeObserver(this);
    }

    // Update is called once per frame
    void Update()
    {
        MakeCoffee();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag(Tags.Player) && !interacted)
        {
            UIManager.Instance.ShowPanel(UIPanelTypeEnum.Interactive);
            inCollision = true;
        }
    }

    private void MakeCoffee()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCollision && !interacted)
        {
            UIManager.Instance.ShowPanelQuestionsAnswersAndAsignQuestionsAnswers("Make coffee?", "Yes", "No", ActionEnum.AnswerQuestion, ActionEnum.OptionB);
            interacted = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            UIManager.Instance.HidePanel(UIPanelTypeEnum.Interactive);
            inCollision = false;
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
