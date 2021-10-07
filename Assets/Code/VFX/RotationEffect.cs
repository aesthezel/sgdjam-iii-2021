using System;
using UnityEngine;

namespace Code.VFX
{
    public class RotationEffect : MonoBehaviour
    {

        [Header("Rotate by degrees per seconds")]
        public Vector3 rotateVector = Vector3.zero;

        public enum SpaceEnum { Local, World };
        public SpaceEnum rotateSpace;
        
        void Update()
        {
            switch (rotateSpace)
            {
                case SpaceEnum.Local:
                    transform.Rotate(rotateVector * Time.deltaTime);
                    break;
                case SpaceEnum.World:
                    transform.Rotate(rotateVector * Time.deltaTime, Space.World);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}