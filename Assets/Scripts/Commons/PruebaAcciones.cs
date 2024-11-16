using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.UI;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Commons
{
    public class PruebaAcciones : MonoBehaviour
    {
        [SerializeField] private ActionEnum actionEnum;
        [SerializeField] private bool interacted;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player) && !interacted)
            {
                UIManager.Instance.ShowPanelIndicationsAnAddIndications("Presione \"E\" para interactuar!");
                if (Input.GetKeyDown(KeyCode.E))
                {
                    UIManager.Instance.ShowPanelQuestionsAnswersAndAsignQuestionsAnswers("Option A or B?", "Option A", "Option B", ActionEnum.AnswerQuestion, ActionEnum.OptionB);
                    interacted = true;
                }
            }
                
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player) && !interacted)
            {
                UIManager.Instance.HidePanel(Commons.Enums.UIPanelTypeEnum.Indications);
            }
                
        }
    }
}
