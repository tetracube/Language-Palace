using Newtonsoft.Json;
using System.Collections.Generic;


public class GeneradorPruebas
{
    public EstrategiaPrueba estrategiaPrueba { get; set; }
    public Dictionary<string, string> L1L2 { get; set; }
    private Prueba prueba = new Prueba();
    private const int NUM_OPCIONES = 3;
      


    public void GeneraCuestiones()
    {
        Prueba pruebaGenerada = new Prueba();
        List<Cuestion> cuestionesGeneradas = new List<Cuestion>();

        foreach (var pareja in L1L2)
        {
            cuestionesGeneradas.Add(estrategiaPrueba.GeneraCuestion(pareja.Key, pareja.Value, NUM_OPCIONES, L1L2));
        }
        pruebaGenerada.cuestiones = cuestionesGeneradas;
        pruebaGenerada.pregunta = estrategiaPrueba.GeneraEnunciadoPregunta();

        string archivoJson = JsonConvert.SerializeObject(pruebaGenerada);
        Utils.GuardarArchivo(estrategiaPrueba.GetType() + ".json", archivoJson);
    }
    public void CargarCuestiones()
    {

        string jsonFile = Utils.CargarArchivo(estrategiaPrueba.GetType() + ".json");
        prueba = JsonConvert.DeserializeObject<Prueba>(jsonFile);

    }

    public Prueba GenerarPrueba(int numeroCuestiones)
    {
        numeroCuestiones = numeroCuestiones > prueba.cuestiones.Count ? prueba.cuestiones.Count : numeroCuestiones;
        List<Cuestion> pruebaGenerada = new List<Cuestion>();
        List<Cuestion> cuestionesCopia = CrearCopia().cuestiones;

        for (int i = 0; i < numeroCuestiones; i++)
        {
            int posicion = Utils.RandomInt(cuestionesCopia.Count);
            cuestionesCopia[posicion].opciones = Utils.RandomOpcionesCuestion(cuestionesCopia[posicion]);
            pruebaGenerada.Add(cuestionesCopia[posicion]);
            cuestionesCopia.RemoveAt(posicion);
        }
        return new Prueba(pruebaGenerada, prueba.pregunta);
    }
    private Prueba CrearCopia()
    {
        return new Prueba(prueba.cuestiones, prueba.pregunta);
    }


}
