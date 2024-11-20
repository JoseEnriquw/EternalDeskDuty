using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.GameManager;
using Assets.Scripts.Commons.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Phone : MonoBehaviour
{
    [SerializeField] private TMP_Text displayText;
    [SerializeField] private string secretCode = "1084";
    private string currentInput = "";
    private static Phone instance;
    public static Phone Instance => instance;

    [Header("Audios")]
    [SerializeField] private AudioClip gotout;
    [SerializeField] private AudioClip nextloop;

    private AudioSource gotoutSource, nextloopsource;
    private void Awake()
    {
        // Singleton Pattern
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
      
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {


        gotoutSource = GetComponents<AudioSource>()[0];
        nextloopsource = GetComponents<AudioSource>()[1];
        

    }

    void Update()
    {
        
    }

    public void OnButtonPress(string value)
    {
        
        currentInput += value;
        
        displayText.text = currentInput;

        if (currentInput.Length == secretCode.Length)
        {
            if (currentInput == secretCode)
            {
                ExecuteCorrectAction(); // Acci�n para c�digo correcto
            }
            else
            {
                ExecuteIncorrectAction(); // Acci�n para c�digo incorrecto
            }

            ResetInput(); // Reinicia despu�s de evaluar
        }
    }

    private void ExecuteCorrectAction()
    {
        Debug.Log("�C�digo correcto! Ejecutando acci�n...");
        gotoutSource.PlayOneShot(gotout);
        Invoke("theend", 5.0f);
       
        
    }
    public void theend()
    {
        UIManager.Instance.HidePanel(UIPanelTypeEnum.Telefono);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void next()
    {
        UIManager.Instance.HidePanel(UIPanelTypeEnum.Telefono);
        GameManager.GetGameManager().RestartScene(0);
    }
    private void ExecuteIncorrectAction()
    {
        Debug.Log("�C�digo incorrecto! Intentar nuevamente...");
        nextloopsource.PlayOneShot(nextloop);
        Invoke("next", 5.0f);
        
        
    }
    public void ResetInput()
    {
        currentInput = "";
        displayText.text = "";
    }

}
