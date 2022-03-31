using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuPrincipal : MonoBehaviour
{

    private static MenuPrincipal instance;
    [SerializeField]
    private Button continuar;
    [SerializeField]
    private Button seleccionarCapitulo;
    [SerializeField]
    private GameObject nuevaPartida;

    private bool primeraPartida;


    private int[] palabrasPorNivel = { 0, 10, 20, 10, 25 };
    void Awake()
    {
        instance = this;

    }
    private void Start()
    {
        Mostrar();
    }
    public static MenuPrincipal GetInstance() { return instance; }
    public static void Mostrar()
    {
        instance.gameObject.SetActive(true);
        instance.primeraPartida = DataBase.EsPrimeraPartida();
        instance.continuar.gameObject.SetActive(!instance.primeraPartida);
        instance.seleccionarCapitulo.gameObject.SetActive(!instance.primeraPartida);
    }

    public static void Esconder()
    {
        instance.gameObject.SetActive(false);
    }
    public void Continuar() {
        PanelPalabras.GetInstance().gameObject.SetActive(true);
        PanelPalabras.pantallaAnterior = Pantalla.MenuPrincipal;

        int ultimoNivelSuperado = DataBase.UltimoNivelSuperado();
        var nivel = ultimoNivelSuperado + 1;
        if (nivel > 2) nivel = 2;
        PanelPalabras.MostrarEstatico(palabrasPorNivel[nivel], nivel);
    }

    public void SeleccionarCapitulo() {
        PanelSelectorNiveles.Mostrar();
    }
    public void NuevaPartida()
    {
        if (!primeraPartida) { nuevaPartida.SetActive(true); }
        else {
            PanelDatosUsuario.Mostrar();
        }
    }

    public void Salir()
    {
        DataBase.GetInstance().CerrarConexion();
        Application.Quit();
    }

    public void IniciarContinuar() {
        StartCoroutine(ContinuarEnUnSegundo());
    }
    IEnumerator ContinuarEnUnSegundo() {

        yield return new WaitForSecondsRealtime(1f);
        Continuar();
    }

}
