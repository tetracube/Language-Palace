using UnityEngine;

public class PanelFinNivel : Transicion
{
    private static PanelFinNivel instance;


    [SerializeField]
    private UnityEngine.UI.Button continuar;

    public static PanelFinNivel GetInstance()
    {
        return instance;
    }
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Esconder();
           }

    public static void Mostrar()
    {
        JugadorManager.GetInstance().MostrarPuntero(true);
        if (GameManager.GetInstance().nivel.numero == 2) instance.continuar.gameObject.SetActive(false);
        instance.gameObject.SetActive(true);
    }

    public static void Esconder()
    {
        instance.gameObject.SetActive(false);
    }
    public void VolverMenu() {
        Esconder();
        GameManager.GetInstance().VolverMenu();
    }

    public void RepetirNivel() {
        Esconder();
        GameManager.GetInstance().ReintentarNivel();
    }
    public void Continuar()
    {
        Esconder();
        GameManager.GetInstance().SiguienteNivel();
    }
}
