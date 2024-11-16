using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Commons.Observers
{
    public class ObserverConsequence : MonoBehaviour, IObserverConsequence
    {
        [SerializeField] private Material material; 
        private void Start()
        {
            GameManager.GameManager.GetGameManager().GetDecisionsManager().SubscribeObserver(this);
        }

        public void UpdateConsequence(ConsequenceEnum consecuencia)
        {
            switch (consecuencia)
            {
                case ConsequenceEnum.ConsequenceA:
                    material.color=Color.blue;
                    Debug.Log("Consecuencia A");
                    break;
                case ConsequenceEnum.ConsequenceB:
                    
                    material.color = Color.red;
                    Debug.Log("Consecuencia B");
                    break;
                default:
                    break;
            }
        }
    }
}
