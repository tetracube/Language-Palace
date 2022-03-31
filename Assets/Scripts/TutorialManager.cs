using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private GameObject tutorial;
    private static TutorialManager instance;
    private bool ayudaActivada = false;
    private TutorialManager() { }

    public static TutorialManager GetInstance()
    {
        return instance;
    }

    void Awake()
    {
        instance = this;
    }


    public void ActivarAyuda(bool activar)
    {
        ayudaActivada = activar;

        foreach (Pista pista in tutorial.GetComponentsInChildren<Pista>())
        {
            pista.ActivarPista(activar);
        }
    }
    public void DesactivarPista(string nombre) {

        foreach (Pista pista in tutorial.GetComponentsInChildren<Pista>())
        {
            if (pista.nombre.Equals(nombre))
            {
                pista.ActivarPista(false);
            }
        }
    }
    public bool AyudaActivada() { return ayudaActivada; }
    public void ReactivarAyuda() {
        ActivarAyuda(ayudaActivada);
    }

}
