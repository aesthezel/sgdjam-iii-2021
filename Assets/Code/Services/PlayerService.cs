using Code.Hero;
using Code.Interfaces;
using UnityEngine;

namespace Code.Services
{
    public class PlayerService : MonoBehaviour, IService
    {
        public PlayerController2D Controller { get; private set; }
        public PlayerReceiver Receiver { get; private set; }
        public InputMapper Mapper { get; private set; }

        private void Awake()
        {
            Controller = GetComponent<PlayerController2D>();
            Receiver = GetComponent<PlayerReceiver>();
            Mapper = GetComponent<InputMapper>();
        }
    }
}