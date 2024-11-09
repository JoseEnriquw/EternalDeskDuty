using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Commons.Observers
{
    public class ObserverConsequence : MonoBehaviour, IObserverConsequence
    {
        private void Start()
        {
            GameManager.GameManager.GetGameManager().GetDecisionsManager().SubscribeObserver(this); ;
        }

        public void UpdateConsequence(ConsequenceEnum consecuencia)
        {
            switch (consecuencia)
            {
                case ConsequenceEnum.ConsequenceA:
                    gameObject.transform.position += Vector3.right;
                    Debug.Log("Consecuencia A");
                    break;
                case ConsequenceEnum.ConsequenceB:
                    gameObject.transform.position += Vector3.left;
                    Debug.Log("Consecuencia B");
                    break;
                default:
                    break;
            }
        }
    }
}
