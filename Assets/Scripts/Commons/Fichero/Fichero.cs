using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using UnityEngine;

public class Fichero : MonoBehaviour
{
    private bool isViewing =false;
    private bool inCollision =false;
    private bool isPanelsLocked =false;

    void Start()
    {
        GameManager.GetGameManager().OnPanelLockStateChanged += PanelsLocked;
    }

    // Update is called once per frame
    void Update()
    {
        InteractFichero();
    }

    private void OnTriggerStay(Collider other)
    {        
        if(other.CompareTag(Tags.Player))
        {
            if(!isViewing && !isPanelsLocked)UIManager.Instance.ShowPanel(UIPanelTypeEnum.Interactive);
            inCollision = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            UIManager.Instance.HidePanel(UIPanelTypeEnum.Interactive);
            inCollision = false;
        }
    }

    private void InteractFichero()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCollision)
        {
            if (isViewing)
            {
                UIManager.Instance.HidePanel(UIPanelTypeEnum.Fichero);
                GameManager.GetGameManager().SetEnablePlayerInput(true);
            }
            else
            {
                UIManager.Instance.ShowPanelFicheros();
                GameManager.GetGameManager().SetEnablePlayerInput(false);
            }
            isViewing = !isViewing;
        }
    }

    private void OnDestroy()
    {
        GameManager.GetGameManager().OnPanelLockStateChanged -= PanelsLocked;
    }

    private void PanelsLocked(bool isPanelsLocked)
    {
        this.isPanelsLocked = isPanelsLocked;
    }
}
