using System.Collections.Generic;
using UnityEngine;

namespace Code.Hero
{
    public struct ColliderOrigins
    {
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }

    public struct PlayerCollisions
    {
        public bool leftCollision;
        public bool rightCollision;
        public bool topCollision;
        public bool groundCollision;

        public void ResetFaceCollisions() => leftCollision = rightCollision = false;
    }

    public class MultiRayInfo
    {
        public List<bool> hasHit = new List<bool>();
        public List<float> hitDistances = new List<float>();

        public void AddRayInfo(bool hit, float distance)
        {
            hasHit.Add(hit);
            hitDistances.Add(distance);
        }
    }
}