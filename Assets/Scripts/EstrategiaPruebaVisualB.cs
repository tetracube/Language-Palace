using System.Collections.Generic;
using System.Linq;

public class EstrategiaPruebaVisualB : EstrategiaPrueba
{
    public Cuestion GeneraCuestion(string L1, string L2, int opciones, Dictionary<string, string> L1L2)
    {
        List<string> respuestas = new List<string>();
        respuestas.Add(L2);
        List<string> answers = L1L2.Values.ToList<string>();
        answers.Remove(L2);
        for (int i = 1; i < opciones; i++)
        {
            string respuestaRandom = Utils.RandomString(answers);
            respuestas.Add(respuestaRandom);
            answers.Remove(respuestaRandom);
        }

        return new Cuestion(L1, respuestas, L2);
    }
    public string GeneraEnunciadoPregunta()
    {
        return "Elige la palabra que mejor se adapate.";
    }
}
