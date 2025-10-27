using UnityEngine;

public interface IAccionesEnergia : IAcciones
{
    public int EnergyCost { get; }
    public int FaithCost { get; }
    public int ReputationChurchCost { get; }
    public int ReputationPeopleCost { get; }
    
}
