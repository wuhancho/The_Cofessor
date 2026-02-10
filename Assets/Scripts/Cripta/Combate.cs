using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class Combate : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float velocityPlayer = 5f;
    [SerializeField] private float heightCanvas;
    [SerializeField] private float widthCanvas;
    [SerializeField] private RectTransform CanvasCombat;
    [SerializeField] private RectTransform PlayerCombat;
    [SerializeField] private CombatPhase currentPhase;


    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        heightCanvas = CanvasCombat.rect.height;
        widthCanvas = CanvasCombat.rect.width;

    }
    private void Update()
    {
        Vector2 input = InputSystemStaticProvider.InputSystem.Player.Move.ReadValue<Vector2>();

        MoveLogic(input);
    }

    private void MoveInput(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        MoveLogic(input);
    }

    private void MoveLogic(Vector2 position)
    {
        position = ClampPositionInArea(position);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        //positionMouse = Input.mousePosition;
        //positionMouse.x = Mathf.Clamp(positionMouse.x, 0, widthCanvas);
        //positionMouse.y = Mathf.Clamp(positionMouse.y, 0, heightCanvas);
        PlayerCombat.anchoredPosition = position;

    }

    private Vector2 ClampPositionInArea(Vector2 position)
    {
        //logica para que no salga de los bordes
        position.x = Mathf.Clamp(position.x, 0, widthCanvas);
        position.y = Mathf.Clamp(position.y, 0, heightCanvas);
        return position;
    }

    //private void OnEnable()
    //{
    //    InputSystemStaticProvider.InputSystem.Player.Move.performed += MoveInput;
    //}

    //private void OnDisable()
    //{
    //    InputSystemStaticProvider.InputSystem.Player.Move.performed -= MoveInput;
    //}
}
