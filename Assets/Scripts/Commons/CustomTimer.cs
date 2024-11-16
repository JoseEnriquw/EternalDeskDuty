using UnityEngine;

namespace Assets.Scripts.Commons
{

    public class CustomTimer : MonoBehaviour
    {
        private float timer = 0f;

        public bool isRunning = true;
        public static CustomTimer Instance; 

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(this);
        }

        void Update()
        {
            if (isRunning)
            {
                timer += Time.deltaTime;
            }
        }

        public void ResetTimer()
        {
            timer = 0f;
        }

        public float GetTimer()
        {
            return timer;
        }
    }

}
