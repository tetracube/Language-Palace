using UnityEngine;

public class Pista : MonoBehaviour
{
    [SerializeField]
    private GameObject representacionFlecha;

    private bool activar = false;
    public string nombre { get { return name; } }

    private void Start()
    {
        representacionFlecha.SetActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        representacionFlecha.SetActive(activar);
    }
    private void OnTriggerExit(Collider other)
    {
        representacionFlecha.SetActive(false);
    }
    public void ActivarPista(bool activar){
        this.activar = activar;
        representacionFlecha.SetActive(activar);
    }
}
