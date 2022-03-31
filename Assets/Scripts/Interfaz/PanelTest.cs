using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelTest : Transicion
{
    private static PanelTest instance;
    public string pregunta { get; set; }
    public List<Cuestion> cuestiones { get; private set; }

    [SerializeField]
    private Button anterior;
    [SerializeField]
    private Button posterior;
    [SerializeField]
    private List<Button> buttons;
    [SerializeField]
    private Sprite spriteSeleccionado;
    [SerializeField]
    private Sprite spriteNoSeleccionado;

    private int [] opcionElegida;
    private int preguntaActual;
    private int preguntaAnterior;
    private bool pruebaFinal;
    private List<Button> botonesActivos;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Esconder();
    }
    public static PanelTest GetInstance()
    {
        return instance;
    }

    public static void Mostrar()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        instance.gameObject.SetActive(true);
    }

    public static void Esconder()
    {
        Cursor.lockState = CursorLockMode.Locked;
        JugadorManager.GetInstance().MostrarPuntero(false);
        instance.gameObject.SetActive(false);
    }

    public static void Mostrar(string pregunta, List <Cuestion> cuesiones, bool pruebaFinal)
    {
        Cursor.lockState = CursorLockMode.None;
        HUD.Esconder();
        Cursor.visible = true;
        instance.pruebaFinal = pruebaFinal;
        instance.buttons.ForEach((x) => { x.gameObject.SetActive(pruebaFinal); });
        instance.preguntaActual = 2; 

        instance.botonesActivos = new List<Button>();
        int i = 0;
        foreach (Button boton in instance.GetComponentsInChildren<Button>())
        {
            if (boton.name.Equals("BotonProgreso"))
            {
                instance.botonesActivos.Add(boton);
                int j = i++;
                boton.onClick.RemoveAllListeners();
                boton.onClick.AddListener(() => { instance.MostrarPregunta(j); });

            };
        }

        instance.CambiarSpriteTodos();
        instance.gameObject.SetActive(true);
        instance.cuestiones = cuesiones;
        instance.opcionElegida = Enumerable.Repeat(-1, cuesiones.Count).ToArray();

        foreach (Transform hijo in instance.transform)
        {
            if (hijo.name.Equals("Pregunta"))
            {
                hijo.GetComponentInChildren<TMP_Text>().text = pregunta;
            }
        }
        instance.MostrarPregunta(0);

    }

    public void MostrarPregunta(int numero) {
        preguntaAnterior = preguntaActual;
        preguntaActual = numero;
        CambiarSprite();
        foreach (Transform hijo in instance.transform)
        {
            switch (hijo.name)
            {
                case "Progreso":
                    if (numero >= cuestiones.Count - 1)
                    {
                        posterior.enabled = false;
                        anterior.enabled = true;
                    }
                    if (numero != cuestiones.Count - 1 && numero != 0) {
                        posterior.enabled = true;
                        anterior.enabled = true;
                    }
                    if (numero <= 0) {
                        posterior.enabled = true;
                        anterior.enabled = false;
                    }
                                
                    break;
                case "Palabra":
                    hijo.GetComponentInChildren<TMP_Text>().text = cuestiones[numero].enunciado;
                    break;
                case "Opciones":
                    var opciones = hijo.gameObject.GetComponentsInChildren<Toggle>();
                    for (int i = 0; i < opciones.Length; i++) {
                        opciones[i].GetComponentInChildren<TMP_Text>().text = cuestiones[numero].opciones[i];
                        opciones[i].isOn = instance.opcionElegida[numero] == i;
                    }
                    break;
                case "Finalizar":
                    break;
            }
        }
        
    }

    public void MostrarPreguntaAnterior()
    {
        MostrarPregunta(preguntaActual - 1);
    }
    public void MostrarPreguntaPosterior()
    {
        MostrarPregunta(preguntaActual + 1);
    }


    public void MarcarOpcion(Toggle toggle) {
       if(toggle.isOn) opcionElegida[preguntaActual] = toggle.transform.GetSiblingIndex();

        if (opcionElegida[preguntaActual] == toggle.transform.GetSiblingIndex() && !toggle.isOn) {
            opcionElegida[preguntaActual] = -1;
        }

    }

    public void Finalizar() {
        List<string> fallos = CalcularFallos();
        int puntuacion =  CalcularPuntuacion(fallos.Count);
        if (pruebaFinal) { PanelCalificacion.intentosMax = 1; } else { PanelCalificacion.intentosMax = 3; }
        DataBase.ActualizarEnPrueba(puntuacion, fallos.Count, cuestiones.Count - fallos.Count, fallos);
        instance.gameObject.SetActive(false);
        PanelCalificacion.Mostrar(new HashSet<string>(fallos).ToList(), puntuacion);
        

    }
      private int CalcularPuntuacion(int fallos)
    {
        double aciertos = cuestiones.Count - fallos;
       
        return (int) (aciertos / cuestiones.Count  * 100) ;
    }
   
    private List <string> CalcularFallos()
    {
        List<string> fallos = new List<string>();
     
        for (int i = 0; i < cuestiones.Count; i++)
        {
            if (opcionElegida[i] == -1 || cuestiones[i].respuesta != cuestiones[i].opciones[opcionElegida[i]])

            {
                var palabraFallada = cuestiones[i].enunciado;
                palabraFallada = palabraFallada.Replace("*", cuestiones[i].respuesta);
                fallos.Add(palabraFallada);
            }
        }
        return fallos;
    }

    private void CambiarSprite() {
        botonesActivos[preguntaActual].image.sprite = spriteSeleccionado;
        botonesActivos[preguntaAnterior].image.sprite = spriteNoSeleccionado;

    }
    private void CambiarSpriteTodos()
    {
        botonesActivos.ForEach((x)=> { x.image.sprite = spriteNoSeleccionado; });

    }

}
