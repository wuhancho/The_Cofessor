using UnityEngine;

public class PlayerStatus: MonoBehaviour
{
    [SerializeField] private float energy;
    [SerializeField] private float faith;
    [SerializeField] private float money;
    [SerializeField] private float repIglesia;
    [SerializeField] private float repPueblo;
    [SerializeField] private float food;

    public float Energy { get => energy; set => energy = value; }
    public float Faith { get => faith; set => faith = value; }
    public float Money { get => money; set => money = value; }
    public float RepIglesia { get => repIglesia; set => repIglesia = value; }
    public float RepPueblo { get => repPueblo; set => repPueblo = value; }
    public float Food { get => food; set => food = value; }
}
