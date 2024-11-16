using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Character
{
    public class PlayerSounds : MonoBehaviour
    {
        [Header("Footstep Sounds")]
        [SerializeField] private List<AudioClip> audioClipSteps;

        [Header("Grab Object")]
        [SerializeField] private AudioClip grabObject;

        [Header("Drop Object")]
        [SerializeField] private AudioClip dropObject;

        private AudioSource audioSourceSteps, pickobject, _dropObject;
       

        private void Start()
        {
            audioSourceSteps = GetComponents<AudioSource>()[0];
            pickobject = GetComponents<AudioSource>()[1];
        }

        public void PlaySteps()
        {


            if (audioClipSteps.Count > 0)
            {
                AudioClip audioClip = audioClipSteps[Random.Range(0, audioClipSteps.Count)];
                audioSourceSteps.PlayOneShot(audioClip);
            }
        }
        public void PlayPickObject()
        {   
             audioSourceSteps.PlayOneShot(grabObject);            
        }

        public void PlayDropObject()
        {
            audioSourceSteps.PlayOneShot(dropObject);
        }
    }
}
