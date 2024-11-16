using Assets.Scripts.Commons.Commands;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.UI;
using Assets.Scripts.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Commons.Factory
{
    public class FactoryCommands : MonoBehaviour
    {
        private static readonly Dictionary<ActionEnum, Func<ICommand>> mapaComandos = new Dictionary<ActionEnum, Func<ICommand>>
        {
            { ActionEnum.OptionA, () => new CommandOptionA() },
            { ActionEnum.OptionB, () => new CommandOptionB() },
        };
        public static ICommand GetCommand(ActionEnum action)
        {
            UIManager.Instance.HidePanel(UIPanelTypeEnum.QuestionsAnswers);

            if (mapaComandos.TryGetValue(action, out Func<ICommand> constructorComando))
            {
                return constructorComando();
            }
            else
            {
                throw new ArgumentException($"No se encontró un comando para la acción: {action}");
            }

        }
    }
}
