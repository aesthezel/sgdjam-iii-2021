using System;
using Code.Camera;
using Code.Hero;
using UnityEngine;
using UnityEngine.Assertions;

namespace Code.Utils
{
    public class InputEffectsSubscriber: MonoBehaviour
    {
        [SerializeField] private CameraEffects effects;
        [SerializeField] private PlayerController controller;
        [SerializeField] private InputMapper mapper;
        
        private void Start()
        {
            JumpEffects();
        }


        private void JumpEffects()
        {
            var done = mapper.ActionMapper.TryGetValue("Jump", out var actions);
            Assert.IsTrue(done, "Error retrieving <Jump> input");

            actions.start += f => effects.DoOrtoSize(9.5f, f/3, Ease.InQuad);
            actions.finished += () => effects.DoOrtoSize(10f, 0.2f, Ease.OutQuad);
        }

        private void DashEffects()
        {
            var done = mapper.ActionMapper.TryGetValue("Dash", out var actions);
            Assert.IsTrue(done, "Error retrieving <Dash> input");

            actions.ok += f => effects.DoLookAhead(4, controller.FacingRight ? 1 : -1, 0.5f, 0.1f);
        }
        
        
        
    }
}