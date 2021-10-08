using System;
using Code.Hero;
using Unity.Mathematics;
using UnityEngine;

namespace Code.CameraSystem
{
    public class FollowCamera : ICameraItem
    {
        // Que se quede dentro de un collider
        // Sacar esquinas > lanzar rayo > calcular tope
        // Seguir al player

        [SerializeField] private Transform target; // SERVICELOCATOR 
        [SerializeField] private float ortographicSize;
        [SerializeField] private BoxCollider2D limits;
        [SerializeField] private float followVelocity;
        [SerializeField] private float groundedY;
        [SerializeField] private float jumpingY;
        [SerializeField] private float movingX;
        [SerializeField] private float standX;
        
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
            // EN Y
            if (controller.CurrentVelocity.y > 0 || controller.Grounded)
                destiny.y += groundedY;
           // if (controller.CurrentVelocity.y < -20f)
            //    destiny.y -= jumpingY;
            
            // EN X
            if (Mathf.Abs(controller.CurrentVelocity.x) > 0)
                destiny.x += (movingX * Mathf.Abs(controller.CurrentVelocity.x)) * controller.FacingDirection;
            else
                destiny.x += standX * controller.FacingDirection;
        
            // Tuneado
            destiny += new Vector3(Tuning.x, Tuning.y , 0);
            
            // Lerp la posicion
            var newPos = Vector3.Lerp(transform.position, destiny, Time.deltaTime * followVelocity);
            newPos.z = transform.position.z;
            

            transform.position = newPos;
        }
    }
}