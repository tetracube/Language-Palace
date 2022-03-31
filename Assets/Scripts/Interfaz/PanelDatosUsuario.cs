using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelDatosUsuario : MenuTransicion
{
    private static PanelDatosUsuario instance;

    [SerializeField]
    private Button aceptar;

    [SerializeField]
    private TMP_InputField apodo;
    [SerializeField]
    private TMP_InputField edad;
    [SerializeField]
    private TMP_InputField L1;
    [SerializeField]
    private TMP_InputField L2;
    [SerializeField]
    private TMP_InputField nacionalidad;
    [SerializeField]
    private TMP_InputField pais;
    [SerializeField]
    private TMP_Dropdown genero;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Esconder();

    }
    public static PanelDatosUsuario GetInstance()
    {
        return instance;
    }

    public static void Mostrar()
    {
        instance.ActivarAceptar();
        instance.gameObject.SetActive(true);
      
    }

    public static void Esconder()
    {
        instance.LimpiarCampos();
        instance.ActivarAceptar();
        instance.FadeOut(() =>  instance.gameObject.SetActive(false));
       // instance.gameObject.SetActive(false);
    }
    private void LimpiarCampos() {

        List<TMP_InputField> campos = new List<TMP_InputField>(GetComponentsInChildren<TMP_InputField>());
        campos.ForEach(x => x.text = string.Empty);
    }
    public void ActivarAceptar()
    {
        List<TMP_InputField> campos = new List<TMPro.TMP_InputField>(GetComponentsInChildren<TMP_InputField>());
        aceptar.interactable = !campos.Any(x => string.IsNullOrEmpty(x.text));
    }
    public void Aceptar() {       
        PanelPalabras.pantallaAnterior = Pantalla.Personalizacion;
        PanelPalabras.MostrarEstatico(10,1);// Al crear una partida, el único nivel disponible es el primero
    }
    public void Retroceder()
    {
        Esconder();
        MenuPrincipal.Mostrar();
    }
    public void CrearNuevaPartida() {
        DataBase.InsertarEnPartida(apodo.text, int.Parse(edad.text), genero.options[genero.value].text, L1.text, L2.text, nacionalidad.text, pais.text);
    }
}

