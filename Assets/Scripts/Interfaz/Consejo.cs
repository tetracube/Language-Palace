using UnityEngine;

public class Consejo : MonoBehaviour
{
    private static Consejo instance;

    void Awake()
    {
        instance = this;
        Esconder();
    }

   public static void Mostrar()
    {
        instance.gameObject.SetActive(true);
    }

    public static void Esconder()
    {
        instance.gameObject.SetActive(false);
    }
}
