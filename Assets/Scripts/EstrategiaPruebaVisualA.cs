using System.Collections.Generic;
using System.Text;

public class EstrategiaPruebaVisualA : EstrategiaPrueba
{
    public Cuestion GeneraCuestion(string L1, string L2, int opciones, Dictionary<string, string> L1L2)
    {
        StringBuilder stringBuilder = new StringBuilder(L2);
        string letra = " ";
        int posicion = 0;

        while (letra.Equals(" "))
        {
            posicion = Utils.RandomInt(L2.Length);
            letra = L2[posicion].ToString(); 
        }
        stringBuilder[posicion] = '*';
        L2 = stringBuilder.ToString();

        List<string> respuestas = new List<string>();
      //  letra = letra.ToLower();
        respuestas.Add(letra);
        string exclude = letra;

        for (int i = 1; i < opciones; i++)
        {
            string randomChar = Utils.RandomChar(exclude);
            respuestas.Add(randomChar);
            exclude += randomChar;
        }

        return new Cuestion(L2, respuestas, letra);
    }
    public string GeneraEnunciadoPregunta() {
        return "Completa las palabras con la letra faltante entre las opciones ofrecidas.";
    }
}
