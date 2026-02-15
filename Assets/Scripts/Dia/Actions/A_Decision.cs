using UnityEngine;

public class A_Decision : MonoBehaviour, IAcciones
{
    [SerializeField] private int dayToActivate;
    [SerializeField] private VoteCanvas voteCanvas;
    private PlayerController playerController;
    private PenitentController penitentController;
    public void Initialize(PlayerController playerController)
    {
        
    }
    public void Initialize(PlayerController playerController, PenitentController penitentController)
    {
        //Debug.Log("A_confecciones - Initialize invoked.");
        this.playerController = playerController;
        this.penitentController = penitentController;
    }

    public void SetDay(int day)
    {
        dayToActivate = day;
    }
    public void EjecutarAccion(PlayerController playerController)
    {
        
    }
    public void TriggerAction()
    {
        voteCanvas.gameObject.SetActive(true);
        voteCanvas.Initialize(penitentController, playerController,dayToActivate);
    }
    public void CancelAction()
    {
       
    }

    public void DebugAccion()
    {
        
    }



}
