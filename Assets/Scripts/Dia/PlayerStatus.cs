using UnityEngine;

public class PlayerStatus
{
    private float energy;
    private float faith;
    private float money;
    private float repIglesia;
    private float repPueblo;

    public float Energy { get => energy; set => energy = value; }
    public float Faith { get => faith; set => faith = value; }
    public float Money { get => money; set => money = value; }
    public float RepIglesia { get => repIglesia; set => repIglesia = value; }
    public float RepPueblo { get => repPueblo; set => repPueblo = value; }
}
