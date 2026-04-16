using UnityEngine;

/// <summary>
/// Rayo individual de la fase 2. Hace daño al player al colisionar.
/// Requiere un BoxCollider2D (Is Trigger) en el GameObject.
/// </summary>
public class CombatRay : MonoBehaviour
{
    private Combate combate;
    private int faithDamage = 1;
    private float canvasHeightMax; // altura del canvas para limitar el tamaño del rayo
    private float canvasHeightMin; // altura del canvas para limitar el tamaño del rayo
    private float canvasWidthMax; // ancho del canvas para limitar el tamaño del rayo
    private float canvasWidthMin; // ancho del canvas para limitar el tamaño del rayo
    private RectTransform rectTransform; // referencia al RectTransform del rayo para ajustar su tamaño según el canvas

    // Cooldown para no hacer daño cada frame
    private float damageCooldown = 0.5f;
    private float lastDamageTime;

    public void Initialize(Combate combate, int damage)
    {
        this.combate = combate;
        canvasHeightMax = combate.HeightCanvasMax;
        canvasHeightMin = combate.HeightCanvasMin;
        canvasWidthMax = combate.WidthCanvasMax;
        canvasWidthMin = combate.WidthCanvasMin;
        this.faithDamage = damage;
        rectTransform = GetComponent<RectTransform>();
        lastDamageTime = -damageCooldown; // permitir daño inmediato
    }

    private void Update()
    {
        //limitar el tamaño del rayo para que no se extienda más allá del canvas
    }

    public void HeightEdit(float newLength)
    {
        RectTransform rt = GetComponent<RectTransform>();
        if (rt != null)
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, newLength);
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
