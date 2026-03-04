using System;
using System.Collections;
using UnityEngine;

public enum CombatPhase { Phase1, Phase2, Phase3 }

/// <summary>
/// Estados internos del flujo de combate.
/// Dialogue = el CombatDialogue está activo, combate pausado.
/// Combat = el jugador esquiva ataques del boss.
/// Ended = combate finalizado.
/// </summary>
public enum CombatState { Dialogue, Combat, Ended }

public class CanvasCombat : MonoBehaviour
{
    [SerializeField] private Combate combate;
    [SerializeField] private GameObject boss;
    [SerializeField] private CombatDialogue combatDialogue;
    private PlayerController playerController;
    private A_Decision decision;

    [Header("Duración de cada fase de combate en segundos")]
    [SerializeField] private float phase1Duration = 40f;
    [SerializeField] private float phase2Duration = 40f;
    [SerializeField] private float phase3Duration = 40f;

    private CombatPhase currentPhase;
    private CombatState currentState;
    private Coroutine phaseTimerCoroutine;

    public Action onCombatUpdated;
    public Action onEndCombat;

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
        onCombatUpdated?.Invoke();

        // Empezar con el diálogo de la fase 1
        currentPhase = CombatPhase.Phase1;
        StartDialogueState();
    }

    private void Start()
    {
        combatDialogue.onDialogueFinished += OnDialogueFinished;
    }

    private void OnDisable()
    {
        combatDialogue.onDialogueFinished -= OnDialogueFinished;
    }

    private void Update()
    {
        // Solo procesar combate cuando estamos en estado Combat
        if (currentState != CombatState.Combat) return;

        combate.MovePlayer();

        switch (currentPhase)
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
    /// Activa el diálogo y desactiva el combate.
    /// </summary>
    private void StartDialogueState()
    {
        currentState = CombatState.Dialogue;
        combatDialogue.gameObject.SetActive(true);
        combatDialogue.StartDialogue(currentPhase);
        Debug.Log($"[CanvasCombat] Diálogo iniciado para {currentPhase}");
    }

    /// <summary>
    /// Callback cuando el CombatDialogue termina. Activa el combate de la fase actual.
    /// </summary>
    private void OnDialogueFinished()
    {
        combatDialogue.gameObject.SetActive(false);
        StartCombatState();
    }

    /// <summary>
    /// Activa el combate y lanza el temporizador de la fase actual.
    /// </summary>
    private void StartCombatState()
    {
        currentState = CombatState.Combat;
        combate.SetPhase(currentPhase);

        float duration = GetPhaseDuration(currentPhase);
        phaseTimerCoroutine = StartCoroutine(PhaseTimer(duration));
        Debug.Log($"[CanvasCombat] Combate iniciado para {currentPhase} ({duration}s)");
    }

    /// <summary>
    /// Espera la duración de la fase y luego transiciona al siguiente estado.
    /// </summary>
    private IEnumerator PhaseTimer(float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        OnPhaseTimeUp();
    }

    /// <summary>
    /// Se ejecuta al terminar el tiempo de combate de la fase actual.
    /// </summary>
    private void OnPhaseTimeUp()
    {
        Debug.Log($"[CanvasCombat] Tiempo de combate agotado para {currentPhase}");

        // Avanzar a la siguiente fase o terminar
        switch (currentPhase)
        {
            case CombatPhase.Phase1:
                currentPhase = CombatPhase.Phase2;
                StartDialogueState();
                break;
            case CombatPhase.Phase2:
                currentPhase = CombatPhase.Phase3;
                StartDialogueState();
                break;
            case CombatPhase.Phase3:
                EndCombat();
                break;
        }
    }

    private void EndCombat()
    {
        currentState = CombatState.Ended;
        Debug.Log("[CanvasCombat] Combate finalizado.");
        onEndCombat?.Invoke();
    }

    private float GetPhaseDuration(CombatPhase phase)
    {
        switch (phase)
        {
            case CombatPhase.Phase1: return phase1Duration;
            case CombatPhase.Phase2: return phase2Duration;
            case CombatPhase.Phase3: return phase3Duration;
            default: return 0f;
        }
    }
}
