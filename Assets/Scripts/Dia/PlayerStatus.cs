using System;
using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatus", menuName = "Scriptable Objects/PlayerStatus", order = 1)]
public class PlayerStatus : ScriptableObject
{
    [SerializeField] private float energy;
    private readonly float maxEnergy = 3;
    private readonly float minEnergy = 0;
    [SerializeField] private float faith;
    private readonly float maxFaith = 15;
    private readonly float minFaith;
    [SerializeField] private float money;
    private readonly float maxMoney = 9999;
    private readonly float minMoney = 0;
    [SerializeField] private float repIglesia;
    private readonly float minRepIglesia = 0;
    private readonly float maxRepIglesia = 15;
    [SerializeField] private float repPueblo;
    private readonly float minRepPueblo = 0;
    private readonly float maxRepPueblo = 15;
    [SerializeField] private float food;
    private readonly float maxFood = 99;
    private readonly float minFood = 0;
    [SerializeField] private int day = 1;
    [SerializeField] private bool Cleaned;
    [SerializeField] private bool misaDone;

    #region Getters
    //energy
    public float Energy { get => energy; }
    public float MaxEnergy { get => maxEnergy; }
    public float MinEnergy { get => minEnergy; }

    //faith
    public float Faith { get => faith; }
    public float MaxFaith { get => maxFaith; }
    public float MinFaith { get => minFaith; }

    //money
    public float Money { get => money; }
    public float MaxMoney { get => maxMoney; }
    public float MinMoney { get => minMoney; }

    //reputation iglesia
    public float RepIglesia { get => repIglesia; }
    public float MaxRepIglesia { get => maxRepIglesia; }
    public float MinRepIglesia { get => minRepIglesia; }

    //reputation pueblo
    public float RepPueblo { get => repPueblo; }
    public float MaxRepPueblo { get => maxRepPueblo; }
    public float MinRepPueblo { get => minRepPueblo; }

    //food
    public float Food { get => food; }
    public float MaxFood { get => maxFood; }
    public float MinFood { get => minFood; }

    //estado de limpieza
    public bool cleaned { get => Cleaned; }
    //day
    public int Day { get => day; }

    //misa done
    public bool MisaDone { get => misaDone; }
    #endregion

    public void Getmoney(float amount)
    {
        money = math.clamp(money + amount, minMoney, maxMoney);
    }
    public void Spendmoney(float amount)
    {
        money = math.clamp(money - amount, minMoney, maxMoney);
    }
    public void RestoreEnergy(float amount)
    {
        energy = math.clamp(energy + amount, minEnergy, maxEnergy);

    }
    public void DecreaseEnergy(float amount)
    {
        energy = math.clamp(energy - amount, minEnergy, maxEnergy);
    }
    public void IncreaseFaith(float amount)
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
    public void SetMisaDone(bool status)
    {
        misaDone = status;
    }

    public void SetDay(int dayNumber)
    {
        day = dayNumber;
    }

    internal void ResetFaith()
    {
        faith = 7;
    }

    internal void ResetRepIglesia()
    {
        repIglesia = 7;
    }

    internal void ResetRepPueblo()
    {
        repPueblo = 7;
    }
    internal void ResetFood()
    {
        food = 2;
    }
    internal void ResetEnergy()
    {
        energy = 3;
    }
    internal void ResetMoney()
    {
        money = 0;
    }
    internal void ResetCleaned()
    {
        Cleaned = false;
    }
    internal void ResetMisa()
    {
        misaDone = false;
    }
    internal void ResetAllStatus()
    {
        ResetEnergy();
        ResetFaith();
        ResetMoney();
        ResetRepIglesia();
        ResetRepPueblo();
        ResetFood();
        ResetCleaned();
        ResetMisa();
    }
}
