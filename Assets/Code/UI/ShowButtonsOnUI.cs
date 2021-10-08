using System;
using System.Collections;
using Code.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class ShowButtonsOnUI : MonoBehaviour
    {
        [SerializeField] private InputSpritesUI sprites;
        [SerializeField] private Image[] images;
        private Coroutine _p1delayCoroutine;
        private Coroutine _p2delayCoroutine;

        private void Start()
        {
            var player = ServiceLocator.Instance.ObtainService<PlayerService>();
        }

        
        public void ChangeSkillIndicator(int mind, string actionName)
        {
            var image = images[mind];
            image.enabled = true;
            image.sprite = sprites.GetByName(actionName);
            
            if (mind == 0)
            {
                if(_p1delayCoroutine != null)
                    StopCoroutine(_p1delayCoroutine);
                _p1delayCoroutine = StartCoroutine(SpriteBackToNull(images[mind]));
            }
            
            else if (mind == 1)
            {
                if(_p2delayCoroutine != null)
                    StopCoroutine(_p2delayCoroutine);
                _p2delayCoroutine = StartCoroutine(SpriteBackToNull(images[mind]));
            }
        }
        
        public void ChangeMovementIndicator(int mind, string actionName, Vector2 movement)
        {
            var name = "Stop";
            var imageUI = images[mind + 2];

            if (movement.x > 0.05f)
                name = "Right";
            else if(movement.x < -0.05f)
                name = "Left";
            
            var image = sprites.GetByName(name);

            if (name.Equals("Stop"))
            {
                imageUI.enabled = false;
                return;
            }

            imageUI.enabled = true;
            imageUI.sprite = image;
        }

        private IEnumerator SpriteBackToNull(Image image)
        {
            yield return new WaitForSeconds(1f);
            image.enabled = false;
        }
    }
}