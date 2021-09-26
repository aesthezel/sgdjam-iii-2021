using System;
using System.Threading.Tasks;
using Code.Data;

namespace Code.Utils
{
    public class SoundEffect
    {
        private bool fadingIn = false, fadingOut = false;

        public async Task<bool> MaxTrackIn(Sound sound, float timeSpeed)
        {
            fadingIn = true;
            while (sound.Source.volume < 1)
            {
                if(fadingOut)
                {
                    ResetEffects(sound);
                    break;
                }

                sound.Source.volume += timeSpeed;
                await Task.Delay(60);
            }
            fadingIn = false;
            return true;
        }
    
        public async Task<bool> MaxTrackOut(Sound sound, float timeSpeed)
        {
            fadingOut = true;
            while (sound.Source.volume > 0)
            {
                if(fadingIn)
                {
                    ResetEffects(sound);
                    break;
                }

                sound.Source.volume -= timeSpeed;
                await Task.Delay(60);
            }
            fadingOut = false;
            return true;
        }

        private void ResetEffects(Sound sound)
        {
            sound.Source.volume = 0;
            fadingIn = false;
            fadingOut = false;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}