using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class VoteCanvas : MonoBehaviour
{
    [SerializeField] private Culprit_Button culpritButtonPrefab; // Prefab del botón de culpable
    [SerializeField] private ConfirmDecision confirmDecision; // Game object que contiene los dos botones de confirmar y cancelar
    [SerializeField] private GameObject HorizontalGroupIz; // Game objects que actúan como contenedores para los botones de culpable
    [SerializeField] private GameObject HorizontalGroupDe; // Game objects que actúan como contenedores para los botones de culpable
    [SerializeField] private Button noVoteButton; // Botón para votar "No votar"
    private Culprit_Button[] culpritButtons; // Array para almacenar las referencias a los botones de culpable creados
    private PenitentController penitentController; // Referencia al PenitentController para obtener los penitentes del día
    private PlayerController playerController;
    private int dia;

    public event Action<SPenitent> OnCulpritSelected; // evento para notificar selección de culpable
    private void Start()
    {
        gameObject.SetActive(false); // Asegurarse de que el canvas de votación esté oculto al inicio
        confirmDecision.OnCulpritConfirmed += HandleCulpritConfirmed; // Suscribirse al evento de confirmación del culpable
    }


    public void Initialize(PenitentController ptController, PlayerController player, int dia)
    {
        penitentController = ptController;
        playerController = player;
        this.dia = dia;
        CreateCulpritButtons();
        Debug.Log($"VoteCanvas: inicializado para el día {dia} con {culpritButtons.Length} botones de culpable");
        culpritButtons.ToList().ForEach(button => button.OnCulpritClicked += HandleCulpritSelected); // Suscribirse al evento de cada botón
    }

    private void HandleCulpritSelected(SPenitent penitent)
    {
        confirmDecision.initialize(penitent);
    }
    private void HandleCulpritConfirmed(SPenitent penitent)
    {
        OnCulpritSelected?.Invoke(penitent); // Notificar a los suscriptores que se ha confirmado un culpable
    }

    private void CreateCulpritButtons()
    {
        SPenitent[] penitents = penitentController.GetSPenitents(dia);
        int total = penitents.Length;
        int half = Mathf.CeilToInt(total / 2f); // mitad redondeada hacia arriba para el grupo izquierdo
        culpritButtons = new Culprit_Button[total];

        for (int i = 0; i < total; i++)
        {
            // Primera mitad → hijo de HorizontalGroupIz, segunda mitad → hijo de HorizontalGroupDe
            Transform parent = (i < half)
                ? HorizontalGroupIz.transform
                : HorizontalGroupDe.transform;

            Culprit_Button button = Instantiate(culpritButtonPrefab, parent);
            button.Initialize(penitents[i]);
            culpritButtons[i] = button;
        }

        Debug.Log($"VoteCanvas: creados {total} botones ({half} izq, {total - half} der) para el día {dia}");
    }

}
