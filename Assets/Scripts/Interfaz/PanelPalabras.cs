using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PanelPalabras : MenuTransicion
{
    private static PanelPalabras instance;
    private string[] L1;
    private string[] L2;
    private int paginaActual = 0;
    private const int PALABRAS_PAGINA = 5;
    private int paginasTotales;

    public static Dictionary<string, string> L1L2 { get; private set; }

    [SerializeField]
    private GameObject contendorL1;
    [SerializeField]
    private GameObject contendorL2;
    [SerializeField]
    private Button anterior;
    [SerializeField]
    private Button posterior;
    [SerializeField]
    private Button aceptar;
    [SerializeField]
    private TMP_Text error;

    [SerializeField]
    private GameObject pantallaDeCarga;

    public static Pantalla pantallaAnterior;
    public static int nivelElegido;
    void Awake() 
    { 
        instance = this; 
    }

    void Start()
    {
        Esconder();
    }
    public static PanelPalabras GetInstance() { return instance; }

    public static void MostrarEstatico(int cantidadPalabras, int nivel)
    {
        instance.error.text = "";
        nivelElegido = nivel;
        instance.paginasTotales = cantidadPalabras / PALABRAS_PAGINA;
        instance.L1 = new string[cantidadPalabras];
        instance.L2 = new string[cantidadPalabras];
        instance.paginaActual = 0;
        instance.anterior.interactable = false;
        instance.posterior.interactable = true;
        instance.aceptar.interactable = false;

        instance.gameObject.SetActive(true);
    }

    public void Mostrar(int cantidadPalabras)
    {
        instance.error.text = "";
        cantidadPalabras = 10;
        instance.paginasTotales = cantidadPalabras / PALABRAS_PAGINA;
        instance.L1 = new string[cantidadPalabras];
        instance.L2 = new string[cantidadPalabras];
        instance.paginaActual = 0;
        instance.anterior.interactable = false;
        instance.posterior.interactable = true;
        instance.aceptar.interactable = false;

        instance.gameObject.SetActive(true);
    }
    public static void Esconder()
    {
        instance.LimpiarCampos();
        instance.FadeOut(() => instance.gameObject.SetActive(false));
        //instance.gameObject.SetActive(false);
    }
    public void ActivarAceptar(TMP_InputField input)
    {
        aceptar.interactable = !string.IsNullOrEmpty(input.text) && Utils.ChcekIfFull(L1) && Utils.ChcekIfFull(L2);
    }

    public void RecogerValoresIntroducidos(TMP_InputField palabra) {
        if (palabra.name.Equals("InputFieldL1")) {
            L1[paginaActual * PALABRAS_PAGINA + palabra.transform.GetSiblingIndex() - 1] = palabra.text;

        }
        if (palabra.name.Equals("InputFieldL2")) {
            L2[paginaActual * PALABRAS_PAGINA + palabra.transform.GetSiblingIndex() - 1] = palabra.text;
        }

    }

    public void MostrarPaginaAnterior()
    {
        paginaActual--;
        ColocarValoresIntroducidos();
        ActualizarBotonesProgreso();

    }
    public void MostrarPaginaPosterior()
    {
        paginaActual++;
        ColocarValoresIntroducidos();
        ActualizarBotonesProgreso();
    }
    private void ActualizarBotonesProgreso() {

        if (paginaActual == 0)
        {
            anterior.interactable = false;
            posterior.interactable = true;
        }
        if (paginaActual == paginasTotales - 1)
        {
            posterior.interactable = false;
            anterior.interactable = true;
        }
        if (paginaActual != paginasTotales - 1 && paginaActual != 0)
        {
            posterior.interactable = true;
            anterior.interactable = true;
        }

    }
    private void ColocarValoresIntroducidos() {
        var hijosContenedorL1 = contendorL1.GetComponentsInChildren<TMP_InputField>();
        var hijosContenedorL2 = contendorL2.GetComponentsInChildren<TMP_InputField>();
        for (int i = 0; i < hijosContenedorL1.Length; i++) {
            hijosContenedorL1[i].text = L1[paginaActual * PALABRAS_PAGINA + i];
            hijosContenedorL2[i].text = L2[paginaActual * PALABRAS_PAGINA + i];
        }

    }

    public void CargarJuego()
    {
       
        try {
            error.text = "";
            L1L2 = Utils.ConstruirDiccionario(L1, L2);
            if (pantallaAnterior.Equals(Pantalla.Personalizacion)) PanelDatosUsuario.GetInstance().CrearNuevaPartida();
            AsyncOperation operacion = SceneManager.LoadSceneAsync("Main");
            StartCoroutine(CargarEscena(operacion));
            
        }
        catch (ArgumentException e)
        {
            error.text = "No puedes introducir palabras repetidas:" + e.Message.Split(':')[1];
        }

    }
    private IEnumerator CargarEscena(AsyncOperation operacionAsincrona)
    {
        operacionAsincrona.allowSceneActivation = false;

        Instantiate(pantallaDeCarga, transform.parent);
        yield return null;

        while (!operacionAsincrona.isDone)
        {
            if (operacionAsincrona.progress >= 0.9f)
            {
                operacionAsincrona.allowSceneActivation = true;
            }
            yield return new WaitForFixedUpdate();
        }


    }
    private void LimpiarCampos()
    {

        List<TMPro.TMP_InputField> campos = new List<TMPro.TMP_InputField>(GetComponentsInChildren<TMPro.TMP_InputField>());
        campos.ForEach(x => x.text = string.Empty);
    }
    public void Retroceder() {
        switch (pantallaAnterior) {
            case Pantalla.MenuPrincipal:
                Esconder();
                MenuPrincipal.Mostrar();
                break;
            case Pantalla.Personalizacion:
                Esconder();
                PanelDatosUsuario.Mostrar();
                break;
            case Pantalla.SelectorNiveles:
                Esconder();
                PanelSelectorNiveles.Mostrar();
                break;
            }
    
    
   }
}
public enum Pantalla
{
    MenuPrincipal,
    SelectorNiveles,
    Personalizacion
}
