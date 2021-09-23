﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Settings.Input;

namespace Code.Hero
{
    public class ActionSet
    {
        public Func<float, bool> start = f => true; //The default start will always work
        public Action<float> ok;
        public Action failed;
    }
    
    public class InputMapper: MonoBehaviour
    {
        public Action<int, Vector2> OnMove;
        
        private GameInput _master;

        // Dictionary with all events
        public Dictionary<string, ActionSet> ActionMapper { get; } = new Dictionary<string, ActionSet>();

        private void Awake()
        {
            _master = new GameInput();
            var inputsNames = _master.asset.actionMaps[0].actions;
            
            foreach (var input in inputsNames)
            {
                ActionMapper.Add(input.name, new ActionSet());
            }
        }
    }
}