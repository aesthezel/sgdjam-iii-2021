using System.Collections;
using Code.Data;
using Code.Hero;
using Code.Services;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace Code.Utils
{
    [RequireComponent(typeof(PlayerController))]
    public class InputEffectsSubscriber: MonoBehaviour
    {
        [BoxGroup("CAMERA EFFECTS")]
        [TitleGroup("CAMERA EFFECTS/DASH")] 
        [SerializeField] private FloatData lookAheadDistance;
        [TitleGroup("CAMERA EFFECTS/DASH")] 
        [SerializeField] private FloatData sustainTime;
        [TitleGroup("CAMERA EFFECTS/DASH")]
        [SerializeField] private FloatData inTime;
        [TitleGroup("CAMERA EFFECTS/DASH")]
        [SerializeField] private FloatData outTime;
        
        [TitleGroup("CAMERA EFFECTS/JUMP")]
        [SerializeField] private FloatData startTargetOrtoSize;
        [TitleGroup("CAMERA EFFECTS/JUMP")]
        [SerializeField] private FloatData normalOrtoSize;
        [TitleGroup("CAMERA EFFECTS/JUMP")]
        [SerializeField] private FloatData endZoomOutTime;

        [TitleGroup("CAMERA EFFECTS/TIME DELAY")] [SerializeField]
        private LayerMask delayTriggerers;
        
        private CameraEffects _effects;
        private PlayerController _controller;
        private InputMapper _mapper;

        private void Start()
        {
            var player = ServiceLocator.Instance.ObtainService<PlayerService>();
            _effects = ServiceLocator.Instance.ObtainService<CameraEffects>();
            _controller = player.Controller;
            _mapper = player.Mapper;
            
            JumpEffects();
            DashEffects();
        }

        private void JumpEffects()
        {
            var done = _mapper.ActionMapper.TryGetValue("Jump", out var actions);
            Assert.IsTrue(done, "Error retrieving <Jump> input");
            
            //actions.start += TimeDelay;
            actions.start += f => _effects.DoOrtoSize(startTargetOrtoSize.Value, f/3, Camera.Ease.InQuad);
            actions.finished += () => _effects.DoOrtoSize(startTargetOrtoSize.Value * -1, endZoomOutTime.Value, Camera.Ease.OutQuad);
        }

        private void DashEffects()
        {
            var done = _mapper.ActionMapper.TryGetValue("Dash", out var actions);
            Assert.IsTrue(done, "Error retrieving <Dash> input");
            
            actions.start += TimeDelay;
            actions.ok += f => _effects.DoLookAhead(
                lookAheadDistance.Value,
                _controller.FacingRight ? 1 : -1,
                sustainTime.Value,
                inTime.Value,
                outTime.Value);
        }
        
        // TODO: Mal Optimizado
        private void TimeDelay(float time)
        {
            var objs = Physics2D.OverlapCircle(_controller.transform.position, 1, delayTriggerers);
            if (objs != null)
            {
                StartCoroutine(ProgressiveDelay(time));
            }
        }

        private IEnumerator ProgressiveDelay(float time)
        {
            Time.timeScale = 0.7f;
            yield return new WaitForSeconds(time);
            Time.timeScale = 1f;
        }


    }
}