using Assets.Scripts.Commons;
using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using UnityEngine;

namespace Assets.Scripts
{
    public class PhoneCall : MonoBehaviour
    {
        [SerializeField] private float waitTime;
        private AudioSource audioSource;
        private bool isCalling;
        private bool isCalled;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            isCalling = false;
            isCalled = false;
        }

        private void Update()
        {
            if (waitTime <= CustomTimer.Instance.GetTimer() && !isCalling)
            {
                audioSource.Play();
                isCalling = true;
                gameObject.tag=Tags.Interactive;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(Tags.Player) && isCalling && !isCalled)
            {
                UIManager.Instance.ShowPanel(UIPanelTypeEnum.Interactive);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    isCalled = true;
                    audioSource.Stop();
                    UIManager.Instance.HidePanel(UIPanelTypeEnum.Interactive);
                    GameManager.GetGameManager().RestartScene(2f);
                }
            }

        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(Tags.Player) && isCalling)
            {
                UIManager.Instance.HidePanel(UIPanelTypeEnum.Interactive);
            }
        }
    }
}
