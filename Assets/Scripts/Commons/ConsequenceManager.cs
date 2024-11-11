using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.Interfaces;
using System.Collections.Generic;

namespace Assets.Scripts.Commons
{
    public class ConsequenceManager
    {
        private readonly List<IObserverConsequence> observers = new();

        public void Subscribe(IObserverConsequence observer)
        {
            observers.Add(observer);
        }

        public void Notify(ConsequenceEnum consequence)
        {
            foreach (var observer in observers)
            {
                observer.UpdateConsequence(consequence);
            }
        }
    }
}
