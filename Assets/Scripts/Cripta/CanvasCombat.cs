using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
public enum CombatPhase { Phase1, Phase2, Phase3 }
public class CanvasCombat : MonoBehaviour
{
    [SerializeField] private Combate combate;
    [SerializeField] private GameObject boss;
    [SerializeField] private CombatDialogue combatDialogue;
    private PlayerController playerController;
    private A_Decision decision;

    [Header("Duración de cada fase en segundos")]
    [SerializeField] private float phase1Duration = 40f;
    [SerializeField] private float phase2Duration = 40f;
    private CombatPhase nextPhase;
    private bool phaseTransitioning = false;

    public Action onCombatUpdated;
    public Action<CombatPhase> onChangePhase;

    public GameObject Boss { get => boss; set => boss = value; }
    public Combate Combate { get => combate; }
    public CombatDialogue CombatDialogue { get => combatDialogue; }

    public void Initialize(PlayerController playerController, A_Decision _Decision)
    {
        this.playerController = playerController;
        this.decision = _Decision;
        boss.SetActive(true);
        combate.Initialize(playerController);
        combatDialogue.Initialize(playerController, decision.PenitentSelected);
        StartCombat();
    }

    private void StartCombat()
    {
        onCombatUpdated?.Invoke();
        // Iniciar la primera fase con su temporizador
        phaseTransitioning = false;
        HandlePhaseChange(CombatPhase.Phase1);
        StartCoroutine(PhaseTimer(phase1Duration, CombatPhase.Phase2));
    }

    private void Start()
    {
        onChangePhase += HandlePhaseChange;

    }
    private void Update()
    {
        if (combate.CurrentPhase == CombatPhase.Phase1 || combate.CurrentPhase == CombatPhase.Phase2 || combate.CurrentPhase == CombatPhase.Phase3)
        {
            combate.MovePlayer();
        }

        // Solo ejecutar spawn si no estamos en transición de diálogo
        if (phaseTransitioning) return;

        switch (combate.CurrentPhase)
        {
            case CombatPhase.Phase1:
                combate.UpdatePhase1Spawn();
                break;
            case CombatPhase.Phase2:
                combate.UpdatePhase2Spawn();
                break;
            case CombatPhase.Phase3:
                combate.UpdatePhase3Spawn();
                break;
        }
    }

    /// <summary>
    /// Espera la duración indicada y luego lanza la transición al diálogo de la siguiente fase.
    /// </summary>
    private IEnumerator PhaseTimer(float duration, CombatPhase nextPhase)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        TransicionCombatDialogue(nextPhase);
    }

    private void HandlePhaseChange(CombatPhase phase)
    {
        combate.SetPhase(phase);
    }

    private void TransicionCombatDialogue(CombatPhase nextPhase)
    {
        this.nextPhase = nextPhase;
        phaseTransitioning = true;
        combatDialogue.gameObject.SetActive(true);
        combatDialogue.StartDialogue(nextPhase);
        combatDialogue.onDialogueFinished += OnDialogueFinished;

    }
    private void OnDialogueFinished()
    {
        // Desuscribir para evitar acumulación de listeners
        combatDialogue.onDialogueFinished -= OnDialogueFinished;
        combatDialogue.gameObject.SetActive(false);
        combate.SetPhase(nextPhase);
        phaseTransitioning = false;

        // Lanzar el temporizador de la siguiente fase si corresponde
        if (nextPhase == CombatPhase.Phase2)
        {
            StartCoroutine(PhaseTimer(phase2Duration, CombatPhase.Phase3));
        }
        // Phase3 no tiene transición automática (es la última)
    }

}
