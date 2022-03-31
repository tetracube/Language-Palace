using TMPro;
using UnityEngine;

public class PanelListaObjetos : MonoBehaviour
{
    private static PanelListaObjetos instance;

    [SerializeField]
    private TMP_Text palabrasTexto;
    public static PanelListaObjetos GetInstance()
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
    private void OnEnable()
    {
        transform.localPosition = new Vector2(0, -Screen.height);
        transform.LeanMoveLocalY(0, 0.4f).setEaseOutBack();
    }

    public static void Esconder()
    {
        instance.gameObject.SetActive(false);
    }

    public static void ActualizarPalabras(string palabras)
    {

        instance.palabrasTexto.text = palabras;

    }

}
