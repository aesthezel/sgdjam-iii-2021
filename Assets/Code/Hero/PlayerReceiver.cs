using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Code.Hero
{
    [RequireComponent(typeof(InputMapper))]
    public class PlayerReceiver : MonoBehaviour
    {
        // For buttons that require waiting
        private string _requiredActionToComplete;
        private Coroutine _waitingCoroutine;
        private float _timeUntilComplete;
        private int _firstMindId = -1;

        // Input Mapper
        private InputMapper _mapper;
        
        // --------------
        // UNITY METHODS
        // --------------
        private void Awake()
        {
            _mapper = GetComponent<InputMapper>();
        }

        private void Start()
        {
            // For each Input subscribe the failure effect
            var keys = _mapper.ActionMapper.Keys;
            
            foreach (var key in keys)
            {
                if (_mapper.ActionMapper.TryGetValue(key, out var action)) 
                    action.failed += ResetInputEventSystem;
            }
        }

        private void Update()
        {
            if (_waitingCoroutine != null)
                _timeUntilComplete += Time.deltaTime;
        }

        // --------------
        // INPUT METHODS
        // --------------
        public void MovementInput(int mindId, Vector2 velocity) => _mapper.OnMove?.Invoke(mindId, velocity);

        public void InputActionPerformed(int mindId, string actionName)
        {
            if (string.IsNullOrEmpty(_requiredActionToComplete))
                InputEventStart(mindId, actionName, 1f);
            
            else if(_firstMindId != mindId)
                InputEventFinish(mindId, actionName);
        }
        
        private void InputEventStart(int mindId, string actionName, float waitTime)
        {
            _requiredActionToComplete = actionName;
            _firstMindId = mindId;
            
            var success = _mapper.ActionMapper.TryGetValue(actionName, out var actions);
            
            Assert.IsTrue(success, $"InputAction name {actionName} not found in <ActionMapper>");

            // Event Starts: Methods are called sequentially so we can decide whether to run or not the Coroutine
            var delegates = actions.start.GetInvocationList();

            var outputs = delegates.Select(action => (bool) action.DynamicInvoke(waitTime)).ToList();
            
            if (!outputs.Contains(false))
                _waitingCoroutine = StartCoroutine(WaitForOtherMind(waitTime, actions.failed));
            else
            {
                Debug.Log($"Action <{_requiredActionToComplete}> was prohibited to perform");
                actions.failed?.Invoke();
            }
        }

        private void InputEventFinish(int mindId, string actionName)
        {
            var success = _mapper.ActionMapper.TryGetValue(actionName, out var actions);

            Assert.IsTrue(success, $"InputAction name {actionName} not found in <ActionMapper>");

            if (String.Equals(_requiredActionToComplete, actionName))
            {
                // Event completed correctly
                Debug.Log("IN TIME!");
                actions.ok?.Invoke(_timeUntilComplete);  
            }
            // Don't matter if the event was completed successfully or not when second player inout is detected
            // the event must finish to give chance to other events to perform
            actions.failed?.Invoke();
        }
        
        private IEnumerator WaitForOtherMind(float time, Action actionOnFail)
        {
            var elapsedTime = 0f;

            while (elapsedTime < time)
            {
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            
            Debug.Log("TIME OUT!!");
            actionOnFail?.Invoke();
        }

        private void ResetInputEventSystem()
        {
            if(_waitingCoroutine != null)
                StopCoroutine(_waitingCoroutine);
            
            _requiredActionToComplete = null;
            _firstMindId = -1;
            _timeUntilComplete = 0;
            _waitingCoroutine = null;
        }
    }
}