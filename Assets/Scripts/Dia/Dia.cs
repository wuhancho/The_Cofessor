using UnityEngine;

public class Dia : MonoBehaviour
{
     private IFases[] faseActual;
    PlayerStatus playerStatus;

    public void Initialize(PlayerStatus playerStatus)
    {
        faseActual = GetComponentsInChildren<IFases>();

        this.playerStatus = playerStatus;

        for (int i = 0; i < faseActual.Length; i++)
        {
            faseActual[i].Initialize(playerStatus);
        }
    }
}
