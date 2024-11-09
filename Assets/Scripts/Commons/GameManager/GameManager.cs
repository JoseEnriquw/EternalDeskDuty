using UnityEngine.SceneManagement;
using UnityEngine;

namespace Assets.Scripts.Commons.GameManager
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager gameManager;
        public static GameManager GetGameManager() => gameManager;
        private DecisionsManager decisionsManager;
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
    }
}
