using UnityEngine;

public class Objeto : MonoBehaviour
{
    public string cadena;

    private bool presionadoE = false;
    private bool encontradoPrimeraVez { get { return encontrado == 1; } }
    public int encontrado = 0;
    private GameObject personaje { get { return JugadorManager.GetInstance().personaje; } }
     public void OnMouseEnter()
    {        
        if (Vector3.Distance(personaje.transform.position, transform.position) <= 2 && !GameManager.IsShowingUI()) {
                Consejo.Mostrar();
        }
       
    }
    public void OnMouseOver()
    {
        if (Vector3.Distance(personaje.transform.position, transform.position) > 2.5f)
        {
            Consejo.Esconder();
        }
        else
        {
            if (!GameManager.IsShowingUI()) {
                Consejo.Mostrar(); }
            
            InteractPanelObjeto();
        }
    }

    public void OnMouseExit()
    {
        Consejo.Esconder();
        if (PanelObjeto.GetInstance().gameObject.activeSelf) {
            PanelObjeto.Esconder();
            if (encontradoPrimeraVez) GameManager.NotificarObjetoEncontrado(name, cadena.Split('-')[0].Trim(), cadena.Split('-')[1].Trim());
        }
    }

    private void InteractPanelObjeto() {
        if (!presionadoE)
        {
            if (Input.GetKeyDown("e"))
            {
                presionadoE = true;
                Consejo.Esconder();
                PanelObjeto.Mostrar(cadena);
                encontrado++;               
            }
        }
        else
        {
            if (Input.GetKeyDown("e") || Input.GetKey(KeyCode.Escape))
            {
                presionadoE = false;
                PanelObjeto.Esconder();                
                if (encontradoPrimeraVez) GameManager.NotificarObjetoEncontrado(name, cadena.Split('-')[0].Trim(), cadena.Split('-')[1].Trim());

            }
        }
    }
}
