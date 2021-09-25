using System;
using System.Linq;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Settings.Input;
using UnityEditor;
using UnityEngine;

namespace Code.UI
{
    [Serializable]
    public class InputWithSprite
    {
        [ValueDropdown("GetInputNames")]
        public string inputName;
        public Sprite uiButtom;
        
        private List<string> GetInputNames()
        {
            var input = (new GameInput()).asset.actionMaps[0].actions;
            return input.Select(action => action.name).ToList();
        }
    }
    
    [CreateAssetMenu(fileName = "ButtonUIIcons", menuName = "Scriptables/InputIconsUI", order = 0)]
    public class InputSpritesUI: ScriptableObject
    {
        [SerializeField] private List<InputWithSprite> inputUIImages;

        public List<InputWithSprite> InputUIImages => inputUIImages;

        public Sprite GetByName(string name)
        {
            foreach (var inputUI in inputUIImages)
            {
                if (inputUI.inputName.Equals(name))
                    return inputUI.uiButtom;
            }

            return null;
        }

    }
}