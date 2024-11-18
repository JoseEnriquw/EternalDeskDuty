using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Painting : MonoBehaviour
{
    private bool isViewing = false;
    private bool inCollision = false;
    void Start()
    {
        
    }
    private string[] phrases = {
    "This wasn't here before",
    "Something feels off...",
    "Did this just appear?",
    "I don't remember this being here...",
    "Am I imagining things?",
    "Was this here before?",
    "Something has changed...",
    "Why does this keep happening?",
    "Why are all these images of phones?",
    "I feel like someone is watching me...",   
    "I feel like I've been here before...",
    "Everything here feels... wrong.",
    "Am I stuck in a loop?",
    "I can't tell what's real anymore.",
    "The same picture again? Or is it different this time?",
    "The clock hasn't moved since I got here."
};


    // Update is called once per frame
    void Update()
    {
        //InteractPainting();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tags.Player) && !isViewing)
        {            
            inCollision = true;
            InteractPainting();
        }
    }

    private void InteractPainting()
    {
        if (inCollision)
        {
            if (isViewing)
            {
                UIManager.Instance.HidePanel(UIPanelTypeEnum.Indications);
               // GameManager.GetGameManager().SetEnablePlayerInput(true);
            }
            else
            {
                string randomPhrase = GetRandomPhrase();
                UIManager.Instance.ShowPanelIndicationsAnAddIndications("randomPhrase");
               // GameManager.GetGameManager().SetEnablePlayerInput(false);
            }
            isViewing = !isViewing;
        }
    }

    private string GetRandomPhrase()
    {
        // Selecciona una frase aleatoria de la lista
        int randomIndex = Random.Range(0, phrases.Length);
        return phrases[randomIndex];
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            UIManager.Instance.HidePanel(UIPanelTypeEnum.Indications);
            inCollision = false;
            isViewing = !isViewing;
            InteractPainting();
        }
    }
}
