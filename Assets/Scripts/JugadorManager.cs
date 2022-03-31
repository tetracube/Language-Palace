using UnityEngine;

public class JugadorManager : MonoBehaviour
{
    public GameObject personaje;
    private static JugadorManager instance;
    private JugadorManager() { }

    public static JugadorManager GetInstance()
    {
        return instance;
    }

    void Awake()

    {
        instance = this;

    }

    public void CongelarJugador(bool conjelar)
    {
        personaje.GetComponent<MovimientoJugador>().Parar(conjelar);
        personaje.GetComponent<CamaraJugador>().Frenar(conjelar);
    }
    public void MostrarPuntero(bool mostrar)
    {
        if (mostrar)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

    }

    public void PosicionarJugador()
    {
        personaje.transform.position = new Vector3(80.4602f, 0.005000055f, -0.169294f);
        personaje.transform.eulerAngles = new Vector3(0, -270, 0);
    }

    public void ActualizarSensibilidad()
    {
        personaje.GetComponent<CamaraJugador>().sensibilidadRaton = PanelAjustes.sensibilidad;
    }

}
