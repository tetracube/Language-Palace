using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelAjustes : MenuTransicion
{
    [SerializeField]
    private TMPro.TMP_Text resolucionesTexto;
    [SerializeField]
    private TMPro.TMP_Text pantallaCompletaTexto;
    [SerializeField]
    private UnityEngine.UI.Slider sensibilidadSlider;
    private Resolution[] resoluciones;
    private List<string> opciones;
    public static float sensibilidad { get; private set; } = 5f;
    private int resolucionElegida  { get {return opciones.IndexOf(Screen.width + " X " + Screen.height); } set { } }
    private int pantallaCompletaElegida { get { return Screen.fullScreen ? 1 : 2; } }

    private static PanelAjustes instance;
    public static PanelAjustes GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        instance.gameObject.SetActive(false);
        ResolucionesDisponibles();
        IniciarConfiguracion();
        
    }
    private void OnEnable()
    {
        if(GameManager.GetInstance() != null  && GameManager.GetInstance().nivel != null) GameManager.GetInstance().nivel.temporizador.Pausar();
        FadeIn();
    }
    private void OnDisable()
    {
        if (GameManager.GetInstance() != null && GameManager.GetInstance().nivel != null) GameManager.GetInstance().nivel.temporizador.Reanudar();
    }
    public static void Mostrar()
    {
        instance.gameObject.SetActive(true);
    }
    public void CambiarResolucion(int i) {
       
        var indiceActual = opciones.IndexOf(resolucionesTexto.text);
        indiceActual = (indiceActual + i) % opciones.Count;       
        if (indiceActual == -1) indiceActual = opciones.Count - 1;
        resolucionesTexto.text = opciones[indiceActual];

        Resolution resolucion = resoluciones[indiceActual];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
    }

    public void PantallaCompleta(int i) {
        pantallaCompletaTexto.pageToDisplay = (pantallaCompletaTexto.pageToDisplay + i) % 2;
        if (pantallaCompletaTexto.pageToDisplay == 0) pantallaCompletaTexto.pageToDisplay = 2;
        Screen.fullScreen = pantallaCompletaTexto.pageToDisplay == 1;
    }


    private void ResolucionesDisponibles() {
        resoluciones = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();
        opciones = new List<string>();
        for (int i = 0; i < resoluciones.Length; i++)
        {
            opciones.Add(resoluciones[i].width + " X " + resoluciones[i].height);
            if (resoluciones[i].width == Screen.width && resoluciones[i].height == Screen.height)
            {
                resolucionesTexto.text = opciones[i];
            }
        }
    }

    public void Sensibilidad(float inputSensibilidad) {

        sensibilidad = inputSensibilidad * 10;
    }
    private void IniciarConfiguracion() {
        sensibilidadSlider.value = sensibilidad / 10;
        resolucionesTexto.text = opciones[resolucionElegida];
        pantallaCompletaTexto.pageToDisplay = pantallaCompletaElegida;


    }
}
