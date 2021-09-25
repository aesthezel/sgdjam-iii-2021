using System;
using System.Collections;
using System.Linq;
using Code.Hero;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class ShowButtonsOnUI : MonoBehaviour
    {
        [SerializeField] private InputSpritesUI sprites;
        [SerializeField] private Image[] images;
        private Coroutine _delayCoroutine;
        
        public void ChangeImage(int mind, string actionName)
        {
            images[mind].sprite = sprites.GetByName(actionName);
            
            if(_delayCoroutine != null)
                StopCoroutine(_delayCoroutine);
            
            _delayCoroutine = StartCoroutine(SpriteBackToNull(images[mind]));
        }
        
        public void ChangeImageMovement(int mind, string actionName, Vector2 movement)
        {
            var name = "Stop";
            
            if (movement.x > 0.05f)
                name = "Right";
            else if(movement.x < -0.05f)
                name = "Left";
            
            var image = sprites.GetByName(name);

            if (name.Equals("Stop"))
                image = null;

            images[mind + 2].sprite = image;
        }

        private IEnumerator SpriteBackToNull(Image image)
        {
            yield return new WaitForSeconds(1f);
            image.sprite = null;
        }
    }
}