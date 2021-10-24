using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;


public class SelectInputs : MonoBehaviour
{
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private Sprite keyboardSprite;
    [SerializeField] private Sprite gamepadSprite;
    [SerializeField] private InputAction horizontal;
    
    private Dictionary<int, (InputDevice device, Image image)> _devices = new Dictionary<int, (InputDevice, Image)>();
    private List<Image> images = new List<Image>();
    private GameObject instantiableGO;
    
    // TODO: Generar Input Actions especificos para cada dispositivo, de manera que puedan moverse a la vez
    
    
    private void Awake()
    {
        horizontal.Enable();
        CheckDevices();
        InputSystem.onDeviceChange +=
            (device, change) =>
            {
                switch (change)
                {
                    case InputDeviceChange.Added:
                        Debug.Log("Device added: " + device);
                        break;
                    case InputDeviceChange.Removed:
                        Debug.Log("Device removed: " + device);
                        break;
                    case InputDeviceChange.ConfigurationChanged:
                        Debug.Log("Device configuration changed: " + device);
                        break;
                }
            };
    }

    
    
    private void Start()
    {
        horizontal.performed += (ctx) =>
        {
            var inputValue = ctx.ReadValue<float>();
            var activeDevice = horizontal.activeControl.device;
            var image = _devices[activeDevice.deviceId].image;
            image.rectTransform.position += Vector3.right * Screen.width / 4 * inputValue;
        };
    }

    private void CheckDevices()
    {
        var allDevices = InputSystem.devices;
        
        foreach (var device in allDevices)
        {
            // We don't want to catch the mouse
            var disconnected = InputSystem.disconnectedDevices;
            
            foreach (var VARIABLE in disconnected)
            {
                Debug.Log($"Disconnected: {VARIABLE.description}");
            }
            
            if (!(device is Mouse) && device.enabled)
                CreateValidDevice(device);
        }
    }

    private void CreateValidDevice(InputDevice device)
    {
        var go = new GameObject();
        go.transform.parent = parentCanvas.transform;
        
        var image = go.AddComponent<Image>();
        image.preserveAspect = true;
        image.sprite = device is Keyboard ? keyboardSprite : gamepadSprite;
        
        var rTransform = go.GetComponent<RectTransform>();
        rTransform.position = new Vector2(Screen.width / 2f, Screen.height / 2f);
        rTransform.localScale = Vector3.one * 0.5f;
        
        //var instantiatedGO = Instantiate(go);
        
        _devices.Add(device.deviceId, (device, image));
    }
}