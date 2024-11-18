using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fridge : MonoBehaviour
{
    private bool isViewing = false;
    private bool inCollision = false;
    private bool CanIeatCake = false;
    [SerializeField] private GameObject cake;
    Kate _kate;

    private static Fridge instance;
    public static Fridge Instance => instance;
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
    void Start()
    {
        _kate = FindObjectOfType<Kate>();
    }

    // Update is called once per frame
    void Update()
    {
        InteractFridge();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tags.Player) && !isViewing)
        {
            UIManager.Instance.ShowPanel(UIPanelTypeEnum.Interactive);
            inCollision = true;
        }
    }

    private void InteractFridge()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCollision)
        {
            if (isViewing)
            {
               
                UIManager.Instance.HidePanel(UIPanelTypeEnum.Fridge);
                GameManager.GetGameManager().SetEnablePlayerInput(true);
            }
            else
            {
                //si no funciona revisar dialogo que llame a la funcion canieatcake
                CanIeatCake = _kate.Cake;
                cake.SetActive(CanIeatCake);
                UIManager.Instance.ShowPanelFridge();
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
