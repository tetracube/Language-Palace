using System.Collections.Generic;

public interface EstrategiaPrueba
{
    Cuestion GeneraCuestion(string L1, string L2, int opciones, Dictionary<string, string> L1L2);
    string GeneraEnunciadoPregunta();

}
