using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Se instancia en el centro de spPOP2 durante la fase 2.
/// Crea 8 rayos que van desde el centro hasta los límites del canvas,
/// girando en sentido horario. Se destruye al llamar a Cleanup().
/// </summary>
public class RotatingRays : MonoBehaviour
{
    [Header("Configuración de rayos")]
    [SerializeField] private int rayCount = 8;
    [SerializeField] private float rotationSpeed = 30f;    // grados por segundo (sentido horario)
    [SerializeField] private float rayWidth = 15f;          // ancho de cada rayo en píxeles
    [SerializeField] private int faithDamage = 1;           // daño por rayo al player
    [SerializeField] private Color rayColor = new Color(1f, 0.3f, 0.1f, 0.8f);

    private RectTransform rectTransform;
    private Combate combate;

    /// <summary>
    /// Inicializa los rayos giratorios.
    /// </summary>
    /// <param name="combate">Referencia al combate para daño.</param>
    /// <param name="rayLength">Longitud de cada rayo (distancia al borde más lejano del canvas).</param>
    public void Initialize(Combate combate, float rayLength)
    {
        this.combate = combate;
        rectTransform = GetComponent<RectTransform>();
        CreateRays(rayLength);
    }

    private void CreateRays(float rayLength)
    {
        float angleStep = 360f / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            // Crear el GameObject del rayo
            GameObject rayObj = new GameObject($"Ray_{i}");
            rayObj.transform.SetParent(transform, false);

            // RectTransform con pivote en la base para que rote desde el centro
            RectTransform rayRect = rayObj.AddComponent<RectTransform>();
            rayRect.pivot = new Vector2(0.5f, 0f);
            rayRect.anchoredPosition = Vector2.zero;
            rayRect.sizeDelta = new Vector2(rayWidth, rayLength);
            // Rotar cada rayo con su ángulo correspondiente
            rayRect.localRotation = Quaternion.Euler(0f, 0f, -(angleStep * i));

            // Imagen visual del rayo
            Image rayImage = rayObj.AddComponent<Image>();
            rayImage.color = rayColor;
            rayImage.raycastTarget = false;

            // Collider para detectar al player
            BoxCollider2D col = rayObj.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = rayRect.sizeDelta;
            col.offset = new Vector2(0f, rayLength / 2f); // centrar el collider a lo largo del rayo

            // Script de daño
            CombatRay combatRay = rayObj.AddComponent<CombatRay>();
            combatRay.Initialize(combate, faithDamage);
        }
    }

    private void Update()
    {
        // Girar en sentido horario (rotación negativa en Z)
        transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Destruye el objeto central y todos sus rayos.
    /// </summary>
    public void Cleanup()
    {
        Destroy(gameObject);
    }
}