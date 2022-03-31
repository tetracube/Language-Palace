using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
   {

    public Dictionary<string, string> L1L2;

    private List<string> objetosEncontrados = new List<string>();
    private Dictionary<string, string> palabrasEncontradas = new Dictionary<string, string>();
    public Nivel nivel { get; private set; }
    private string objetosEnNivel;

    private string tipoPruebaActual;



    [SerializeField]
    private GameObject pantallaDeCarga;

    private static GameManager instance;
    private GameManager() { }

    public static GameManager GetInstance() {
        return instance;
    }


    void Awake()
   
    {
        instance = this;
    }

    void Start()
    {
        IniciarNivel();
        MostarIntroduccion();

       
    }
  
    private static void MostarIntroduccion() {
        HUD.Esconder();
        PanelIntroduccion.Mostrar(PanelPalabras.nivelElegido);
    }
    public void ReintentarNivel()
    {
        JugadorManager.GetInstance().PosicionarJugador();

        GameObject[] objetosAntiguoNivel = GameObject.FindGameObjectsWithTag("Interactivo");
        foreach (GameObject objetoAntiguoNivel in objetosAntiguoNivel)
            objetoAntiguoNivel.GetComponent<Objeto>().encontrado = 0;

        palabrasEncontradas = new Dictionary<string, string>();
        objetosEncontrados = new List<string>();
        objetosEnNivel = instance.nivel.objetos;
        nivel.temporizador.Iniciar();

        TutorialManager.GetInstance().ReactivarAyuda();
        HUD.Mostrar();
        PanelListaObjetos.ActualizarPalabras(instance.nivel.objetos);
        HUD.ActualizarContador(instance.objetosEncontrados.Count);


        PanelTest.Esconder();
        DataBase.InsertarEnNivel(nivel.numero, -1, instance.L1L2);
        JugadorManager.GetInstance().CongelarJugador(false);

    }
    private static void IniciarNivel() {
        JugadorManager.GetInstance().PosicionarJugador();
        JugadorManager.GetInstance().ActualizarSensibilidad();


        ConstructorNivel constructorNivel = new ConstructorNivelImp();
        Director director = new Director(constructorNivel);
        instance.L1L2 = PanelPalabras.L1L2; 
        switch (PanelPalabras.nivelElegido)
        {
            case 1:
                instance.nivel = director.GeneraNivelUno(instance.L1L2);
                break;
            case 2:
                instance.nivel = director.GeneraNivelDos(instance.L1L2);
                break;
            case 3:
                break;
            case 4:
                break;
        }
        DataBase.InsertarEnNivel(instance.nivel.numero, -1, instance.L1L2);
        instance.objetosEnNivel = instance.nivel.objetos;
        PanelListaObjetos.ActualizarPalabras(instance.nivel.objetos);
        HUD.ActualizarContador(instance.objetosEncontrados.Count);

    }
    
    public static void NotificarObjetoEncontrado(string nombreObjeto, string L1, string L2) {
        try
        {
            instance.palabrasEncontradas.Add(L1, L2);
            instance.objetosEncontrados.Add(nombreObjeto);
            instance.objetosEnNivel = Utils.TacharPalabra(nombreObjeto, instance.objetosEnNivel);
            PanelListaObjetos.ActualizarPalabras(instance.objetosEnNivel);
            HUD.ActualizarContador(instance.objetosEncontrados.Count);
            if (TutorialManager.GetInstance().AyudaActivada()) TutorialManager.GetInstance().DesactivarPista(nombreObjeto);

            if (instance.objetosEncontrados.Count == instance.nivel.cantidadObjetos / 2)
            {
                JugadorManager.GetInstance().CongelarJugador(true);
                var prueba = GenerarPruebaIntermedia(instance.palabrasEncontradas, new EstrategiaPruebaVisualA());
                PanelTest.Mostrar(prueba.pregunta, prueba.cuestiones, false);
                instance.tipoPruebaActual = "PruebaVisualAIntermedia";
                InsertarEnBDPruebaGenerada();

            }
            if (instance.objetosEncontrados.Count == instance.nivel.cantidadObjetos)
            {
                JugadorManager.GetInstance().CongelarJugador(true);
                PanelTest.Mostrar("Elige la opción correcta entre las siguientes opciones.", instance.nivel.cuestiones, true);
                instance.tipoPruebaActual = "PruebaFinal";
                InsertarEnBDPruebaGenerada();
            }
        }
        catch (System.ArgumentException) { }
    }
    private static Prueba GenerarPruebaIntermedia(Dictionary<string, string> L1L2, EstrategiaPrueba prueba)
    {
        GeneradorPruebas generadorPruebas = new GeneradorPruebas();
        generadorPruebas.L1L2 = L1L2;
        generadorPruebas.estrategiaPrueba = prueba;
        generadorPruebas.GeneraCuestiones();
        generadorPruebas.CargarCuestiones();
        return generadorPruebas.GenerarPrueba(5);
    }



    public static void MostrarSiguientePrueba() {
        switch (instance.tipoPruebaActual){

            case "PruebaVisualAIntermedia":        
                var prueba = GenerarPruebaIntermedia(instance.palabrasEncontradas, new EstrategiaPruebaVisualB());
                PanelTest.Mostrar(prueba.pregunta, prueba.cuestiones, false);
                instance.tipoPruebaActual = "PruebaVisualBIntermedia";
                InsertarEnBDPruebaGenerada();
                break;

            case "PruebaVisualBIntermedia":        
                PanelTest.Esconder();
                JugadorManager.GetInstance().CongelarJugador(false);
                HUD.Mostrar();
                break;

            case "PruebaFinal":
                PanelTest.Esconder();
                DataBase.ActualizarEnNivel((int) instance.nivel.temporizador.Pausar());
                PanelFinNivel.Mostrar();
                break;
        }

    }
    public static void RepetirPrueba() {
        Prueba prueba;
        switch (instance.tipoPruebaActual) {
            case "PruebaVisualAIntermedia":
                prueba = GenerarPruebaIntermedia(instance.palabrasEncontradas, new EstrategiaPruebaVisualA());
                InsertarEnBDPruebaGenerada();
                PanelTest.Mostrar(prueba.pregunta, prueba.cuestiones, false);
                break;
            case "PruebaVisualBIntermedia":
                prueba = GenerarPruebaIntermedia(instance.palabrasEncontradas, new EstrategiaPruebaVisualB());
                InsertarEnBDPruebaGenerada();
                PanelTest.Mostrar(prueba.pregunta, prueba.cuestiones, false);
                break;
            default:
                PanelTest.Esconder();
                JugadorManager.GetInstance().CongelarJugador(false);
                break;

        }
    }
    public static bool IsShowingUI() {
        return PanelObjeto.GetInstance().gameObject.activeSelf || PanelTest.GetInstance().gameObject.activeSelf || PanelCalificacion.GetInstance().gameObject.activeSelf || PanelAjustes.GetInstance().gameObject.activeSelf || PanelFinNivel.GetInstance().gameObject.activeSelf || PanelListaObjetos.GetInstance().gameObject.activeSelf;
    }
    private static void InsertarEnBDPruebaGenerada() {
        DataBase.InsertarEnPrueba(instance.tipoPruebaActual, -1, -1, -1, new List<string>(), instance.palabrasEncontradas.Keys.ToList()); }

    public void VolverMenu() {
        AsyncOperation operacion = SceneManager.LoadSceneAsync("PantallaPrincipal");
        StartCoroutine(CargarEscena(operacion));
    }
    public void SiguienteNivel()
    {
        AsyncOperation operacion = SceneManager.LoadSceneAsync("PantallaPrincipal");
        operacion.completed += (x) => { MenuPrincipal.GetInstance().IniciarContinuar(); };
        StartCoroutine(CargarEscena(operacion));
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
}
 