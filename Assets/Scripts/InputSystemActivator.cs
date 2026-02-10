using UnityEngine;

public class InputSystemActivator : MonoBehaviour
{
    private void OnEnable()
    {
        InputSystemStaticProvider.EnableInputSystem();
    }

    private void OnDisable()
    {
        InputSystemStaticProvider.DisableInputSystem();
    }
}
