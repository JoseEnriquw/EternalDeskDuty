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
                ExecuteCorrectAction(); // Acción para código correcto
            }
            else
            {
                ExecuteIncorrectAction(); // Acción para código incorrecto
            }

            ResetInput(); // Reinicia después de evaluar
        }
    }

    private void ExecuteCorrectAction()
    {
        Debug.Log("¡Código correcto! Ejecutando acción...");
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
        Debug.Log("¡Código incorrecto! Intentar nuevamente...");
        nextloopsource.PlayOneShot(nextloop);
        Invoke("next", 5.0f);
        
        
    }
    public void ResetInput()
    {
        currentInput = "";
        displayText.text = "";
    }

}
