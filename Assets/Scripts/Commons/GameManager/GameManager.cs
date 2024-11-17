using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using System.Collections;

namespace Assets.Scripts.Commons.GameManager
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager gameManager;
        public static GameManager GetGameManager() => gameManager;
        private DecisionsManager decisionsManager;

        public Action<bool> OnChangePlayerInput;
        public Action<Transform> OnChangePlayerPosition;
        
        
        private void Awake()
        {
            if (gameManager != null)
            {
                Destroy(gameObject);
                return;
            }
            decisionsManager = new();
            gameManager = this;
            DontDestroyOnLoad(this);
        }
        public void NextScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }  
        public void RestartScene(float seconds)
        {
            StartCoroutine(EjecutarConDelay(seconds, () => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }));
            CustomTimer.Instance.ResetTimer();
            
        }

        public void GoToScene(int scene)
        {
            SceneManager.LoadScene(scene);
        }

        public int GetSceneNumber()
        {
            return SceneManager.GetActiveScene().buildIndex;
        }

        public DecisionsManager GetDecisionsManager()
        {
            return decisionsManager;
        }

        public void SetEnablePlayerInput(bool enable)
        {
            OnChangePlayerInput?.Invoke(enable);
            if(enable) Cursor.lockState = CursorLockMode.Locked;
            else Cursor.lockState = CursorLockMode.None;
        }
        IEnumerator EjecutarConDelay(float seconds, Action action)
        {
            yield return new WaitForSeconds(seconds);

            // Código que se ejecuta después del delay
            Debug.Log($"Han pasado {seconds} segundos");
            action?.Invoke();
        }

        public void ChagePlayerPosition(Transform transform)
        {
            OnChangePlayerPosition?.Invoke(transform);
        }
    }
}
