using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Code.Hero.Interactions
{
    [CreateAssetMenu(fileName = "Task Scriptable", menuName = "Alonne/Interactions/Task", order = 0)]
    public class SOTask : ScriptableObject
    {
        private List<SOInteraction> _interactions;

        public InputAction action;

        // public GetInteractionByName(string name)
        // {
        //     SOInteraction interaction = Array.Find(_interactions, )
        // }
    }
}