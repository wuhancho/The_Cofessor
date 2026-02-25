using UnityEngine;

public class EvTutorial : DayEvent
{
    [SerializeField] private int day;
    [Header("Asigna unicamente LOS DOS GAMEOBJECTS que representaran\n la portada[0] y la noticia[1] ponlos en ese orden. ")]
    [SerializeField] private GameObject[] newsPaper;
    [SerializeField] private TypeEventDay typeEventDay;


    public override TypeEventDay GetTypeEventDay()
    {
        return typeEventDay;
    }

    public override int Day => day;

    public override GameObject[] NewsPaper => newsPaper;

}
