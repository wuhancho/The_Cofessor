using System;
using UnityEngine;

public class VoteCanvas : MonoBehaviour
{
    [SerializeField] private Culprit_Button culpritButtonPrefab;
    [SerializeField] private GameObject HorizontalGroupIz;
    [SerializeField] private GameObject HorizontalGroupDe;
    private Culprit_Button[] culpritButtons;
    private PenitentController penitentController;
    private PlayerController playerController;
    private int dia;

    public void Initialize(PenitentController ptController, PlayerController player, int dia)
    {
        penitentController = ptController;
        playerController = player;
        this.dia = dia;
        CreateCulpritButtons();
        Debug.Log($"VoteCanvas: inicializado para el día {dia} con {culpritButtons.Length} botones de culpable");
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
