using System.Collections.Generic;

public class Prueba 
{
    public string pregunta { get; set; }
    public List<Cuestion> cuestiones { get; set; }


    public Prueba(List<Cuestion> cuestiones, string pregunta) {
        this.cuestiones = cuestiones;
        this.pregunta = pregunta;
    }
    public Prueba()
    {
        cuestiones = new List<Cuestion>();
    }
}
