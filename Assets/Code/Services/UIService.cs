using System;
using Code.Interfaces;
using Code.UI;
using UnityEngine;

namespace Code.Services
{
    public class UIService : MonoBehaviour, IService
    {
        public ShowButtonsOnUI ButtonDisplayer { get; private set; }

        private void Awake()
        {
            ButtonDisplayer = GetComponentsInChildren<ShowButtonsOnUI>()[0];
        }
    }
}