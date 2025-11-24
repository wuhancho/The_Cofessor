using System;
using The_cofessor.Personajes.Dialogs;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerStatus playerStatus;
    [SerializeField] private PlayerConversant playerConversant;

    public PlayerStatus PlayerStatus { get => playerStatus; set => playerStatus = value; }
    public PlayerConversant PlayerConversant { get => playerConversant; set => playerConversant = value; }

    
}
