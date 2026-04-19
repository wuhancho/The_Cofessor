using System.Collections.Generic;
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
    [SerializeField] private int faithDamage = 1;           // daño por rayo al player
    [SerializeField] private GameObject spawnToRays;        // referencia al spawnpoint para calcular el centro para rotar los rayos
    private GameObject spawnpoint;          // referencia al spawnpoint para calcular la longitud de los rayos
    private RectTransform rayRectObj;         // referencia al RectTransform del rayo para ajustar su tamaño según el canvas
    private float angleStep;              // ángulo entre cada rayo (360 / rayCount)
    private List<GameObject> rays;                   // array para almacenar referencias a los rayos creados

    [Header("Apariencia")]
    [SerializeField] private float rayWidth = 15f;          // ancho de cada rayo en píxeles
    [SerializeField] private float rayLength = 500f;       // longitud de cada rayo (distancia al borde del canvas)
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
        rays = new List<GameObject>();
        this.rayLength = rayLength;
        spawnpoint = combate.SpPOP2; // Obtener la referencia al spawnpoint desde el combate
        rectTransform = GetComponent<RectTransform>();
        CreateRays(rayLength);
    }

    private void CreateRays(float rayLength)
    {
        angleStep = 360f / rayCount;

        for (int i = 0; i < rayCount; i++)
        {
            // Crear el GameObject del rayo
            GameObject rayObj = new GameObject($"Ray_{i}");

            rayObj.transform.SetParent(spawnToRays.transform, false);

            // RectTransform con pivote en la base para que rote desde el centro
            rayRectObj = rayObj.AddComponent<RectTransform>();
            rayRectObj.pivot = new Vector2(0.5f, 0f);
            rayRectObj.anchoredPosition = Vector2.zero;
            rayRectObj.sizeDelta = new Vector2(rayWidth, rayLength);
            // Rotar cada rayo con su ángulo correspondiente
            rayRectObj.localRotation = Quaternion.Euler(0f, 0f, -(angleStep * i));

            // Imagen visual del rayo
            Image rayImage = rayObj.AddComponent<Image>();
            rayImage.color = rayColor;
            rayImage.raycastTarget = false;

            // Collider para detectar al player
            BoxCollider2D col = rayObj.AddComponent<BoxCollider2D>();
            col.isTrigger = true;
            col.size = rayRectObj.sizeDelta;
            col.offset = new Vector2(0f, rayLength / 2f); // centrar el collider a lo largo del rayo

            // Script de daño
            CombatRay combatRay = rayObj.AddComponent<CombatRay>();
            AddRays(rayObj); // Agregar el rayo a la lista para gestión
            combatRay.Initialize(combate, faithDamage);
        }
    }

    private void Update()
    {
        // Girar en sentido horario (rotación negativa en Z)
        //spawnToRays.transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
        foreach (GameObject ray in rays)
        {
            if (ray != null)
            {
                //ray.transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
                ray.GetComponent<CombatRay>().UpdateRay(rotationSpeed);
                ray.transform.Rotate(0f, 0f, -rotationSpeed * Time.deltaTime);
            }
        }

    }

    /// <summary>
    /// Destruye el objeto central y todos sus rayos.
    /// </summary>
    public void Cleanup()
    {
        Destroy(gameObject);
    }

    private void AddRays(GameObject rayObj)
    {
        rays.Add(rayObj);
    }
    private void RemoveRays(GameObject rayObj)
    {
        rays.Remove(rayObj);
    }
}