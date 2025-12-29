using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatus", menuName = "Scriptable Objects/PlayerStatus", order = 1)]
public class PlayerStatus: ScriptableObject
{
    [SerializeField] private float energy;
    private float maxEnergy = 3;
    private float minEnergy = 0;
    [SerializeField] private float faith;
    private float maxFaith = 15;
    private float minFaith;
    [SerializeField] private float money;
    private float maxMoney = 9999;
    private float minMoney = 0;
    [SerializeField] private float repIglesia;
    private float minRepIglesia = 0;
    private float maxRepIglesia = 15;
    [SerializeField] private float repPueblo;
    private float minRepPueblo = 0;
    private float maxRepPueblo = 15;
    [SerializeField] private float food;
    private float maxFood = 9999;
    private float minFood = 0;
    [SerializeField] private bool Cleaned;
    [SerializeField] private int day = 1;
    
    public float Energy { get => energy; }
    public float Faith { get => faith; }
    public float Money { get => money; }
    public float RepIglesia { get => repIglesia; }
    public float RepPueblo { get => repPueblo; }
    public float Food { get => food; }
    public bool cleaned { get => Cleaned; }
    public int Day { get => day; }

    public void Getmoney(float amount)
    {
        money = math.clamp(money + amount, minMoney, maxMoney);
    }
    public void Spendmoney(float amount)
    {
        money = math.clamp(money + amount, minMoney, maxMoney);
    }
    public void RestoreEnergy(float amount)
    {
        energy = math.clamp(energy + amount, minEnergy, maxEnergy);

    }
    public void DecreaseEnergy(float amount)
    {
        energy = math.clamp(energy + amount, minEnergy, maxEnergy);
    }
    public void IncreaseFaith(float amount)
    {
        faith = math.clamp(faith + amount, minFaith, maxFaith);
    }
    public void DecreaseFaith(float amount)
    {
        faith = math.clamp(faith + amount, minFaith, maxFaith);
    }
    public void IncreaseRepIglesia(float amount)
    {
        repIglesia = math.clamp(repIglesia + amount, minRepIglesia, maxRepIglesia);
    }
    public void DecreaseRepIglesia(float amount)
    {
        repIglesia = math.clamp(repIglesia + amount, minRepIglesia, maxRepIglesia);
    }
    public void IncreaseRepPueblo(float amount)
    {
        repPueblo = math.clamp(repPueblo + amount, minRepPueblo, maxRepPueblo);
    }
    public void DecreaseRepPueblo(float amount)
    {
        repPueblo = math.clamp(repPueblo + amount, minRepPueblo, maxRepPueblo);
    }
    public void GetFood(float amount)
    {
        food = math.clamp(food + amount, minFood, maxFood);
    }
    public void SpendFood(float amount)
    {
        food = math.clamp(food + amount, minFood, maxFood);
    }
    public void SetCleaned(bool status)
    {
        Cleaned = status;
    }

    public void SetDay(int dayNumber)
    {
        day = dayNumber;
    }
}
