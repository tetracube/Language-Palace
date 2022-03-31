using UnityEngine;
using System.Data;
using System;
using System.Collections.Generic;
using System.Data.SQLite;

public class DataBase : MonoBehaviour
{

	private static DataBase instance;
	private static IDbConnection dataBase;

	private int idPartida;
	private long fechaPartida;
	private long idNivel;
	private long idPrueba;
	private DataBase() { }

	public static DataBase GetInstance()
	{
		return instance;
	}
	void Awake()

	{
		if (instance == null)
		{	
			instance = this;
			DontDestroyOnLoad(instance.gameObject);
			dataBase = CrearConexion();
			CrearEsquema();
			instance.idPartida = Utils.ObtenerMac().GetHashCode();
			instance.fechaPartida = FechaPartidaActual();
		}
	}

	private IDbConnection CrearConexion() {
		string urlConexion = "URI=file:" + Application.persistentDataPath + "/ROMAN_PALACE";
		IDbConnection dataBase = new SQLiteConnection(urlConexion);
		dataBase.Open();
		return dataBase;
	}

	private void CrearEsquema() {

		IDbCommand comando = dataBase.CreateCommand();
		string crearEsquema = Resources.Load<TextAsset>("EsquemaBD").text;
		comando.CommandText = crearEsquema;
		comando.ExecuteReader();

	}
	public void CerrarConexion()
	{ 
		dataBase.Close();
	}
	public static void InsertarEnPartida(string apodo, int edad, string genero, string L1, string L2, string nacionalidad, string pais) {
		IDbCommand comando = dataBase.CreateCommand();
		apodo = apodo.Trim();
		genero = genero.Trim();
		L1 = L1.Trim();
		L2 = L2.Trim();
		nacionalidad = nacionalidad.Trim();
		pais = pais.Trim();
		DateTimeOffset ahora = DateTime.Now;
		instance.fechaPartida = ahora.ToUnixTimeSeconds();
		comando.CommandText = "INSERT INTO PARTIDA (ID, FECHA, APODO, EDAD, GENERO, L1, L2, NACIONALIDAD, PAIS, PELO, OJOS, PIEL, VESTIMENTA) VALUES" +
			" ("+ instance.idPartida + ","+ instance.fechaPartida + ",'" + apodo + "'," + edad + ",'" + genero + "','" + L1 + "','" + L2 + "','" + nacionalidad + "','" + pais + "','','','','')";
		comando.ExecuteNonQuery();

	}

	public static void InsertarEnNivel(int nivel, int tiempo, Dictionary <string, string> L1L2)
	{
		IDbCommand comando = dataBase.CreateCommand();
		DateTimeOffset ahora = DateTime.Now;
		instance.idNivel = ahora.ToUnixTimeSeconds();
		comando.CommandText = "INSERT INTO NIVEL (ID, FECHA_PARTIDA, PARTIDA, NUMERO, TIEMPO_COMPLETAR, PALABRAS) VALUES" +
			" ("+ instance.idNivel + ","+ instance.fechaPartida + "," + instance.idPartida + "," + nivel + "," + tiempo+ ",'" + Utils.DictionaryToString(L1L2) +"')";
		comando.ExecuteNonQuery();

	}
	public static void InsertarEnPrueba(string tipo, int puntuacion, int cantidadFallos, int cantidadAciertos, List <string> fallos, List<string> palabras)
	{
		IDbCommand comando = dataBase.CreateCommand();

		DateTimeOffset ahora = DateTime.Now;
		instance.idPrueba = ahora.ToUnixTimeSeconds();
		comando.CommandText = "INSERT INTO PRUEBA (ID, FECHA_PARTIDA, PARTIDA, NIVEL, TIPO, PUNTUACION, CANTIDAD_FALLOS, CANTIDAD_ACIERTOS, FALLOS, PALABRAS) VALUES" +
			" (" + instance.idPrueba + "," + instance.fechaPartida + "," + instance.idPartida + "," + instance.idNivel+ ",'" + tipo + "'," + puntuacion + "," + cantidadFallos + "," + cantidadAciertos + ",'" +Utils.ListToString(fallos) + "','" + Utils.ListToString(palabras) + "')";
		comando.ExecuteNonQuery();

	}

