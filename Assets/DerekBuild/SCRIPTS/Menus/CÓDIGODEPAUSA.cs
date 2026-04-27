using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;
public class CÓDIGODEPAUSA : MonoBehaviour
{
    public GameObject ObjetoMenuPausa;
    public bool Pausa = false;
    public Animator pergaminoAnimator;
    public GameObject botonesContenido;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Pausa)
            {
                ReanudarJuego();
            }
            else
            {
                PausarJuego();
            }
        }
    }

    public void PausarJuego()
    {
        Pausa = true;
        ObjetoMenuPausa.SetActive(true);
        botonesContenido.SetActive(false);
        pergaminoAnimator.SetTrigger("Abrir");
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ReanudarJuego()
    {
        Pausa = false;
        ObjetoMenuPausa.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ActivarBotonesMenu()
    {
        botonesContenido.SetActive(true);
    }
}
        
