using System;
using The_cofessor.Personajes.Dialogs;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private PlayerConversant playerConversant;

    public PlayerStatus PlayerStatus { get => playerStatus; set => playerStatus = value; }
    public PlayerConversant PlayerConversant { get => playerConversant; set => playerConversant = value; }
    public void ChangeStatesPlayer(DialogNode dialogNode)
    {
        if (dialogNode.GetFaithCost() != 0)
        {
            if (dialogNode.GetFaithCost() < 0)
            {
                PlayerStatus.DecreaseFaith(dialogNode.GetFaithCost());
                return;
            }
            else if (dialogNode.GetFaithCost() >= 0)
            {
                PlayerStatus.IncreaseFaith(dialogNode.GetFaithCost());
            }
        }
        if (dialogNode.GetRepIglesiaCost() != 0)
        {
            if (dialogNode.GetRepIglesiaCost() < 0)
            {
                PlayerStatus.DecreaseRepIglesia(dialogNode.GetRepIglesiaCost());
                return;
            }
            else if (dialogNode.GetRepIglesiaCost() >= 0)
            {
                PlayerStatus.IncreaseRepIglesia(dialogNode.GetRepIglesiaCost());
            }
        }
        if (dialogNode.GetRepPuebloCost() != 0)
        {
            if (dialogNode.GetRepPuebloCost() < 0)
            {
                PlayerStatus.DecreaseRepPueblo(dialogNode.GetRepPuebloCost());
                return;
            }
            else if (dialogNode.GetRepPuebloCost() >= 0)
            {
                PlayerStatus.IncreaseRepPueblo(dialogNode.GetRepPuebloCost());
            }
        }
        if (dialogNode.GetSobornoCost() != 0)
        {
            if (PlayerStatus.Money < dialogNode.GetSobornoCost())
            {
                Debug.Log("No tienes suficiente dinero para sobornar.");
                return;
            }
            else if (PlayerStatus.Money >= dialogNode.GetSobornoCost())
            {
                PlayerStatus.Spendmoney(dialogNode.GetSobornoCost());
            }
            //playerController.ChangeMoney(dialogNode.GetSobornoCost());
        }
    }

}
