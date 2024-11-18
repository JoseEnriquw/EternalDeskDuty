using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeeBoard : MonoBehaviour
{
    private bool isViewing = false;
    private bool inCollision = false;
    [SerializeField] private List<GameObject> panels;
    int _scene = 0;
    private static SeeBoard instance;
    public static SeeBoard Instance => instance;
    private void Awake()
    {
        // Singleton Pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        if (transform.parent != null)
        {
            transform.SetParent(null);
        }
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        InteractBoard();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tags.Player) && !isViewing)
        {
            UIManager.Instance.ShowPanel(UIPanelTypeEnum.Interactive);
            inCollision = true;
        }
    }

    private void InteractBoard()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCollision)
        {
            if (isViewing)
            {

                UIManager.Instance.HidePanel(UIPanelTypeEnum.Board);
                GameManager.GetGameManager().SetEnablePlayerInput(true);
            }
            else
            {
                foreach (var panel in panels)
                {
                    panel.SetActive(false);
                }

                int indexToActivate = _scene % panels.Count;
                panels[indexToActivate].SetActive(true);
                UIManager.Instance.ShowPanelBoard();
                GameManager.GetGameManager().SetEnablePlayerInput(false);
            }
            isViewing = !isViewing;
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

}