	public static void ActualizarEnPrueba(int puntuacion, int cantidadFallos, int cantidadAciertos, List<string> fallos)
	{
		IDbCommand comando = dataBase.CreateCommand();
		comando.CommandText = "UPDATE PRUEBA SET PUNTUACION=" + puntuacion + ", CANTIDAD_FALLOS=" + cantidadFallos + ", CANTIDAD_ACIERTOS=" + cantidadAciertos + ", FALLOS='" + Utils.ListToString(fallos) +
			"' WHERE ID IS " + instance.idPrueba + " AND FECHA_PARTIDA IS " + instance.fechaPartida + " AND PARTIDA IS " + instance.idPartida + " AND NIVEL IS " + instance.idNivel;
		comando.ExecuteNonQuery();
	}
	public static void ActualizarEnNivel(int tiempo)
	{
		IDbCommand comando = dataBase.CreateCommand();
		comando.CommandText = "UPDATE NIVEL SET TIEMPO_COMPLETAR=" + tiempo +
			" WHERE ID IS " + instance.idNivel + " AND FECHA_PARTIDA IS " + instance.fechaPartida + " AND PARTIDA IS " + instance.idPartida;
		comando.ExecuteNonQuery();
	}

	public static bool EsPrimeraPartida() {

		IDbCommand cmnd_read = dataBase.CreateCommand();
		IDataReader reader;
		string query = "SELECT COUNT (*) FROM PARTIDA WHERE ID IS " + instance.idPartida;
		cmnd_read.CommandText = query;
		reader = cmnd_read.ExecuteReader();

		reader.Read();
		int count = reader.GetInt32(0);
		return count == 0;

	}
	public static int UltimoNivelSuperado()
	{
		IDbCommand cmnd_read = dataBase.CreateCommand();
		IDataReader reader;
		string query = "SELECT MAX (NUMERO) FROM NIVEL WHERE " +
			"TIEMPO_COMPLETAR > -1 AND PARTIDA IS " + instance.idPartida + " AND FECHA_PARTIDA IS " +
			" (SELECT MAX (FECHA) FROM PARTIDA WHERE ID IS " + instance.idPartida + " )";
		cmnd_read.CommandText = query;
		reader = cmnd_read.ExecuteReader();

		reader.Read();
		int nivel = 0;
		try
		{
			nivel = reader.GetInt32(0); //si no se cumple, lanza excepción
		}
		catch (InvalidCastException e) { 
		}
		return nivel;

	}

	public static int[] MejorJugada(int nivel)
	{
		int[] mejorJugada = new int[2];
		IDbCommand cmnd_read = dataBase.CreateCommand();
		IDataReader reader;
		string query = "SELECT MAX (P.PUNTUACION), TIEMPO_COMPLETAR FROM NIVEL N, PRUEBA P" +
			" WHERE N.ID = P.NIVEL AND P.PARTIDA = " + instance.idPartida + " AND P.FECHA_PARTIDA = (SELECT MAX (FECHA) FROM PARTIDA WHERE ID IS " + instance.idPartida + ") AND P.TIPO = 'PruebaFinal' AND N.NUMERO = " + nivel;
		cmnd_read.CommandText = query;
		reader = cmnd_read.ExecuteReader();

		reader.Read();
		try
		{
			mejorJugada[0] = (int) reader.GetDouble(0); //si no se cumple, lanza excepción
			mejorJugada[1] = reader.GetInt32(1);
		}
		catch (InvalidCastException e)
		{
		}
		return mejorJugada;

	}
	private static int FechaPartidaActual()
	{
		IDbCommand cmnd_read = dataBase.CreateCommand();
		IDataReader reader;
		string query = "SELECT MAX (FECHA) FROM PARTIDA" +
			" WHERE ID = " + instance.idPartida;
		cmnd_read.CommandText = query;
		reader = cmnd_read.ExecuteReader();

		reader.Read();
		int fecha = 0;
		try
		{
			fecha = reader.GetInt32(0); //si no se cumple, lanza excepción
		}
		catch (InvalidCastException e)
		{
		}
		return fecha;

	}
}
