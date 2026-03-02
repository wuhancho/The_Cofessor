using UnityEngine;

public class EvAmuleto : DayEvent
{

    [SerializeField] private int day;
    [Header("Asigna unicamente LOS DOS GAMEOBJECTS que representaran\n la portada[0] y la noticia[1] ponlos en ese orden. ")]
    [SerializeField] private GameObject[] newsPaper;
    [SerializeField] private TypeEventDay typeEventDay;
    [SerializeField] private SPenitent guiltyPenitent;

    public override SPenitent GuiltyPenitent => guiltyPenitent;

    private void Start()
    {
        guiltyPenitent.isGuilty = true;
    }


    public override TypeEventDay GetTypeEventDay()
    {
        return typeEventDay;
    }
    public override void ActivateEvent()
    {
        guiltyPenitent.isGuilty = true;
        base.ActivateEvent();
    }
    public override void DeactivateEvent()
    {
        guiltyPenitent.isGuilty = false;
        base.DeactivateEvent();
    }

    public override int Day => day;

    public override GameObject[] NewsPaper => newsPaper;

}
