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
        public Action<int, Vector2> onMove;

        // Dictionary containing the input names and events
        public Dictionary<string, ActionSet> ActionMapper { get; private set; }

        private void Awake()
        {
            ActionMapper = new Dictionary<string, ActionSet>();
            
            var master = new GameInput();
            var inputNames = master.asset.actionMaps[0].actions; // TODO: Este [0] podria ser "Player" ??
            
            foreach (var input in inputNames)
            {
                ActionMapper.Add(input.name, new ActionSet());
            }
        }
    }
}