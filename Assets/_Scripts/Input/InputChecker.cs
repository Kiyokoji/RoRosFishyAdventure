using System.Linq;
using UnityEngine;
public class InputChecker : MonoBehaviour
{
    public enum InputDevice { controller = 0, keyboard = 1 };
    public InputDevice inputDevice;
    public string nameDevice;
    private PlayerInputActions inputActions;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }
    private void Update()
    {

    }
}