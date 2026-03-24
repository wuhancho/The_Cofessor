using System;
using System.Collections;
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
    [SerializeField] private int rayCount = 8;             // cantidad de rayos giratorios
    [SerializeField] private float rotationSpeed = 30f;    // grados por segundo (sentido horario)
    [SerializeField] private Color rayColor = new Color(1f, 0.3f, 0.1f, 0.8f);

    [Header("Spawn Settings Phase 3")]
    [SerializeField] private int phase3ProjectileCount = 10;    // cantidad de monedas por oleada
    [SerializeField] private float phase3SpawnRadius = 120f;    // radio alrededor de spPOP2 donde aparecen
    [SerializeField] private float phase3LaunchInterval = 0.8f; // intervalo entre lanzamiento de cada moneda
    [SerializeField] private float phase3WaveCooldown = 2f;     // cooldown entre oleadas

    // Referencia al objeto central de fase 2 para limpiarlo al cambiar de fase
    private RotatingRays phase2RaysInstance;
    private bool phase2Spawned = false;

    // Control de fase 3
    private bool phase3Running = false;
    private Coroutine phase3Coroutine;


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
        // Limpiar fase 3 al salir de ella
        if (currentPhase == CombatPhase.Phase3 && phase != CombatPhase.Phase3)
        {
            CleanupPhase3();
        }

        currentPhase = phase;
        spawnTimer = 0f; // reiniciar timer al cambiar de fase
        phase2Spawned = false;
        phase3Running = false;
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
        GameObject panfleto = Instantiate(objToSpawn1, canvasCombat);
        RectTransform objRect = panfleto.GetComponent<RectTransform>();
        objRect.anchoredPosition = spawnPos;

        // Inicializar el componente FallingObject
        FallingObject falling = panfleto.GetComponent<FallingObject>();
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

    /// <summary>
    /// Llamar desde Update de CanvasCombat para la fase 3.
    /// Lanza oleadas de monedas alrededor de spPOP2 que titilean y se dirigen al player.
    /// </summary>
    internal void UpdatePhase3Spawn()
    {
        if (phase3Running) return;
        phase3Running = true;
        phase3Coroutine = StartCoroutine(Phase3SpawnLoop());
    }

    /// <summary>
    /// Bucle de oleadas de fase 3: spawnea las monedas en círculo alrededor de spPOP2,
    /// espera un cooldown, y luego las activa una por una con intervalo.
    /// Cada moneda titilea internamente antes de lanzarse hacia el player.
    /// Se repite hasta que la fase cambie.
    /// </summary>
    private IEnumerator Phase3SpawnLoop()
    {
        if (objToSpawn3 == null || spPOP2 == null || canvasCombat == null) yield break;

        RectTransform spawnRect = spPOP2.GetComponent<RectTransform>();
        Vector2 center = spawnRect.anchoredPosition;

        while (currentPhase == CombatPhase.Phase3)
        {
            // Crear las monedas distribuidas en círculo alrededor del centro
            GameObject[] projectiles = new GameObject[phase3ProjectileCount];
            float angleStep = 360f / phase3ProjectileCount;

            for (int i = 0; i < phase3ProjectileCount; i++)
            {
                float angle = angleStep * i * Mathf.Deg2Rad;
                Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * phase3SpawnRadius;
                Vector2 spawnPos = center + offset;

                GameObject obj = Instantiate(objToSpawn3, canvasCombat);
                RectTransform objRect = obj.GetComponent<RectTransform>();
                objRect.anchoredPosition = spawnPos;

                // Inicializar el HomingProjectile
                HomingProjectile homing = obj.GetComponent<HomingProjectile>();
                if (homing == null)
                {
                    homing = obj.AddComponent<HomingProjectile>();
                }
                homing.Initialize(this, PlayerCombat,
                                  heightCanvasMax, heightCanvasMin,
                                  widthCanvasMin, widthCanvasMax);

                // Desactivar para lanzarlos uno por uno
                obj.SetActive(false);
                projectiles[i] = obj;
            }

            // Cooldown de oleada antes de empezar a lanzar
            yield return new WaitForSeconds(phase3WaveCooldown);

            // Activar (lanzar) uno por uno con intervalo
            for (int i = 0; i < projectiles.Length; i++)
            {
                if (currentPhase != CombatPhase.Phase3) yield break;

                if (projectiles[i] != null)
                {
                    projectiles[i].SetActive(true);
                    Debug.Log($"Phase 3 - Moneda {i + 1}/{phase3ProjectileCount} lanzada");
                }
                yield return new WaitForSeconds(phase3LaunchInterval);
            }

            // Pausa antes de la siguiente oleada
            yield return new WaitForSeconds(phase3WaveCooldown);
        }
    }

    /// <summary>
    /// Limpia la corrutina de fase 3.
    /// </summary>
    private void CleanupPhase3()
    {
        if (phase3Coroutine != null)
        {
            StopCoroutine(phase3Coroutine);
            phase3Coroutine = null;
        }
        phase3Running = false;
    }
}
