using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Combate : MonoBehaviour
{
    [SerializeField,IReadOnly]private PlayerController playerController;
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private float velocityPlayer = 5f;
    [SerializeField] private float heightCanvasMax;
    [SerializeField] private float widthCanvasMax;
    [SerializeField] private float widthCanvasMin;
    [SerializeField] private float heightCanvasMin;
    [SerializeField] private RectTransform canvasCombat;
    [SerializeField] private RectTransform PlayerCombat;
    [SerializeField] private CombatPhase currentPhase;
    [SerializeField,IReadOnly]private Vector2 spawnPlayer;
    [Header("SpawnPoints")]
    [SerializeField] private GameObject spPOP1; // spawnPointObjPhase1 - indica la zona de spawn de objetos del boss en la fase 1
    private float spawnWidth;
    [SerializeField] private GameObject spPOP2; // spawnPointObjPhase2 - indica la zona de spawn de objetos del boss en la fase 2
    [Header("object prefabs for spawning")]
    [SerializeField] private GameObject objToSpawn1; // objeto a spawnear en la fase 1
    [SerializeField] private GameObject objToSpawn2; // objeto a spawnear en la fase 2
    [SerializeField] private GameObject objToSpawn3; // objeto a spawnear en la fase 3

    [Header("Spawn Settings Phase 1")]
    [SerializeField] private float spawnInterval = 1.5f;   // segundos entre cada spawn
    [SerializeField] private int faithDamagePerHit = 1;     // daño de fe por impacto
    private float spawnTimer;

    [Header("Spawn Settings Phase 2")]
    [SerializeField] private float rayRotationSpeed = 30f;  // grados/segundo de los rayos
    [SerializeField] private float rayWidth = 15f;           // ancho de cada rayo
    [SerializeField] private int rayDamage = 1;              // daño por rayo

    // Referencia al objeto central de fase 2 para limpiarlo al cambiar de fase
    private RotatingRays phase2RaysInstance;
    private bool phase2Spawned = false;

    public Action onStop;

    public CombatPhase CurrentPhase { get => currentPhase; set => currentPhase = value; }
    public GameObject ObjToSpawn1 { get => objToSpawn1; set => objToSpawn1 = value; }
    public GameObject ObjToSpawn2 { get => objToSpawn2; set => objToSpawn2 = value; }
    public GameObject ObjToSpawn3 { get => objToSpawn3; set => objToSpawn3 = value; }
    public GameObject SpPOP1 { get => spPOP1; set => spPOP1 = value; }
    public GameObject SpPOP2 { get => spPOP2; set => spPOP2 = value; }

    public void Initialize(PlayerController controller)
    {
        playerController = controller;
        spawnPlayer = PlayerCombat.anchoredPosition;
        onStop += StopAll;
    }
    public void MovePlayer()
    {
        Vector2 input = InputSystemStaticProvider.InputSystem.Player.Move.ReadValue<Vector2>();
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

    internal void SetPhase(CombatPhase phase)
    {
        // Limpiar fase 2 al salir de ella
        if (currentPhase == CombatPhase.Phase2 && phase != CombatPhase.Phase2)
        {
            CleanupPhase2();
        }

        currentPhase = phase;
        spawnTimer = 0f; // reiniciar timer al cambiar de fase
        phase2Spawned = false;
    }

    public float GetSpawnWidth()
    {
        if (currentPhase == CombatPhase.Phase1)
        {
            spawnWidth = spPOP1.GetComponent<RectTransform>().rect.width;
        }
        else if (currentPhase == CombatPhase.Phase2)
        {
            spawnWidth = spPOP2.GetComponent<RectTransform>().rect.width;
        }
        return spawnWidth;
    }
    public Vector2 GetSpawnPosition()
    {
        Vector2 spawnPosition = Vector2.zero;
        if (currentPhase == CombatPhase.Phase1)
        {
            spawnPosition = spPOP1.GetComponent<RectTransform>().anchoredPosition;
        }
        else if (currentPhase == CombatPhase.Phase2)
        {
            spawnPosition = spPOP2.GetComponent<RectTransform>().anchoredPosition;
        }
        return spawnPosition;
    }

    /// <summary>
    /// Llamar desde Update de CanvasCombat para spawnear objetos en la fase 1.
    /// Genera objetos a lo largo del ancho de spPOP1 que caen en zigzag.
    /// </summary>
    public void UpdatePhase1Spawn()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            spawnTimer = 0f;
            SpawnFallingObject();
        }
    }

    private void SpawnFallingObject()
    {
        if (objToSpawn1 == null || spPOP1 == null || canvasCombat == null) return;

        RectTransform spawnRect = spPOP1.GetComponent<RectTransform>();
        Vector2 spawnCenter = spawnRect.anchoredPosition;
        float halfWidth = spawnRect.rect.width / 2f;

        // Posición X aleatoria a lo largo del ancho del spawn point
        float randomX = UnityEngine.Random.Range(spawnCenter.x - halfWidth, spawnCenter.x + halfWidth);
        Vector2 spawnPos = new Vector2(randomX, spawnCenter.y);

        // Instanciar como hijo del canvas de combate
        GameObject obj = Instantiate(objToSpawn1, canvasCombat);
        RectTransform objRect = obj.GetComponent<RectTransform>();
        objRect.anchoredPosition = spawnPos;

        // Inicializar el componente FallingObject
        FallingObject falling = obj.GetComponent<FallingObject>();
        if (falling != null)
        {
            // El límite inferior es el borde bajo del canvas de combate
            falling.Initialize(this);
        }
    }

    /// <summary>
    /// Llamar desde Update de CanvasCombat para la fase 2.
    /// Spawnea objToSpawn2 en el centro de spPOP2 una sola vez y crea los rayos giratorios.
    /// </summary>
    internal void UpdatePhase2Spawn()
    {
        if (phase2Spawned) return;
        phase2Spawned = true;
        SpawnRotatingRays();
    }

    private void SpawnRotatingRays()
    {
        if (objToSpawn2 == null || spPOP2 == null || canvasCombat == null) return;

        RectTransform spawnRect = spPOP2.GetComponent<RectTransform>();
        Vector2 spawnCenter = spawnRect.anchoredPosition;

        // Instanciar objToSpawn2 en el centro de spPOP2
        GameObject centerObj = Instantiate(objToSpawn2, canvasCombat);
        RectTransform centerRect = centerObj.GetComponent<RectTransform>();
        centerRect.anchoredPosition = spawnCenter;

        // Calcular la longitud de los rayos: distancia máxima desde el centro hasta cualquier borde del canvas
        float distToRight = Mathf.Abs(widthCanvasMax - spawnCenter.x);
        float distToLeft = Mathf.Abs(spawnCenter.x - widthCanvasMin);
        float distToTop = Mathf.Abs(heightCanvasMax - spawnCenter.y);
        float distToBottom = Mathf.Abs(spawnCenter.y - heightCanvasMin);
        float rayLength = Mathf.Max(distToRight, distToLeft, distToTop, distToBottom);

        // Añadir el componente RotatingRays si no lo tiene el prefab
        RotatingRays rotating = centerObj.GetComponent<RotatingRays>();
        if (rotating == null)
        {
            rotating = centerObj.AddComponent<RotatingRays>();
        }
        rotating.Initialize(this, rayLength);

        phase2RaysInstance = rotating;
        Debug.Log($"Phase 2 - Rayos giratorios creados en {spawnCenter} con longitud {rayLength}");
    }

    /// <summary>
    /// Limpia los objetos de la fase 2 (objeto central + rayos).
    /// </summary>
    private void CleanupPhase2()
    {
        if (phase2RaysInstance != null)
        {
            phase2RaysInstance.Cleanup();
            phase2RaysInstance = null;
        }
        phase2Spawned = false;
    }

    internal void UpdatePhase3Spawn()
    {
        Debug.Log("Phase 3 - Spawning objects not implemented yet.");
    }

    private void StopAll()
    {
        
    }
}
