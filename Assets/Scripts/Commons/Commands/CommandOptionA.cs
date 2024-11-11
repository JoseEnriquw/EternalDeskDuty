using Assets.Scripts.Interfaces;
using UnityEngine;

namespace Assets.Scripts.Commons.Commands
{
    public class CommandOptionA : MonoBehaviour ,ICommand
    {
        public void Ejecutar()
        {
            Debug.Log("Commando A");
        }
    }
}
