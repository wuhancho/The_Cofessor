using Unity.Mathematics;
using UnityEngine;

public class PlayerStatus: MonoBehaviour
{
    [SerializeField] private float energy;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float minEnergy;
    [SerializeField] private float faith;
    [SerializeField] private float maxFaith;
    [SerializeField] private float minFaith;
    [SerializeField] private float money;
    [SerializeField] private float maxMoney;
    [SerializeField] private float minMoney;
    [SerializeField] private float repIglesia;
    [SerializeField] private float minRepIglesia;
    [SerializeField] private float maxRepIglesia;
    [SerializeField] private float repPueblo;
    [SerializeField] private float minRepPueblo;
    [SerializeField] private float maxRepPueblo;
    [SerializeField] private float food;
    [SerializeField] private float maxFood;
    [SerializeField] private float minFood;
    [SerializeField] private bool Cleaned;
    
    public float Energy { get => energy; }
    public float Faith { get => faith; }
    public float Money { get => money; }
    public float RepIglesia { get => repIglesia; }
    public float RepPueblo { get => repPueblo; }
    public float Food { get => food; }
    public bool cleaned { get => Cleaned; }

    public void Getmoney(float amount)
    {
        money += amount;
    }
    public void Spendmoney(float amount)
    {
        money -= amount;
    }
    public void RestoreEnergy(float amount)
    {
        energy = math.clamp(energy + amount, minEnergy, maxEnergy);
    }
    public void DecreaseEnergy(float amount)
    {
        energy = math.clamp(energy - amount, minEnergy, maxEnergy);
    }
    public void RestoreFaith(float amount)
    {
        faith = math.clamp(faith + amount, minFaith, maxFaith);
    }
    public void DecreaseFaith(float amount)
    {
        faith = math.clamp(faith - amount, minFaith, maxFaith);
    }
    public void IncreaseRepIglesia(float amount)
    {
        repIglesia = math.clamp(repIglesia + amount, minRepIglesia, maxRepIglesia);
    }
    public void DecreaseRepIglesia(float amount)
    {
        repIglesia = math.clamp(repIglesia - amount, minRepIglesia, maxRepIglesia);
    }
    public void IncreaseRepPueblo(float amount)
    {
        repPueblo = math.clamp(repPueblo + amount, minRepPueblo, maxRepPueblo);
    }
    public void DecreaseRepPueblo(float amount)
    {
        repPueblo = math.clamp(repPueblo - amount, minRepPueblo, maxRepPueblo);
    }
    public void GetFood(float amount)
    {
        food = math.clamp(food + amount, minFood, maxFood);
    }
    public void SpendFood(float amount)
    {
        food = math.clamp(food - amount, minFood, maxFood);
    }
    public void SetCleaned(bool status)
    {
        Cleaned = status;
    }
}
