using Assets.Scripts.Commons.Constants;
using Assets.Scripts.Commons.Enums;
using Assets.Scripts.Commons.UI;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

namespace Assets.Scripts.Commons
{
    public class JefeCharla : MonoBehaviour
    {
        [SerializeField] private ActionEnum actionEnum;
        [SerializeField] private bool interacted;

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player) && !interacted)
            {

                //UIManager.Instance.ShowPanelQuestionsAnswersAndAsignQuestionsAnswers("Jefe: \"Estoy ocupado. Esto más vale que sea importante.\"", "Lo es. Necesito discutir algo urgente.", "No, solo quería verificar si necesitas algo.");
                interacted = true;

            }

        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag(Tags.Player) && !interacted)
            {
                UIManager.Instance.HidePanel(Commons.Enums.UIPanelTypeEnum.Indications);
            }

        }
        private void Respuesta2()
        {
            //no solo queria verificar
            //UIManager.Instance.ShowPanelQuestionsAnswersAndAsignQuestionsAnswers("Jefe: \"Ya sabes qué necesito. Y no es más tiempo perdido.\"", "Sí, jefe. Lo tendré listo enseguida.", "Entiendo. Me pondré a ello de inmediato");
            // el jugador sale de la oficina y se cierra la puerta
        }
        private void Respuesta1()
        {
            //si es urgente
            //si tiene el informe
            //UIManager.Instance.ShowPanelQuestionsAnswersAndAsignQuestionsAnswers("Jefe: \"Entonces, ¿qué es tan importante que no podía esperar?\"", "Eh... traje el informe que pidió. Pensé que era mejor entregarlo de inmediato.", "¿Viste el partido anoche? ¡Un golazo! Estuvo tremendo.");
            //si le habla del partido: ¡Sí, cómo no! Ese gol fue una locura.
            // devuelve una sola opcion entregar informe

            //le entrega informe: ¡Bien hecho! Te felicito, esto es justo lo que necesitaba. Ahora, escucha bien… vas a recibir una llamada más tarde, de la gente de la empresa X. Es crucial, pero algo no huele bien.  Lo que está en juego es más grande de lo que parece. Todo lo que necesitas esta en la oficina


            //si no tiene el informe
            //UIManager.Instance.ShowPanelQuestionsAnswersAndAsignQuestionsAnswers("Jefe: \"Entonces, ¿qué es tan importante que no podía esperar?\"", "Escuché algo interesante sobre lo que pasó con Marta de contabilidad...", "Estaba pensando en tomar unos días libres para descansar.");
            // jefe se enoja y dice:¿Vienes aquí para hablar de chismes? Te pagaré por resultados, no por rumores." reinicia escena
            // jefe se enoja y dice: ¿Días libres? ¿Es esto lo que consideras 'importante'? Espero que tengas tu trabajo terminado primero." - reinicia escena
        }
    }

}