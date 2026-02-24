using System;
using The_cofessor.Personajes.Dialogs;
using UnityEngine;

public class A_Decision : MonoBehaviour, IAcciones
{
    [SerializeField] private int dayToActivate;
    private SPenitent[] todayPenitents;
    private int todayPenintentIndex;
    [SerializeField] private VoteCanvas voteCanvas;
    [SerializeField] private CriptaDialogue criptaDialogue;
    private PlayerController playerController;
    private PenitentController penitentController;
    private SPenitent penitentSelected;
    private Texture2D[] penitentImages;

    private void Start()
    {
        voteCanvas.OnCulpritSelected += HandlePenitentSelected; // Suscribirse al evento de selección de culpable
    }


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
        todayPenitents = penitentController.GetSPenitents(day);
        todayPenintentIndex = 0;
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
    private void HandlePenitentSelected(SPenitent penitent)
    {
        Debug.Log($"Penitente seleccionado: {penitent.CharacterName}");
        penitentSelected = penitent;
        Dialog selectedDialog = GetDialogue(penitent);
        if (selectedDialog == null)
        {
            Debug.LogError($"No se encontró diálogo para {penitent.CharacterName}. No se inicia diálogo.");
            return;
        }
        voteCanvas.gameObject.SetActive(false);
        criptaDialogue.gameObject.SetActive(true);
        criptaDialogue.Initialize(playerController.PlayerConversant);
        Sprite sprite = GetCurrentPeninentSprite();
        criptaDialogue.SetupSpeakerSprite(sprite);
        playerController.PlayerConversant.StartDialogue(selectedDialog);

    }

    private Dialog GetDialogue(SPenitent penitent)
    {
        if (penitent == null) return null;

        Debug.Log($"Obteniendo diálogo tipo 'C' para {penitent.CharacterName} en el día {dayToActivate}");

        // Buscar diálogo de tipo "C" para el día actual
        Dialog dialog = penitent.GetDialogByTypeAndDay("C", dayToActivate);

        // Si no se encuentra con día, buscar solo por tipo (por si "AG_C" no tiene día)
        if (dialog == null)
        {
            dialog = penitent.GetDialogByType("C");
        }

        if (dialog != null)
        {
            playerController.PlayerConversant.CurrentSpeakerNPC = penitent.CharacterName;
            Debug.Log($"Diálogo encontrado: {dialog.name} para {penitent.CharacterName}");
        }
        else
        {
            Debug.LogWarning($"No se encontró diálogo tipo 'C' para {penitent.CharacterName}");
        }

        return dialog;
    }
    public Sprite GetCurrentPeninentSprite()
    {
        SPenitent penitent = penitentSelected;
        if (penitent != null)
        {
            Texture2D[] textures = penitent.GetTextures2D();
            if (textures != null && textures.Length > 0)
            {
                return Sprite.Create(textures[0], new Rect(0, 0, textures[0].width, textures[0].height), new Vector2(0.5f, 0.5f));
            }
        }
        return null;
    }
    private void UpdatePenitentImage(SPenitent penitent)
    {
        penitentImages = penitent.GetTextures2D();
        if (penitentImages != null && penitentImages.Length > 0)
        {
            playerController.PlayerConversant.SetIconNPC(penitentImages[0]);
        }
    }

}
