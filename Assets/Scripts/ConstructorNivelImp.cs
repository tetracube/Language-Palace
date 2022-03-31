using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConstructorNivelImp : MonoBehaviour, ConstructorNivel
{
    private Nivel nivel;


    public void Recomponer() {
        nivel = new Nivel();
    }
    public void AñadirPuertas(bool añadir) {

        var puerta = Resources.Load<GameObject>("puerta");
        var pared = Resources.Load<GameObject>("pared");
        var puertas = new List<GameObject>(GameObject.FindGameObjectsWithTag("Puerta"));
        if (añadir) {
            puertas.ForEach((x) => {
                x.GetComponent<MeshFilter>().mesh = puerta.GetComponent<MeshFilter>().sharedMesh;
                x.AddComponent<MeshCollider>();
                x.GetComponentInChildren<SpriteRenderer>().sprite = puerta.GetComponentInChildren<SpriteRenderer>().sprite;
            }); 
        }else
        {
            puertas.ForEach((x) => { 
                x.GetComponent<MeshFilter>().mesh = pared.GetComponent<MeshFilter>().sharedMesh;
                x.AddComponent<MeshCollider>();
                x.GetComponentInChildren<SpriteRenderer>().sprite = pared.GetComponentInChildren<SpriteRenderer>().sprite;
            });
        }
    }
    public void ColocarObjetos(int nivel, Dictionary<string, string> L1L2) {
        string nombreObjetos = "";
        int cantidadObjetos = 0;
        string jsonFile = Resources.Load<TextAsset>("Niveles").text;
        Dictionary<string, List<Vector3>> localizacionesPorNivel = JsonConvert.DeserializeObject<Dictionary<string, List<Vector3>>>(jsonFile);
        List<Vector3> localizaciones = localizacionesPorNivel[nivel.ToString()];

        GameObject[] objetosInteractivos = Resources.LoadAll("Objetos").Cast<GameObject>().ToArray();
        var claves = L1L2.Keys.ToList();
        for (int i = 0; i < localizaciones.Count; i++)
        {
            Quaternion rotacion = objetosInteractivos[i].transform.rotation;
            if (nivel == 1 && objetosInteractivos[i].name.Equals("01-Nave")) { rotacion.x = -1f;}
            GameObject objeto = Instantiate(objetosInteractivos[i], localizaciones[i], rotacion);
            objeto.name = objeto.name.Split('-')[1].Split('(')[0];
            nombreObjetos += objeto.name + "\n";
            cantidadObjetos++;
            objeto.AddComponent<Objeto>();
            int random = Utils.RandomInt(claves.Count);
            objeto.GetComponent<Objeto>().cadena = claves[random] + "    -   " + L1L2[claves[random]];
            claves.RemoveAt(random);
        }
        this.nivel.objetos = nombreObjetos;
        this.nivel.cantidadObjetos = cantidadObjetos;
        this.nivel.numero = nivel;
    }
    public void GenerarPreguntas(Dictionary<string, string> L1L2, List<EstrategiaPrueba> tipoPruebas) {

        List <Cuestion> cuesionesGeneradas = new List<Cuestion>();
        GeneradorPruebas generadorPruebas = new GeneradorPruebas();
        generadorPruebas.L1L2 = L1L2;

        foreach (EstrategiaPrueba estrategiaPrueba in tipoPruebas)
        {
            generadorPruebas.estrategiaPrueba = estrategiaPrueba;
            generadorPruebas.GeneraCuestiones();
            generadorPruebas.CargarCuestiones();
            var prueba = generadorPruebas.GenerarPrueba(5);
            cuesionesGeneradas.AddRange(prueba.cuestiones);
        }
        nivel.cuestiones = cuesionesGeneradas;
    }

    public void AñadirTemporizador() {
        nivel.temporizador = GameManager.GetInstance().GetComponent<Temporizador>();
    }

    public Nivel ObtenerResultado() {
        return nivel;
    }
}
