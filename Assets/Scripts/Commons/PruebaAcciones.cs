using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.UI;
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
                    UIManager.Instance.ShowPanelQuestionsAnswersAndAsignQuestionsAnswers("Option A or B?", "Option A", "Option B");
                    interacted = true;
                }
            }
                
        }
    }
}
