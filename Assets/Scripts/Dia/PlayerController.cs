using System;
using The_cofessor.Personajes.Dialogs;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private PlayerConversant playerConversant;
    private DialogNode dialogNode;


    public PlayerStatus PlayerStatus { get => playerStatus; set => playerStatus = value; }
    public PlayerConversant PlayerConversant { get => playerConversant; set => playerConversant = value; }
    public void SetDialogNode(DialogNode node)
    {
        dialogNode = node;
    }
    public void ChangeStatesPlayer(DialogNode dialogNode)
    {
        
        if (dialogNode.GetFaithCost() != 0)
        {
            if (dialogNode.GetFaithCost() < 0)
            {
                Debug.Log("Change faith+");
                PlayerStatus.DecreaseFaith(dialogNode.GetFaithCost());
                return;
            }
            else if (dialogNode.GetFaithCost() >= 0)
            {
                Debug.Log("change faith-");
                PlayerStatus.IncreaseFaith(dialogNode.GetFaithCost());
            }
        }
        if (dialogNode.GetRepIglesiaCost() != 0)
        {
            if (dialogNode.GetRepIglesiaCost() < 0)
            {
                Debug.Log("Change rep iglesia-");
                PlayerStatus.DecreaseRepIglesia(dialogNode.GetRepIglesiaCost());
                return;
            }
            else if (dialogNode.GetRepIglesiaCost() >= 0)
            {
                Debug.Log("Change rep iglesia+");
                PlayerStatus.IncreaseRepIglesia(dialogNode.GetRepIglesiaCost());
            }
        }
        if (dialogNode.GetRepPuebloCost() != 0)
        {
            if (dialogNode.GetRepPuebloCost() < 0)
            {
                Debug.Log("Change rep pueblo-");
                PlayerStatus.DecreaseRepPueblo(dialogNode.GetRepPuebloCost());
                return;
            }
            else if (dialogNode.GetRepPuebloCost() >= 0)
            {
                Debug.Log("Change rep pueblo+");
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
                Debug.Log("Has sobornado con exito.");
                PlayerStatus.Spendmoney(dialogNode.GetSobornoCost());
            }
            //playerController.ChangeMoney(dialogNode.GetSobornoCost());
        }
    }
    public void ListenEvent(DialogNode node)
    {
        SetDialogNode(node);
        ChangeStatesPlayer(node);
    }

}
