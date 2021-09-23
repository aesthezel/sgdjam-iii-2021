using System;
using System.Collections.Generic;
using UnityEngine;
using Settings.Input;

namespace Code.Hero
{
    public class ActionSet
    {
        public Func<float, bool> checker = f => true; //The default checker will always be true
        public Action<float> start;
        public Action<float> ok;
        public Action finished;
    }
    
    public class InputMapper: MonoBehaviour
    {
        public Action<int, Vector2> OnMove;
        
        private GameInput _master;

        // Dictionary containing the input names and events
        public Dictionary<string, ActionSet> ActionMapper { get; } = new Dictionary<string, ActionSet>();

        private void Awake()
        {
            ActionMapper = new Dictionary<string, ActionSet>();
            
            _master = new GameInput();
            var inputsNames = _master.asset.actionMaps[0].actions;
            
            foreach (var input in inputsNames)
            {
                ActionMapper.Add(input.name, new ActionSet());
            }
        }
    }
}