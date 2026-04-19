using UnityEngine;

/// <summary>
/// Rayo individual de la fase 2. Hace daño al player al colisionar.
/// Requiere un BoxCollider2D (Is Trigger) en el GameObject.
/// </summary>
public class CombatRay : MonoBehaviour
{
    private Combate combate;
    private int faithDamage = 1;
    [SerializeField] private float rayLengthMax; // longitud máxima del rayo (distancia al borde del canvas)
    [SerializeField] private float rayLengthMin; // longitud mínima del rayo (distancia al borde del canvas)
    private RectTransform rectTransform; // referencia al RectTransform del rayo para ajustar su tamaño según el canvas

    // Cooldown para no hacer daño cada frame
    private float damageCooldown = 0.5f;
    private float lastDamageTime;

    public void Initialize(Combate combate, int damage)
    {
        this.combate = combate;
        rayLengthMax = combate.RaylengthMax; // ¡Asegúrate de tener este campo en Combate!
        rayLengthMin = combate.RaylengthMin; // ¡Asegúrate de tener este campo en Combate!
        this.faithDamage = damage;
        rectTransform = GetComponent<RectTransform>();
        lastDamageTime = -damageCooldown; // permitir daño inmediato
    }

    public void HeightEdit(float newLength)
    {
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newLength);
        }
    }

    internal void UpdateRay(float rotationSpeed)
    {
        Debug.Log($"[CombatRay] Name: {gameObject.name} Updating ray. Rotation speed: {rotationSpeed}");
        if (rectTransform != null && combate != null)
        {
            // 2. MAGIA AQUÍ: transform.up es el vector de dirección real del rayo
            Vector2 dir = transform.up;

            // 3. Obtenemos desde dónde nacen los rayos dentro del Canvas
            Vector2 origin = combate.SpPOP2.GetComponent<RectTransform>().anchoredPosition;
            Debug.Log($"[CombatRay] Origin: {origin}, Direction: {dir}");

            // 4. Calculamos cuánto espacio hay hasta la pared en X
            float distanceX = float.MaxValue;
            if (dir.x > 0.001f) // Lo que sea que apunte hacia la Derecha
            {
                distanceX = (combate.WidthCanvasMaxToRay - origin.x) / dir.x;
                Debug.Log($"[CombatRay] Distance to right wall: {distanceX}");
            }
            else if (dir.x < -0.001f) // Lo que sea que apunte hacia la Izquierda
            {
                distanceX = (combate.WidthCanvasMinToRay - origin.x) / dir.x;
                Debug.Log($"[CombatRay] Distance to left wall: {distanceX}");
            }

            // 5. Calculamos cuánto espacio hay hasta la pared en Y
            float distanceY = float.MaxValue;
            if (dir.y > 0.001f) // Lo que sea que apunte hacia Arriba
            {
                distanceY = (combate.HeightCanvasMaxToRay - origin.y) / dir.y;
                Debug.Log($"[CombatRay] Distance to top wall: {distanceY}");
            }
            else if (dir.y < -0.001f) // Lo que sea que apunte hacia Abajo
            {
                distanceY = (combate.HeightCanvasMinToRay - origin.y) / dir.y;
                Debug.Log($"[CombatRay] Distance to bottom wall: {distanceY}");
            }

            Debug.Log($"[CombatRay] DistanceX: {distanceX}, DistanceY: {distanceY}");
            // 6. El tamaño final será chocar contra la pared más cercana
            float newLength = Mathf.Min(Mathf.Abs(distanceX), Mathf.Abs(distanceY));
            Debug.Log($"[CombatRay] New length: {newLength}");

            // Limitamos que el rayo no se encoja o alargue de los límites (opcional)
            newLength = Mathf.Clamp(newLength, rayLengthMin, rayLengthMax);
            Debug.Log($"[CombatRay] Clamped length: {newLength}");
            // 7. Modificamos visualmente
            HeightEdit(newLength);

            // 8. Ajustamos el Collider de colisión para que cubra la nueva distancia exacta
            BoxCollider2D col = GetComponent<BoxCollider2D>();
            if (col != null)
            {
                col.size = rectTransform.sizeDelta;
                // Offset al centro (como pivot es 0.5, 0 -> el centro del Box es la mitad de la altura total)
                col.offset = new Vector2(0f, newLength / 2f);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                lastDamageTime = Time.time;
                combate.TakeFaith(faithDamage);
            }
        }
    }
}
