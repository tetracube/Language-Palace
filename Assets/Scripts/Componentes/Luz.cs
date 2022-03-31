
using UnityEngine;

public class Luz : MonoBehaviour
{
    [SerializeField]
    private Light[] luces;

    private void Start()
    {
        foreach (Light luz in luces) { luz.enabled = false; }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (Light luz in luces) { luz.enabled = true; }
    }
    private void OnTriggerExit(Collider other)
    {
        foreach (Light luz in luces) { luz.enabled = false; }
    }
}
