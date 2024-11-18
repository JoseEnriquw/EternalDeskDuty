using Assets.Scripts.Commons.GameManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    [SerializeField] private List<GameObject> panels;

    private static Board instance;
    public static Board Instance => instance;
    int _scene = 0;
    private void Awake()
    {
        //Singleton Pattern
        //if (instance != null && instance != this)
        //{
        //    Destroy(gameObject);
        //    return;
        //}
        //instance = this;
        //if (transform.parent != null)
        //{
        //    transform.SetParent(null);
        //}
        //DontDestroyOnLoad(gameObject);
        //_scene = GameManager.GetGameManager().GetRestartCount();
        //ActivateCurrentPanel();
    }
    void Start()
    {
       
    }
    private void ActivateCurrentPanel()
    {

        //foreach (var panel in panels)
        //{
        //    panel.SetActive(false);
        //}

        //int indexToActivate = _scene % panels.Count;
        //panels[indexToActivate].SetActive(true);
    }
   
    void Update()
    {
        
    }
}
