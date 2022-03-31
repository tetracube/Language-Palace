using UnityEngine;

public class Temporizador : MonoBehaviour
{
    private bool activo = false;
    private float tiempoActual;

    public void Iniciar() {
        tiempoActual = 0;
        activo = true;
    }
    public float Pausar()
    {
        activo = false;
        return tiempoActual;
    }
    public void Reanudar()
    {
        activo = true;
    }
    void Update()
    {
        if (activo) {
            tiempoActual += Time.deltaTime;
        }
    }

  
}
