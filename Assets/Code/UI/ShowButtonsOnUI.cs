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

        private IEnumerator SpriteBackToNull(Image image)
        {
            yield return new WaitForSeconds(1f);
            image.sprite = null;
        }
    }
}