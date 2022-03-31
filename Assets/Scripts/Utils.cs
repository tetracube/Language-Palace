using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using Random = System.Random;

public class Utils
{
    static Random random = new Random(DateTime.Now.Millisecond);

    public static char RandomChar() {
        return (char) random.Next('a', 'z');
    }

    public static string RandomChar(string excluir)
    {
        string abecedario = "abcdefghijklmnopqrstuvwxyz";
        foreach (char c in excluir){
            abecedario = abecedario.Replace(c.ToString(), string.Empty);     
        }               
        int posicion = random.Next(abecedario.Length);
        return abecedario[posicion].ToString();
    }

    public static int RandomInt(int max)
    {
        return random.Next(max);
    }

    public static string RandomString(List<string> list)
    {
        int posicion = random.Next(list.Count);
        return list[posicion];
    }
    public static bool GuardarArchivo(string path, string contenido)
    {
        string destino = Application.persistentDataPath + "/" + path;
        try
        {
            File.WriteAllText(destino, contenido);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Error leyendo el archvo. " + e.Message);
            return false;
        }
            
    }

    public static string CargarArchivo(string path)
    {
        string destino = Application.persistentDataPath + "/"+ path;
        string archivo = string.Empty;
        try
        {
            archivo = File.ReadAllText(destino);
        }
        catch (Exception e)
        {
            Debug.LogError("Archivo no encontrado. "+e.Message);
        }
        return archivo;
    
    }

    public static string FallosToString(List <string> fallos)
    {
        string resultado = "";
        fallos.ForEach((x) => resultado = resultado + "    -" + x + "\n");
        return resultado;

    }
    public static List<string> RandomOpcionesCuestion(Cuestion cuesition) {

        List<string> opcionesRandomizadas = new List<string>();
        List<string> opcionesCopia = cuesition.opciones.GetRange(0, cuesition.opciones.Count);
  
            for (int i = 0; i < cuesition.opciones.Count; i++)
            {
                int posicion = RandomInt(opcionesCopia.Count);
                opcionesRandomizadas.Add(opcionesCopia[posicion]);
                opcionesCopia.RemoveAt(posicion);
            }
        return opcionesRandomizadas;
    }
    public static bool ChcekIfFull(string[] array)
    {
        foreach (string element in array)
        {
            if (string.IsNullOrEmpty(element)) return false;
        }
        return true;
    }

    public static Dictionary<string,string> ConstruirDiccionario(string[] L1, string [] L2)
    {
        Dictionary<string, string> L1L2 = new Dictionary<string, string>();
        for (int i = 0; i < L1.Length; i++)
        {
            L1L2.Add(L1[i], L2[i]);
        }
        return L1L2;

    }

    public static string ToRomanNumber(int number)
    {
        switch (number) {
            case 1:
                return "I";
            case 2:
                return "II";
            case 3:
                return "III";
            case 4:
                return "IV";

        }
        return null;
    }
    public static string ObtenerMac() {

       return NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(nic =>
            nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)?.GetPhysicalAddress().ToString();
    }
    public static string DictionaryToString(Dictionary<string, string> dictionary)
    {
        string dictionaryString = "";
        foreach (KeyValuePair<string, string> keyValues in dictionary)
        {
            dictionaryString += keyValues.Key + " : " + keyValues.Value + ", ";
        }
        return dictionaryString.TrimEnd(',', ' ') ;
    }
    public static string ListToString(List<string> list) {
       return string.Join(",", list);
    }

    public static string TacharPalabra(string palabra, string palabras) {
        return palabras.Replace(palabra, "<s>" + palabra + "</s>");
    
    }
}

