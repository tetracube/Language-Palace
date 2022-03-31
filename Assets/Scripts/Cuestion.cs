using System.Collections.Generic;

[System.Serializable]
public class Cuestion
{
    public string enunciado { get; set; }
    public List<string> opciones { get; set; }
    public string respuesta { get; set; }

    public Cuestion(string enunciado, List<string> opciones, string respuesta)
    {
        this.enunciado = enunciado;
        this.opciones = opciones;
        this.respuesta = respuesta;
    }


}
