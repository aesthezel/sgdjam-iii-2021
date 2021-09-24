using Code.Camera;
using Code.Data;
using Code.Hero;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Assertions;

namespace Code.Utils
{
    [RequireComponent(typeof(PlayerController))]
    public class InputEffectsSubscriber: MonoBehaviour
    {
        [BoxGroup("CAMERA EFFECTS")]
        [SerializeField] private CameraEffects effects;

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
        
        private PlayerController _controller;
        private InputMapper _mapper;

        private void Awake()
        {
            _controller = GetComponent<PlayerController>();
            _mapper = GetComponent<InputMapper>();
        }

        private void Start()
        {
            JumpEffects();
            DashEffects();
        }

        private void JumpEffects()
        {
            var done = _mapper.ActionMapper.TryGetValue("Jump", out var actions);
            Assert.IsTrue(done, "Error retrieving <Jump> input");

            actions.start += f => effects.DoOrtoSize(startTargetOrtoSize.Value, f/3, Ease.InQuad);
            actions.finished += () => effects.DoOrtoSize(normalOrtoSize.Value, endZoomOutTime.Value, Ease.OutQuad);
        }

        private void DashEffects()
        {
            var done = _mapper.ActionMapper.TryGetValue("Dash", out var actions);
            Assert.IsTrue(done, "Error retrieving <Dash> input");

            actions.ok += f => effects.DoLookAhead(
                lookAheadDistance.Value,
                _controller.FacingRight ? 1 : -1,
                sustainTime.Value,
                inTime.Value,
                outTime.Value);
        }
    }
}