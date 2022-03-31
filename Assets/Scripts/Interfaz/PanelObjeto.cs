using TMPro;
using UnityEngine;

public class PanelObjeto : MonoBehaviour
{
    private static PanelObjeto instance;

    public static PanelObjeto GetInstance()
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
        instance.gameObject.SetActive(true);
    }

    public static void Esconder()
    {
        LeanTween.scale(instance.gameObject, Vector3.zero, 0.2f).setEaseInBack().setOnComplete(() => instance.gameObject.SetActive(false));
        JugadorManager.GetInstance().CongelarJugador(false);
    }

    public static void Mostrar(string mensaje)
    {
        JugadorManager.GetInstance().CongelarJugador(true);
        instance.gameObject.SetActive(true);
        instance.gameObject.GetComponentInChildren<TMP_Text>().text = mensaje;


    }

    private void OnEnable()
    {
        instance.transform.localScale = Vector3.zero;
        LeanTween.scale(instance.gameObject, Vector3.one, 0.2f).setEaseInOutBack();
    }

}
