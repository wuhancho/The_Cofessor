using UnityEngine;

public class Dia: MonoBehaviour
{
    [SerializeField] int numberDay;
    private IFases[] faseActual;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PenitentController penitentController;
    //PlayerStatus playerStatus;

    private void Start()
    {
        Initialize(playerController);
    }

    public void Initialize(PlayerController pController)
    {
        faseActual = GetComponentsInChildren<IFases>();

        this.playerController = pController;

        for (int i = 0; i < faseActual.Length; i++)
        {
            faseActual[i].Initialize(pController);
        }
    }
    public void UpdateDay()
    {
        numberDay++;
    }
    public void EndDay()
    {

    }
    public int GetNumberDay()
    {
        return numberDay;
    }
    
}
