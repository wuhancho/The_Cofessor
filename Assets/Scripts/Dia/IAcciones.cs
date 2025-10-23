using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public interface IAcciones
{
    /// <summary>
    /// ejecutarAccion - Método para ejecutar una acción específica basada en el nombre proporcionado.
    /// </summary>
    /// <param name="accion"> 
    /// el nombre del método o acción a ejecutar.
    /// aqui se pone el nombre de la accion que se desea ejecutar, Por ejemplo: "Comida", "Limpiar","nombre de los personajes", "etc".
    /// </param>
    public void EjecutarAccion();
}
