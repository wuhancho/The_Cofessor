using System;
using UnityEngine;
public enum CombatPhase { Phase1, Phase2, Phase3 }
public class CanvasCombat : MonoBehaviour
{
    [SerializeField] private Combate combate;
    [SerializeField] private GameObject boss;
    [SerializeField] private CombatDialogue combatDialogue;
    private PlayerController playerController;
    private A_Decision decision;

    private Action onCombatUpdated;
    private Action<CombatPhase> onChangePhase;
    public void initilize(PlayerController playerController, A_Decision _Decision)
    {
        this.playerController = playerController;
        this.decision = _Decision;
    }
    private void Start()
    {
        onChangePhase += HandlePhaseChange;
        onCombatUpdated += UpdateCombat;
    }

    private void UpdateCombat()
    {

    }
    private void HandlePhaseChange(CombatPhase phase)
    {
        throw new NotImplementedException();
    }
}
