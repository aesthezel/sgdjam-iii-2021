using System;
using Code.Services;
using UnityEngine;

namespace Code.Testing
{
    public class PlayAnySong : MonoBehaviour
    {
        public string SoundName;
        
        public void ReproduceChoosedSound()
        {
            ServiceLocator.Instance.ObtainService<SoundService>().Play(SoundName, 1f);
        }
    }
}
