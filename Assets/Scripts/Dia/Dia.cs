using UnityEngine;

public class Dia : MonoBehaviour
{
    [SerializeField] int numberDay;
    private IFases[] faseActual;
    private PlayerController playerController;
    private PenitentController penitentController;

    public void Awake()
    {
        if (playerController == null)
        {
            playerController = FindAnyObjectByType<PlayerController>();
        }
        if (penitentController == null)
        {
            penitentController = FindAnyObjectByType<PenitentController>();
        }
        Initialize(playerController, penitentController);
    }

    public void Initialize(PlayerController pController, PenitentController ptController)
    {
        faseActual = GetComponentsInChildren<IFases>();

        this.playerController = pController;

        for (int i = 0; i < faseActual.Length; i++)
        {
            faseActual[i].Initialize(pController, ptController);
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
