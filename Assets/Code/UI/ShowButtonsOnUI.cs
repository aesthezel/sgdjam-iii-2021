using Code.Hero;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class ShowButtonsOnUI: MonoBehaviour
    {
        [SerializeField] private InputSpritesUI sprites;
        [SerializeField] private Image uiImage;
        [SerializeField] private InputMapper mapper;
        
        private void Start()
        {
            foreach (var input in sprites.InputUIImages)
            {
                var action = mapper.ActionMapper[input.inputName];
                action.start += f => ChangeImage(input.uiButtom);
                action.finished += ( ) => ChangeImage(null);
            }
        }

        private void ChangeImage(Sprite sprite) => uiImage.sprite = sprite;
    }
}