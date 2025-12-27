[System.Serializable]
public class PlayerData
{
    public float faith;
    public float money;
    public float repIglesia;
    public float repPueblo;
    public float food;

    public PlayerData (PlayerStatus status)
    {
        faith = status.Faith;
        money = status.Money;
        repIglesia = status.RepIglesia;
        repPueblo = status.RepPueblo;
        food = status.Food;
    }
}
