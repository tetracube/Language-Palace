using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PanelSelectorNiveles : MenuTransicion
{
    private static PanelSelectorNiveles instance;
    [SerializeField]
    private Button aceptar;
    [SerializeField]
    private GameObject descripcionNivel;
    [SerializeField]
    private GameObject mejorJugada;
    [SerializeField]
    private Image [] circulosProgreso;
    [SerializeField]
    private TMP_Text nivelText;
    [SerializeField]
    private GameObject recuadroDescripcion;
    [SerializeField]
    private Sprite[] recuadrosDescripcion;

    public int nivelElegido = -1;
    private int ultimoNivelSuperado;

    private int[] palabrasPorNivel = { 0, 10, 20, 10, 25 };
    private int[] habitacionesPorNivel = {0,  10, 18, 10, 18 };
    private string[] tiempoPorNivel = {"-", "-", "-", "10:00", "-" };

    private int[] mejorJugadaPorNivel = new int[5];
    private int[] tiempoMejorJugadaPorNivel = new int[5];
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
        instance.gameObject.SetActive(true);
        instance.ObtenerDatosMejoresJugadas();
        instance.NivelSeleccionado(1);
        instance.ActualizarCirculos();
        instance.ultimoNivelSuperado = DataBase.UltimoNivelSuperado();
    }
    

    public static void Esconder()
    {
        instance.FadeOut(() => instance.gameObject.SetActive(false));
        //instance.gameObject.SetActive(false);
    }
    public void NivelSeleccionado(int nivel) {
        nivelElegido = nivel;
        ActualizarPanelNivel();
        ActualizarVisibilidadAceptar();
    }
    private void ActualizarPanelNivel() {
        recuadroDescripcion.GetComponent<Image>().sprite = recuadrosDescripcion[nivelElegido - 1];
        nivelText.text = "NIVEL " + Utils.ToRomanNumber(nivelElegido);

        foreach (TMP_Text texto in mejorJugada.GetComponentsInChildren<TMP_Text>()) {
            switch (texto.name) {
                case "Texto puntuacion":
                    texto.text = mejorJugadaPorNivel[nivelElegido] + "%";
                    break;
                case "Texto tiempo":
                    texto.text = tiempoMejorJugadaPorNivel[nivelElegido] > 0 ? System.TimeSpan.FromSeconds(tiempoMejorJugadaPorNivel[nivelElegido]).ToString(@"mm\:ss") : "-";
                    break;
                case "Texto fallos":
                    texto.text = 10 - mejorJugadaPorNivel[nivelElegido] / 10 + "";
                    break;
            }
        }

        foreach (TMP_Text texto in descripcionNivel.GetComponentsInChildren<TMP_Text>())
        {
            switch (texto.name)
            {
                case "Texto objetos":
                    texto.text = palabrasPorNivel[nivelElegido].ToString();
                    break;
                case "Texto habitaciones":
                    texto.text = habitacionesPorNivel[nivelElegido].ToString();
                    break;
                case "Texto tiempo":
                    texto.text = tiempoPorNivel[nivelElegido];
                    break;
            }
        }
    }
    private void ActualizarVisibilidadAceptar() {

        /*12/06/21 LO COMENTO (aun que esta bine el codigo) para que no puedan selccionar otro nivel superiro a uno ya que solo existe ahora ese)
        aceptar.interactable = ultimoNivelSuperado + 1 >= nivelElegido;
        */
        aceptar.interactable = ultimoNivelSuperado + 1 >= nivelElegido && nivelElegido < 3;
    }
    private void ActualizarCirculos() {
        for (int i = 0; i < circulosProgreso.Length; i++) {
            circulosProgreso[i].fillAmount = mejorJugadaPorNivel[i + 1] / 100.0f;
        }
    }

    private void ObtenerDatosMejoresJugadas()
    {   
        for (int i = 1; i < 5; i++)
        {
            var mejorJugada = DataBase.MejorJugada(i);
            mejorJugadaPorNivel[i] = mejorJugada[0];
            tiempoMejorJugadaPorNivel[i] = mejorJugada[1];
        }
    }
    public void Jugar() {
        Esconder();
        PanelPalabras.pantallaAnterior = Pantalla.SelectorNiveles;
        PanelPalabras.MostrarEstatico(palabrasPorNivel[nivelElegido], nivelElegido);
    }

    public void Retroceder()
    {
        Esconder();
        MenuPrincipal.Mostrar();
    }

}
    