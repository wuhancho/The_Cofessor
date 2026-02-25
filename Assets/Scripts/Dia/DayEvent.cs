using UnityEngine;

public class DayEvent : MonoBehaviour
{
    public virtual int Day { get; }
    public virtual GameObject[] NewsPaper { get; }
    public virtual TypeEventDay GetTypeEventDay()
    {
        return TypeEventDay.tutorial; // Devuelve el tipo de evento del día, por ejemplo, tutorial
    }
}
