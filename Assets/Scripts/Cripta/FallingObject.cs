using UnityEngine;

/// <summary>
/// Objeto que cae en zigzag como una hoja dentro del canvas de combate.
/// Se destruye al salir del límite inferior o al chocar con el player.
/// </summary>
public class FallingObject : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 150f;       // velocidad de caída en píxeles/s
    [SerializeField] private float zigzagAmplitude = 60f;  // amplitud horizontal del zigzag
    [SerializeField] private float zigzagFrequency = 2f;   // frecuencia del zigzag
    [SerializeField] private int faithDamage = 1;          // daño de fe al impactar

    private RectTransform rectTransform;
    private float bottomLimit;
    private float timeAlive;
    private float startX;
    private Combate combate;
    private float canvasWidthMax;
    private float canvasWidthMin;
    private float offsetX;
    /// <summary>
    /// Inicializa el objeto con sus parámetros de combate.
    /// </summary>
    /// <param name="combate">Referencia al script de combate para aplicar daño.</param>
    /// <param name="bottomY">Límite inferior del canvas (posición Y mínima).</param>
    public void Initialize(Combate combate)
    {
        this.combate = combate;
        this.bottomLimit = -150f;
        this.canvasWidthMax = combate.SpawnAreaMaxX;
        this.canvasWidthMin = combate.SpawnAreaMinX;
        offsetX = combate.SpawnAreaXOffset;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        startX = rectTransform.anchoredPosition.x;
        timeAlive = 0f;
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;

        // Movimiento zigzag: baja en Y y oscila en X con seno
        float newY = rectTransform.anchoredPosition.y - fallSpeed * Time.deltaTime;
        float newX = startX + Mathf.Sin(timeAlive * zigzagFrequency) * zigzagAmplitude;
        // Asegurar que el nuevo X no salga de los límites del canvas
        newX = Mathf.Clamp(newX, canvasWidthMin + offsetX, canvasWidthMax - offsetX);
        rectTransform.anchoredPosition = new Vector2(newX, newY);

        // Destruir si llega al límite inferior del canvas
        if (newY <= bottomLimit)
        {
            Debug.Log($"falling object: bottonLimit {bottomLimit}  newY {newY}");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si colisiona con el player de combate
        if (other.CompareTag("Player"))
        {
            combate.TakeFaith(faithDamage);
            Destroy(gameObject);
        }
    }
}