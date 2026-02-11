using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class CÓDIGODEPAUSA : MonoBehaviour
{
    public GameObject ObjetoMenuPausa;
    public bool Pausa = false;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (Pausa == false)
            {
                ObjetoMenuPausa.SetActive(true);
                Pausa = true;

                Time.timeScale = 0;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else if (Pausa == true)
                {
                Volver();
            }
        }
    }

    public void Volver()
    {
        ObjetoMenuPausa.SetActive(false);
        Pausa = false;

        Time.timeScale = 1;
    }
}
        
