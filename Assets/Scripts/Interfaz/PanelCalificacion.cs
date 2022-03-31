using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelCalificacion : Transicion
{
    private static PanelCalificacion instance;
    [SerializeField]
    private TMP_Text puntuacion;
    [SerializeField]
    private TMP_Text fallos;
    [SerializeField]
    private Button boton;
    [SerializeField]
    private Button botonExtra;
    [SerializeField]
    private TMP_Text capitulo;
    [SerializeField]
    private TMP_Text descripcion;
    [SerializeField]
    private Image circuloPuntuacion;

    private int intentos = 0;
    public static int intentosMax { get; set; }

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Esconder();
    }
    public static PanelCalificacion GetInstance()
    {
        return instance;
    }
    public static void Mostrar()
    {
        JugadorManager.GetInstance().MostrarPuntero(true);
        instance.gameObject.SetActive(true);
    }

    public static void Esconder()
    {
        JugadorManager.GetInstance().MostrarPuntero(false);
        instance.gameObject.SetActive(false);
        instance.OnClose();
    }

    public static void Mostrar(List<string> fallos, int puntuacion)
    {
        JugadorManager.GetInstance().MostrarPuntero(true);
        instance.intentos++;
        instance.gameObject.SetActive(true);
        instance.ActualizarCirculoPuntuacion(puntuacion);
        instance.capitulo.text = "CAPÍTULO " + Utils.ToRomanNumber(GameManager.GetInstance().nivel.numero);
        instance.fallos.text = Utils.FallosToString(fallos);
        if (puntuacion >= 50) {
            instance.botonExtra.gameObject.SetActive(false);
            instance.boton.GetComponentInChildren<TMP_Text>().text = "Continuar";
            instance.descripcion.text = "Prueba superada";
            instance.intentos = 0;
            instance.boton.onClick.RemoveAllListeners();
            instance.boton.onClick.AddListener(() => {
                instance.gameObject.SetActive(false);
                GameManager.MostrarSiguientePrueba();
            });
            
        }
        if (puntuacion < 50 && intentosMax > instance.intentos) {
            instance.botonExtra.gameObject.SetActive(false);
            instance.boton.GetComponentInChildren<TMP_Text>().text = "Reintentar";
            instance.descripcion.text = "Prueba fallada";
            instance.boton.onClick.RemoveAllListeners();
            instance.boton.onClick.AddListener(() => {
                instance.gameObject.SetActive(false);
                GameManager.RepetirPrueba();
            });
               }
        if (puntuacion < 50 && intentosMax == instance.intentos)
        {
            instance.botonExtra.gameObject.SetActive(true);
            
            instance.descripcion.text = "Nivel fallado";
            instance.intentos = 0;
            instance.boton.GetComponentInChildren<TMP_Text>().text = "Reintentar nivel";
            instance.boton.onClick.RemoveAllListeners();
            instance.boton.onClick.AddListener(() => {
                Esconder();
                GameManager.GetInstance().ReintentarNivel();
            });
            
            instance.botonExtra.onClick.RemoveAllListeners();
            instance.botonExtra.GetComponentInChildren<TMP_Text>().text = "Menú";
            instance.botonExtra.onClick.AddListener(() => {
                instance.gameObject.SetActive(false);
                GameManager.GetInstance().VolverMenu();

            });
        }

    }
    private void ActualizarCirculoPuntuacion(int puntuacion) {
        instance.puntuacion.text = puntuacion + " %";
        instance.circuloPuntuacion.fillAmount = puntuacion / 100.0f;
    }
}
