using UnityEngine;

public class DayEvent : MonoBehaviour
{
    public virtual int Day { get; }
    public virtual GameObject[] NewsPaper { get; }
    public virtual SPenitent GuiltyPenitent { get; }
    public virtual TypeEventDay GetTypeEventDay()
    {
        return TypeEventDay.tutorial; // Devuelve el tipo de evento del día, por ejemplo, tutorial
    }
    public virtual void ActivateEvent()
    {
        // Lógica para activar el evento del día
        Debug.Log($"Activando evento del día {Day} de tipo {GetTypeEventDay()}");
    }
    public virtual void DeactivateEvent()
    {
        // Lógica para desactivar el evento del día
        Debug.Log($"Desactivando evento del día {Day} de tipo {GetTypeEventDay()}");
    }
}
