using System;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmDecision : MonoBehaviour
{
    [SerializeField] private GameObject confirmDecision;
    [SerializeField] private GameObject cancelDecision;
    [SerializeField] private GameObject imageRaycast;
    private SPenitent selectedCulprit;
    private Button confirm;
    private Button cancel;

    public event Action<SPenitent> OnCulpritConfirmed; // evento para notificar confirmación de culpable

    private void Awake()
    {
        confirm = confirmDecision.GetComponent<Button>();
        cancel = cancelDecision.GetComponent<Button>();
        confirm.onClick.AddListener(OnConfirm);
        cancel.onClick.AddListener(OnCancel);
    }
    private void Start()
    {
        gameObject.SetActive(false); // Asegurarse de que el panel de confirmación esté oculto al inicio
    }
    private void OnDisable()
    {
        imageRaycast.SetActive(false); // Desactivar el raycast para evitar interacciones mientras el panel de confirmación está oculto
    }
    public void OnConfirm()
    {
        ConfirmSelection();
    }

    private void ConfirmSelection()
    {
        if (selectedCulprit != null)
        {
            Debug.Log($"ConfirmDecision: Confirmed selection of {selectedCulprit.CharacterName}");
            // Aquí puedes agregar la lógica para procesar la selección del culpable
            // Por ejemplo, notificar a otros sistemas o actualizar el estado del juego
            gameObject.SetActive(false); // Ocultar el panel después de confirmar
            OnCulpritConfirmed?.Invoke(selectedCulprit); // Invocar el evento con el culpable confirmado
        }
        else
        {
            Debug.LogWarning("ConfirmDecision: No culprit selected to confirm.");
        }
    }

    public void OnCancel()
    {
        selectedCulprit = null;
        imageRaycast.SetActive(false); // Desactivar el raycast para evitar interacciones mientras el panel de confirmación está activo
        gameObject.SetActive(false);
    }

    internal void initialize(SPenitent penitent)
    {
        selectedCulprit = penitent;
        Debug.Log($"ConfirmDecision: Initialized with culprit {selectedCulprit.CharacterName}");
        gameObject.SetActive(true); // Mostrar el panel de confirmación
        imageRaycast.SetActive(true); // Activar el raycast para permitir interacciones mientras el panel de confirmación está activo

    }
}
