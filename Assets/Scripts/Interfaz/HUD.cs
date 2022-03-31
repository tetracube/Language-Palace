using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{

    private static HUD instance;

    [SerializeField]
    private TMP_Text objetosEncontrados;
    public static HUD GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
        Esconder();
    }

    public static void Mostrar()
    {
        instance.gameObject.SetActive(true);
    }

    public static void Esconder()
    {
        instance.gameObject.SetActive(false);
    }

    public static void ActualizarContador(int contador)
    {
        instance.objetosEncontrados.text = contador + "";

    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            PanelListaObjetos.GetInstance().gameObject.SetActive(!PanelListaObjetos.GetInstance().gameObject.activeSelf);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PanelAjustes.GetInstance().gameObject.SetActive(!PanelAjustes.GetInstance().gameObject.activeSelf);
            var activadoPanelAjustes = PanelAjustes.GetInstance().gameObject.activeSelf;
            JugadorManager.GetInstance().MostrarPuntero(activadoPanelAjustes);
            JugadorManager.GetInstance().CongelarJugador(activadoPanelAjustes);

        }
    }

}
