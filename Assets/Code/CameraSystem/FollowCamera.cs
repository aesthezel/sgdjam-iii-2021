using System;
using Code.Hero;
using UnityEngine;

namespace Code.CameraSystem
{
    public class FollowCamera : ICameraItem
    {
        [SerializeField] private Transform target; 
        [SerializeField] private float ortographicSize;
        [SerializeField] private float followVelocity;
        [SerializeField] private float groundedY;
        [SerializeField] private float movingX;
        [SerializeField] private float standX;
        [SerializeField] private float xDeadZone;
        [SerializeField] private float yDeadZone;
        
        private PlayerController2D controller;
        public Vector2 Tuning { get; set; }

        private void Awake()
        {
            mycamera = GetComponent<Camera>();
            controller = target.GetComponent<PlayerController2D>();
        }

        private void Start()
        {
            mycamera.orthographicSize = ortographicSize;
        }
        
        private void Update()
        {
            FollowTarget();
        }

        private void OnEnable()
        {
            var pos = target.position;
            pos.z = transform.position.z;
            transform.position = pos;
        }

        public void TuneDisplacement(Vector2 newTune) => Tuning = newTune;


        private void FollowTarget()
        {
            var destiny = target.position;
            
            //if (Math.Abs(destiny.y - transform.position.y) < yDeadZone)
            //    destiny.y = transform.position.y;

            // EN Y
            destiny.y += groundedY;

            // EN X
            if (Math.Abs(destiny.x - transform.position.x) > xDeadZone)
            {
                var velDir = Mathf.Sign(controller.CurrentVelocity.x);
                if (!controller.Dashing)
                {
                    if (Mathf.Abs(controller.CurrentVelocity.x) > 0)
                        destiny.x += (movingX * Mathf.Abs(controller.CurrentVelocity.x)) * velDir;
                    else
                        destiny.x += standX * velDir * controller.FacingDirection;
                }
            }
            else
            {
                destiny.x = transform.position.x;
            }

            // Tuneado
            destiny += new Vector3(Tuning.x, Tuning.y , 0);
            
            // Lerp la posicion
            Vector3 newPos;
            var yVel = controller.CurrentVelocity.y;
            
            // Si le siguieramos muy lento se sale de camara
            if (yVel < -20)
                newPos = Vector3.Lerp(transform.position, destiny, Time.deltaTime * yVel * -1);

            else
                newPos = Vector3.Lerp(transform.position, destiny, Time.deltaTime * followVelocity);
    
            newPos.z = transform.position.z;
            transform.position = newPos;
        }

        private void OnDrawGizmos()
        {
            var color = Color.cyan;
            color.a = 0.2f;
            Gizmos.color = color;
            
            Gizmos.DrawCube(transform.position, new Vector3(xDeadZone * 2, yDeadZone * 2));
        }
    }
}