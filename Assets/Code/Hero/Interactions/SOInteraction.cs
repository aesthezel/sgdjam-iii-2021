using UnityEngine;

namespace Code.Hero.Interactions
{
    public enum InteractionSignal
    {
        Unidirectional,
        Bidirectional,
        Error
    }
    
    
    [CreateAssetMenu(fileName = "FILENAME", menuName = "MENUNAME", order = 0)]
    public class SOInteraction : ScriptableObject
    {

        private Mind _firstMindPerforming = null;
        
        // public InteractionSignal Perform(Mind mind)
        // {
        //     _firstMindPerforming ??= mind;
        //
        //     if (_firstMindPerforming.ID == mind.ID) return InteractionSignal.Unidirectional;
        // }
    }
}