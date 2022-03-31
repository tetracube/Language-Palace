using System.Collections.Generic;

public interface ConstructorNivel {

    void Recomponer();
    void AñadirPuertas(bool añadir);
    void ColocarObjetos(int nivel, Dictionary <string, string> L1L2);
    void GenerarPreguntas(Dictionary<string, string> L1L2, List<EstrategiaPrueba> tipoPruebas);
    void AñadirTemporizador();
    Nivel ObtenerResultado();

}
