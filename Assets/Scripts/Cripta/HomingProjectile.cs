using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Proyectil de fase 3: aparece alrededor del centro, titilea oscureciéndose
/// durante un cooldown, y luego se lanza en dirección al player.
/// Se destruye al colisionar con el player o al salir del canvas.
/// </summary>
public class HomingProjectile : MonoBehaviour
{
    [SerializeField] private float projectileSpeed = 300f;   // velocidad en píxeles/s
    [SerializeField] private float flickerDuration = 1.5f;   // duración del titileo antes de lanzarse
    [SerializeField] private float flickerInterval = 0.1f;   // intervalo entre cambios de color del titileo
    [SerializeField] private int faithDamage = 1;             // daño de fe al impactar
    [SerializeField] private Color darkColor = new Color(0.3f, 0.1f, 0.1f, 1f); // color oscuro del titileo

    private RectTransform rectTransform;
    private Image image;
    private Color originalColor;
    private Combate combate;
    private RectTransform targetPlayer;

    private Vector2 moveDirection;
    private bool launched = false;

    // Límites del canvas para destruirse al salir
    private float limitTop;
    private float limitBottom;
    private float limitLeft;
    private float limitRight;

    /// <summary>
    /// Inicializa el proyectil.
    /// </summary>
    /// <param name="combate">Referencia al combate para aplicar daño.</param>
    /// <param name="playerRect">RectTransform del player para calcular dirección.</param>
    /// <param name="top">Límite superior del canvas.</param>
    /// <param name="bottom">Límite inferior del canvas.</param>
    /// <param name="left">Límite izquierdo del canvas.</param>
    /// <param name="right">Límite derecho del canvas.</param>
    public void Initialize(Combate combate, RectTransform playerRect,
                           float top, float bottom, float left, float right)
    {
        this.combate = combate;
        targetPlayer = playerRect;
        limitTop = top;
        limitBottom = bottom;
        limitLeft = left;
        limitRight = right;
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        if (image != null)
        {
            originalColor = image.color;
        }
        StartCoroutine(FlickerThenLaunch());
    }

    /// <summary>
    /// Titilea cambiando entre color original y oscuro durante el cooldown,
    /// luego calcula la dirección hacia el player y se lanza.
    /// </summary>
    private IEnumerator FlickerThenLaunch()
    {
        // Fase de titileo (cooldown antes de lanzarse)
        float elapsed = 0f;
        bool isDark = false;
        while (elapsed < flickerDuration)
        {
            isDark = !isDark;
            if (image != null)
            {
                image.color = isDark ? darkColor : originalColor;
            }
            yield return new WaitForSeconds(flickerInterval);
            elapsed += flickerInterval;
        }

        // Restaurar color original al lanzarse
        if (image != null)
        {
            image.color = originalColor;
        }

        // Calcular dirección hacia la posición actual del player
        if (targetPlayer != null)
        {
            // Restamos Posición Llegada (Player) - Posición Inicio (Moneda)
            // Luego lo normalizamos para sacar exclusivamente la dirección (magnitud 1)
            moveDirection = targetPlayer.anchoredPosition - rectTransform.anchoredPosition;
            Debug.Log($"[HomingProjectile] Calculated move direction: {moveDirection} towards player at {targetPlayer.anchoredPosition}, from position {rectTransform.anchoredPosition}");
        }
        else
        {
            moveDirection = Vector2.down; // fallback
        }

        launched = true;
    }

    private void Update()
    {
        if (!launched) return;

        rectTransform.anchoredPosition += moveDirection * projectileSpeed * Time.deltaTime;                                                              

        // Destruir si sale de los límites del canvas
        if (rectTransform.anchoredPosition.x < limitLeft || rectTransform.anchoredPosition.x > limitRight ||
            rectTransform.anchoredPosition.y < limitBottom || rectTransform.anchoredPosition.y > limitTop)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"[HomingProjectile] Hit player, applying {faithDamage} faith damage.");
            combate.TakeFaith(faithDamage);
            Destroy(gameObject);
        }
    }
}