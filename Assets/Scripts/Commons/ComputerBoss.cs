using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerBoss : MonoBehaviour
{
    private string input;
    private string codigo = "015344769";
    [SerializeField] GameObject PantalaPass;
    [SerializeField] GameObject PantallaCompu;
    private static ComputerBoss instance;
    private bool isViewing = false;
    private bool inCollision = false;
    public bool lookingcomputer = false;
    Jefe _Jefe;
    public static ComputerBoss Instance => instance;
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
        _Jefe = FindObjectOfType<Jefe>();
        GameManager.GetGameManager().OnRestart += Reiniciarvariables;
    }
    private void OnDestroy()
    {
        GameManager.GetGameManager().OnRestart -= Reiniciarvariables;
    }

    private void Reiniciarvariables()
    {
        isViewing = false;
        inCollision = false;
        lookingcomputer = false;
    }

    void Update()
    {
        InteractComputerBoss();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Tags.Player) && !isViewing && _Jefe.BossisGone)
        {
            UIManager.Instance.ShowPanel(UIPanelTypeEnum.Interactive);
            inCollision = true;
        }
    }

    private void InteractComputerBoss()
    {
        if (Input.GetKeyDown(KeyCode.E) && inCollision)
        {
            if (isViewing)
            {

                UIManager.Instance.HidePanel(UIPanelTypeEnum.ComputerBoss);
                lookingcomputer = false;
                GameManager.GetGameManager().SetEnablePlayerInput(true);
            }
            else
            {               
                UIManager.Instance.ShowPanelComputerBoss();
                lookingcomputer = true;
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


    public void ReadPass(string pass)
    {
        input = pass;
        Debug.Log(input);
    }

    public void Ingresar()
    {
        if(input == codigo)
        {
            PantalaPass.SetActive(false);
            PantallaCompu.SetActive(true);
        }
        else
        {
            Debug.Log("reinicio el loop");
        }
    }
}
