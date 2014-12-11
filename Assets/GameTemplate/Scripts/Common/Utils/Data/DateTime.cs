using UnityEngine;
using System;
using System.Collections;
using System.Globalization;

public static class DateTimes {
	[Flags]
	public enum DateTimeType{
		NONE = 0,
		FULL = 1,  //dd/mm/aaaa HH:mm:ss
		ONLY_DATE = 2, //dd/mm/aaa
		ONLY_TIME = 4, //HH:mm:ss
		SHORT_TIME = 8, //dd/mm/aaa HH:mm
		COMPLETY_FULL = 16 //dd/mm/aaaa HH:mm:ss:ms
	}
	
	/// <summary>
	/// Pasa el DateTime a string segun la condicion.
	/// 
	/// La condicion debe formarse con operaciones de bit usando el enumerado DateTimeType
	/// </summary>
	/// <returns>El DateTime convertido a string</returns>
	/// <param name="dateTime">El DateTime a convertir.</param>
	/// <param name="condicion">La condicion para la conversion. Debe formarse con operaciones de bit usando el enumerado DateTimeType.</param>
	public static string toString(DateTime dateTime, DateTimeType condicion = DateTimeType.FULL, string dateToken = "/", string timeToken = ":", string dateTimeSeparator = " "){
		string res = "";
		
		switch(condicion){
			
			//dd/mm/aaaa HH:mm:ss
		case DateTimeType.FULL:
			res = dateTime.Day.ToString()+ dateToken +dateTime.Month.ToString()+ dateToken +dateTime.Year.ToString()
				+ dateTimeSeparator +dateTime.Hour.ToString()+ timeToken + dateTime.Minute.ToString() + timeToken + dateTime.Second.ToString();
			break;
			
			//dd/mm/aaaa HH:mm:ss:ms
		case DateTimeType.COMPLETY_FULL:
			res = dateTime.Day.ToString()+ dateToken +dateTime.Month.ToString()+ dateToken +dateTime.Year.ToString()
				+ dateTimeSeparator +dateTime.Hour.ToString()+ timeToken + dateTime.Minute.ToString() + timeToken + dateTime.Second.ToString() + timeToken + dateTime.Millisecond.ToString();
			break;
			
			//dd/mm/aaaa HH:mm
		case (DateTimeType.FULL & DateTimeType.SHORT_TIME):
			res = dateTime.Day.ToString()+ dateToken +dateTime.Month.ToString()+ dateToken +dateTime.Year.ToString()
				+ dateTimeSeparator +dateTime.Hour.ToString()+ timeToken + dateTime.Minute.ToString();
			break;
			
			//dd/mm/aaaa
		case DateTimeType.ONLY_DATE:
			res = dateTime.Day.ToString()+ dateToken +dateTime.Month.ToString()+ dateToken +dateTime.Year.ToString();
			break;
			
			//HH:mm
		case (DateTimeType.ONLY_TIME | DateTimeType.SHORT_TIME):
			res = dateTime.Hour.ToString()+ timeToken + dateTime.Minute.ToString();
			break;
			
			//HH:mm:ss
		case DateTimeType.ONLY_TIME:
			res = dateTime.Hour.ToString()+ timeToken + dateTime.Minute.ToString() + timeToken + dateTime.Second.ToString();
			break;
			
			//HH:mm:ss:ms
		case (DateTimeType.ONLY_TIME | DateTimeType.COMPLETY_FULL):
			res = dateTime.Hour.ToString()+ timeToken + dateTime.Minute.ToString() + timeToken + dateTime.Second.ToString();
			break;
		}
		
		
		return res;
	}
	
	/// <summary>
	/// Pasa un string que representa una fecha formateada en castellano
	/// </summary>
	/// <returns>The date time.</returns>
	/// <param name="dateTime">Date time.</param>
	/// <param name="condicion">Condicion.</param>
	/// <param name="dateToken">Date token.</param>
	/// <param name="timeToken">Time token.</param>
	/// <param name="dateTimeSeparator">Date time separator.</param>
	public static DateTime getDateTime(string dateTime, DateTimeType condicion = DateTimeType.FULL, string dateToken = "/", string timeToken = ":", string dateTimeSeparator = " "){
		DateTime res = default(DateTime);
		
		string[] fhSeparada = dateTime.Split (" " [0]); // separamos la fecha de la hora (0) fecha (1) hora
		string fecha = fhSeparada [0]; // dd/mm/aaaa
		string hora = fhSeparada [1]; // HH:mm:ss
		string[] fSeparada = fecha.Split("/" [0]); // separamos la fecha (0) dd, (1) mm, (2) aaaa
		string[] hSeparada = hora.Split (":" [0]); // separamos la hora (0) HH, (1) mm, (2) ss
		int dia = 0, mes = 0, anio = 0, horas = 0, minutos = 0 ,segundos = 0, milisegundos = 0;
		
		int.TryParse(fSeparada[0], out dia); //pasamos a entero el dia
		int.TryParse(fSeparada[1], out mes); //pasamos a entero el mes
		int.TryParse(fSeparada[2], out anio); //pasamos a entero el anio
		
		int.TryParse(hSeparada[0], out horas); //pasamos a entero las horas
		int.TryParse(hSeparada[1], out minutos); //pasamos a entero los minutos
		int.TryParse(hSeparada[2], out segundos); //pasamos a entero los segundos
		int.TryParse(hSeparada[3], out milisegundos); //pasamos a entero los milisegundos
		
		switch(condicion){
			//dd/mm/aaaa HH:mm:ss
		case DateTimeType.FULL:
			res = new DateTime(dia,mes,anio,horas,minutos,segundos);
			break;
			
			//dd/mm/aaaa HH:mm:ss:ms
		case DateTimeType.COMPLETY_FULL:
			res = new DateTime(dia,mes,anio,horas,minutos,segundos,milisegundos);
			break;
			
			//dd/mm/aaaa HH:mm
		case (DateTimeType.FULL & DateTimeType.SHORT_TIME):
			res = new DateTime(dia,mes,anio,horas,minutos,0);
			break;
			
			//dd/mm/aaaa
		case DateTimeType.ONLY_DATE:
			res = new DateTime(dia,mes,anio);
			break;
			
			//HH:mm
		case (DateTimeType.ONLY_TIME | DateTimeType.SHORT_TIME):
			res = new DateTime(0,0,0,horas,minutos,0);
			break;
			
			//HH:mm:ss
		case DateTimeType.ONLY_TIME:
			res = new DateTime(0,0,0,horas,minutos,segundos);
			break;
			
			//HH:mm:ss:ms
		case (DateTimeType.ONLY_TIME | DateTimeType.COMPLETY_FULL):
			res = new DateTime(0,0,0,horas,minutos,segundos,milisegundos);
			break;
		}
		
		return res;
	}
	
	/// <summary>
	/// Pasa un DateTime a Unix Timestamp
	/// </summary>
	/// <returns>The unix timestamp.</returns>
	/// <param name="target">Target.</param>
	public static long ToUnixTimestamp(this DateTime target){
		var date = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
		var unixTimestamp = System.Convert.ToInt64((target - date).TotalSeconds);
		
		return unixTimestamp;
	}
	
	/// <summary>
	/// Obtiene un DateTime apartir de un Unix Timestamp
	/// </summary>
	/// <returns>The date time resultado.</returns>
	/// <param name="target">Target.</param>
	/// <param name="timestamp">Timestamp.</param>
	public static DateTime ToDateTime(this DateTime target, long timestamp){
		var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
		
		return dateTime.AddSeconds(timestamp);
	}

}
