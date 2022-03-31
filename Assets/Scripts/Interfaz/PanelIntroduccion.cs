using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PanelIntroduccion : MonoBehaviour
{

    private static PanelIntroduccion instance;
    private Dictionary <int, string[]> dialogo = new Dictionary<int, string[]>();
    int i = 0;
    int nivel = 0;

    [SerializeField]
    private TMP_Text dialogoTexto;
    [SerializeField]
    private GameObject tutorial;

    public static PanelIntroduccion GetInstance()
    {
        return instance;
    }
    private void Awake()
    {
        dialogo.Add(1, new string[] { "Forastero que habéis decidido cruzar las puertas de este palacio...", "Os propongo una prueba. Hasta este instante habíais usado solo vuestra lengua. ¿Seréis capaz de usar la mía? Se encuentran escondidas 10 palabras en objetos ajenos a esta época. Encontradlas y demostrad vuestras capacidades de aprendizaje.", "[Consejo: para memorizar la lista de palabras, intenta recordar dónde exactamente las encontraste.]", "¿Estáis preparado?","" });
        dialogo.Add(2, new string[] { "Enhorabuena forastero, habéis logrado superar la prueba. Mas aún se hallan más...", "En esta ocasión, se encuentran escondidas 20 palabras en objetos ajenos a esta época. Encontradlas y demostrad vuestras capacidades de aprendizaje.", "¿Estáis preparado?","" });

        instance = this;

    }

    public static void Mostrar(int nivel)
    {
        JugadorManager.GetInstance().CongelarJugador(true);
        instance.i = 0;
        instance.tutorial.SetActive(false);
        instance.nivel = nivel;

        instance.dialogoTexto.text = instance.dialogo[nivel][instance.i++];
        instance.gameObject.SetActive(true);
    }

    public void Esconder()
    {
        JugadorManager.GetInstance().MostrarPuntero(false);
        instance.gameObject.SetActive(false);
        HUD.Mostrar();
        JugadorManager.GetInstance().CongelarJugador(false);
        GameManager.GetInstance().nivel.temporizador.Iniciar();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            try
            {
                dialogoTexto.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                LeanTween.scale(dialogoTexto.gameObject, Vector3.one, 0.5f).setEaseInSine();
                dialogoTexto.text = instance.dialogo[nivel][i++];
               

                if (i >= dialogo[nivel].Length)
                {
                    if (nivel == 1)
                    {
                        JugadorManager.GetInstance().MostrarPuntero(true);
                        tutorial.GetComponent<UnityEngine.UI.HorizontalLayoutGroup>().transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                        LeanTween.scale(tutorial.GetComponent<UnityEngine.UI.HorizontalLayoutGroup>().gameObject, Vector3.one, 0.5f).setEaseInSine();
                        tutorial.SetActive(true);
                    }
                    else
                    {
                        Esconder();
                    }

                }
            }
            catch (System.IndexOutOfRangeException) { };
        }
            
    }
    private void OnEnable()
    {
        dialogoTexto.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        LeanTween.scale(dialogoTexto.gameObject, Vector3.one, 0.5f).setEaseInSine();

    }
}
