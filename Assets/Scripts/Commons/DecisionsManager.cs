using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.Factory;
using Assets.Scripts.Commons.Interfaces;
using Assets.Scripts.Interfaces;
using System;

namespace Assets.Scripts.Commons
{
    public class DecisionsManager
    {
        private readonly ConsequenceManager consequenceManager = new();

        public void ExecuteAction(ActionEnum action)
        {
            ICommand comando = FactoryCommands.GetCommand(action);
            comando.Ejecutar();

            ConsequenceEnum consecuencia = DetermineConsequence(action);
            consequenceManager.Notify(consecuencia);
        }

        private ConsequenceEnum DetermineConsequence(ActionEnum action)
        {
            // Lógica para mapear acciones a consecuencias
            return action switch
            {
                ActionEnum.OptionA => ConsequenceEnum.ConsequenceA,
                ActionEnum.OptionB => ConsequenceEnum.ConsequenceB,
                _ => throw new ArgumentException("Acción no reconocida"),
            };
        }

        public void SubscribeObserver(IObserverConsequence observer)
        {
            consequenceManager.Subscribe(observer);
        }
    }
}
