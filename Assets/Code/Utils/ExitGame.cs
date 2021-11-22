using UnityEngine;
using UnityEngine.InputSystem;

public class ExitGame : MonoBehaviour
{
    [SerializeField] private InputAction exitButton;

    private void Awake()
    {
        exitButton.Enable();
        exitButton.performed += (_) => Quit();
    }

    public void Quit() => Application.Quit();
    
}
