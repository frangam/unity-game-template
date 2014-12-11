using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ANMiniJSON;

public class ScalarOrIntervalValue<T> {
		//--------------------------------------
		// Constantes privadas
		//--------------------------------------
		
		
		//--------------------------------------
		// Atributos privados
		//--------------------------------------
		private T scalar;
		private IntervalObject<T> interval;
		
		//--------------------------------------
		// Constructores
		//--------------------------------------
		public ScalarOrIntervalValue(T pScalar){
			this.scalar = pScalar;
			this.interval = null;
		}
		public ScalarOrIntervalValue(IntervalObject<T> pInterval){
			this.interval = pInterval;
			this.scalar = default(T);
		}
		public ScalarOrIntervalValue(T pFrom, T pTo){
			this.interval = new IntervalObject<T> (pFrom, pTo);
			this.scalar = default(T);
		}
		public ScalarOrIntervalValue(string json){
			this.interval = new IntervalObject<T> (json);
			
			//se trata de un escalar
			if(this.interval == null){
				if(typeof(T) == typeof(int)){
					T ms;
					int value = 0; int.TryParse(json, out value); //el valor del json
					if(Casting.TryCast(value, out ms))
						this.scalar = ms;
					
				}
				else if(typeof(T) == typeof(long)){
					T ms;
					long value = 0; long.TryParse(json, out value); //el valor del json
					if(Casting.TryCast(value, out ms))
						this.scalar = ms;
				}
				else if(typeof(T) == typeof(float)){
					T ms;
					float value = 0f; float.TryParse(json, out value); //el valor del json
					if(Casting.TryCast(value, out ms))
						this.scalar = ms;
				}
				else if(typeof(T) == typeof(double)){
					T ms;
					double value = 0f; double.TryParse(json, out value); //el valor del json
					if(Casting.TryCast(value, out ms))
						this.scalar = ms;
				}
				else if(typeof(T) == typeof(string)){
					T ms;
					string value = json; //el valor del json
					if(Casting.TryCast(value, out ms))
						this.scalar = ms;
				}
			}
		}
		
		//--------------------------------------
		// Getters/Setters
		//--------------------------------------
		public T Scalar{
			get{ return scalar;}
		}
		public IntervalObject<T> Interval{
			get{ return interval;}
		}
		
		//--------------------------------------
		// Metodos sobreescritos
		//--------------------------------------
		public override string ToString (){
			return string.Format ("[ScalarOrIntervalValue: Scalar={0}, Interval={1}]", Scalar, Interval);
		}
}
