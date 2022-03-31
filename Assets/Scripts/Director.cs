using System.Collections.Generic;

public class Director 
{
    public ConstructorNivel constructorNivel { get; private set; }

    public Director(ConstructorNivel constructor) {
        constructorNivel = constructor;
    }

    public void CambiarConstructor(ConstructorNivel constructor)
    {
        constructorNivel = constructor;
    }

    public Nivel GeneraNivelUno(Dictionary<string, string> L1L2) {
        constructorNivel.Recomponer();
        constructorNivel.AñadirPuertas(false);
        constructorNivel.ColocarObjetos(1, L1L2);
        List<EstrategiaPrueba> tipoPruebas = new List<EstrategiaPrueba>();
        tipoPruebas.Add(new EstrategiaPruebaVisualA());
        tipoPruebas.Add(new EstrategiaPruebaVisualB());
        constructorNivel.GenerarPreguntas(L1L2, tipoPruebas);
        constructorNivel.AñadirTemporizador();
        return constructorNivel.ObtenerResultado();

    }
    public Nivel GeneraNivelDos(Dictionary<string, string> L1L2)
    {
        constructorNivel.Recomponer();
        constructorNivel.AñadirPuertas(true);
        constructorNivel.ColocarObjetos(2, L1L2);
        List<EstrategiaPrueba> tipoPruebas = new List<EstrategiaPrueba>();
        tipoPruebas.Add(new EstrategiaPruebaVisualA());
        tipoPruebas.Add(new EstrategiaPruebaVisualB());
        constructorNivel.GenerarPreguntas(L1L2, tipoPruebas);
        constructorNivel.AñadirTemporizador();
        return constructorNivel.ObtenerResultado();

    }


}
