using UnityEngine;
using UnityEngine.InputSystem;

public class Combate : MonoBehaviour
{
    private PlayerController playerController;
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float velocityPlayer = 5f;
    [SerializeField] private float heightCanvasMax;
    [SerializeField] private float widthCanvasMax;
    [SerializeField] private float widthCanvasMin;
    [SerializeField] private float heightCanvasMin;
    [SerializeField] private RectTransform canvasCombat;
    [SerializeField] private RectTransform PlayerCombat;
    [SerializeField] private CombatPhase currentPhase;
    [Header("SpawnPoints")]
    [SerializeField] private GameObject spPOP1; // spawnPointObjPhase1 - indica la zona de spawn de objetos del boss en la fase 1
    [SerializeField] private GameObject spPOP2; // spawnPointObjPhase2 - indica la zona de spawn de objetos del boss en la fase 2


    public void Initialize(PlayerController controller)
    {
        playerController = controller;
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        Vector2 currentPos = PlayerCombat.anchoredPosition;
        Vector2 newPos = currentPos + position * velocityPlayer * Time.deltaTime;
        newPos = ClampPositionInArea(newPos);
        PlayerCombat.anchoredPosition = newPos;
        //Debug.Log($"position of player in combate: {newPos}");

    }

    private Vector2 ClampPositionInArea(Vector2 position)
    {
        //logica para que no salga de los bordes
        position.x = Mathf.Clamp(position.x, widthCanvasMin, widthCanvasMax);
        position.y = Mathf.Clamp(position.y, heightCanvasMin, heightCanvasMax);
        return position;
    }
    public void TakeFaith(int damage)
    {
        playerController.PlayerStatus.DecreaseFaith(damage);
    }

}
