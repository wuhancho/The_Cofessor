using System;
using UnityEngine;

/// <summary>
/// Rayo individual de la fase 2. Hace daño al player al colisionar.
/// Requiere un BoxCollider2D (Is Trigger) en el GameObject.
/// </summary>
public class CombatRay : MonoBehaviour
{
    private Combate combate;
    private int faithDamage = 1;
    private Vector2 canvasSize; // tamaño del canvas para limitar la longitud del rayo
    [SerializeField] private float rayLengthMax; // longitud máxima del rayo (distancia al borde del canvas)
    [SerializeField] private float rayLengthMin; // longitud mínima del rayo (distancia al borde del canvas)
    private RectTransform rectTransform; // referencia al RectTransform del rayo para ajustar su tamaño según el canvas
    private float distanceX;
    private float distanceY;

    // Cooldown para no hacer daño cada frame
    private float damageCooldown = 0.5f;
    private float lastDamageTime;

    public void Initialize(Combate combate, int damage)
    {
        this.combate = combate;
        canvasSize = combate.GetComponent<RectTransform>().sizeDelta;
        rayLengthMax = combate.RaylengthMax;
        rayLengthMin = combate.RaylengthMin;
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
        if (rectTransform != null)
        {
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newLength);
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

    internal void UpdateRay(float rotationSpeed)
    {
        if (rectTransform != null)
        {
            Vector2 worldPosition = rectTransform.sizeDelta;
            //float newLength = Mathf.Min(Mathf.Abs(distanceX), Mathf.Abs(distanceY));
            distanceX = canvasSize.x / 2f - Mathf.Abs(worldPosition.x);
            distanceY = canvasSize.y / 2f - Mathf.Abs(worldPosition.y);
            Debug.Log($"[CombatRay] Calculated distances - distanceX: {distanceX}, distanceY: {distanceY}, canvasSize: {canvasSize}, worldPosition: {worldPosition}");
            float newLength = Mathf.Clamp((Mathf.Abs(distanceX) - Mathf.Abs(distanceY)), rayLengthMin, rayLengthMax);
            HeightEdit(newLength);
            transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
            Debug.Log($"[CombatRay] Updated ray length to {newLength} (distanceX: {distanceX}, distanceY: {distanceY})");
            // También ajustamos el Collider en consecuencia
            BoxCollider2D col = GetComponent<BoxCollider2D>();
            if (col != null)
            {
                col.size = rectTransform.sizeDelta;
                // El offset del collider siempre debe estar en el centro real del rayo para que lo abarque completo
                col.offset = new Vector2(0f, newLength / 2f);
            }
        }
    }
}
