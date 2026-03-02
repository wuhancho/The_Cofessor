using UnityEngine;

/// <summary>
/// Rayo individual de la fase 2. Hace daño al player al colisionar.
/// Requiere un BoxCollider2D (Is Trigger) en el GameObject.
/// </summary>
public class CombatRay : MonoBehaviour
{
    private Combate combate;
    private int faithDamage = 1;

    // Cooldown para no hacer daño cada frame
    private float damageCooldown = 0.5f;
    private float lastDamageTime;

    public void Initialize(Combate combate, int damage)
    {
        this.combate = combate;
        this.faithDamage = damage;
        lastDamageTime = -damageCooldown; // permitir daño inmediato
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
