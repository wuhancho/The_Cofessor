using UnityEngine;

public static class InputSystemStaticProvider
{
    private static InputSystem_Actions inputSystem = new InputSystem_Actions();
    public static InputSystem_Actions InputSystem => inputSystem;

    public static void EnableInputSystem()
    {
        inputSystem.Enable();
    }

    public static void DisableInputSystem()
    {
        inputSystem.Disable();
    }
}
