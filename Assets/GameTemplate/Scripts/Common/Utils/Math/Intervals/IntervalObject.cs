using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ANMiniJSON;


//-----
//We need this dummy classes in order to edit attributes of IntervalObject<T> 
//from Unity Inspector
//-----
[System.Serializable]
public class IntervalObjectInt : IntervalObject<int> {}
[System.Serializable]
public class IntervalObjectLong : IntervalObject<long> {}
[System.Serializable]
public class IntervalObjectFloat : IntervalObject<float> {}
[System.Serializable]
public class IntervalObjectDouble : IntervalObject<double> {}
[System.Serializable]
public class IntervalObjectString : IntervalObject<string> {}


public class IntervalObject<T> {
	//--------------------------------------
	// Constantes privadas
	//--------------------------------------
	private const string PARAM_FROM 	= "from";
	private const string PARAM_TO 		= "to";
	
	//--------------------------------------
	// Atributos privados
	//--------------------------------------
	[SerializeField]
	private T from;

	[SerializeField]
	private T to;
	
	//--------------------------------------
	// Constructores
	//--------------------------------------
	public IntervalObject(){
		from = default(T);
		to = default(T);
	}
	/// <summary>
	/// Inicializa una nueva instancia de la clase <see cref="IntervalObject`1"/> .
	/// 
	/// Inicializa el intervalo con el mismo valor de from para to.
	/// </summary>
	/// <param name="pFrom">P from.</param>
	public IntervalObject(T pFrom){
		from = pFrom;
		to = pFrom;
	}
	
	public IntervalObject(T pFrom, T pTo){
		from = pFrom;
		to = pTo;
	}
	
	public IntervalObject(string json){
		var dict = Json.Deserialize(json) as Dictionary<string,object>; //deserializamos el json que nos llega 
		
		T res1, res2;
		int f1 = (int)((long) dict [PARAM_FROM]);
		
		//tenemos que realizar diferentes comprobaciones para los diferentes tipos validos: int, long, float, double y string
		//comprobando de que tipo es T
		
		//--
		//Cast int
		//--
		if(Casting.TryCast(f1, out res1)){
			int t1 = (int)((long) dict [PARAM_TO]);
			
			if(Casting.TryCast(f1, out res2))
				this.to = res2;
			
			this.from = res1;
		}
		else{
			long f2 = (long) dict [PARAM_FROM];
			
			//--
			//Cast long
			//--
			if(Casting.TryCast(f2, out res1)){
				long t2 = (long) dict [PARAM_TO];
				
				if(Casting.TryCast(t2, out res2))
					this.to = res2;
				
				this.from = res1;
			}
			else{
				float f3 = (float) ((double) dict[PARAM_FROM]);
				
				//--
				//Cast float
				//--
				if(Casting.TryCast(f3, out res1)){
					float t3 = (float) ((double) dict[PARAM_TO]);
					
					if(Casting.TryCast(t3, out res2))
						this.to = res2;
					
					this.from = res1;
				}
				else{
					double f4 = (double) dict[PARAM_FROM];
					
					//--
					//Cast double
					//--
					if(Casting.TryCast(f4, out res1)){
						double t4 = (double) dict[PARAM_TO];
						
						if(Casting.TryCast(t4, out res2))
							this.to = res2;
						
						this.from = res1;
					}
					else{
						string f5 = (string) dict[PARAM_FROM];
						
						//--
						//Cast string
						//--
						if(Casting.TryCast(f5, out res1)){
							string t5 = (string) dict[PARAM_TO];
							
							if(Casting.TryCast(t5, out res2))
								this.to = res2;
							
							this.from = res1;
						}
					}
				}
			}
		}
		
		
		
		//		if(this.from is ((int) ((long) dict["from"]))){
		// 			this.from = ((int) ((long) dict["from"]));
		//			this.to = (object) ((int) ((long) dict["to"]));
		//		}
		//		else if(typeof(T) == typeof(long)){
		//			this.from = (object) ((long) dict["from"]);
		//			this.to = (object) ((long) dict["to"]);
		//		}
		//		else if(typeof(T) == typeof(float)){
		//			this.from = (object) ((float) ((double) dict["from"]));
		//			this.to = (object) ((float) ((double) dict["to"]));
		//		}
		//		else if(typeof(T) == typeof(double)){
		//			this.from = (object) ((double) dict["from"]);
		//			this.to = (object) ((double) dict["to"]);
		//		}
		//		else if(typeof(T) == typeof(string)){
		//			this.from = (object) ((string) dict["from"]);
		//			this.to = (object) ((string) dict["to"]);
		//		}
	}
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public T From{
		get{ return from;}
		set{ from = value;}
	}
	public T To{
		get{ return to;}
		set{ to = value;}
	}
	
	//--------------------------------------
	// Metodos sobreescritos
	//--------------------------------------
	public override string ToString ()
	{
		return string.Format ("[IntervalIntObject: From={0}, To={1}]", From, To);
	}
	
	//--------------------------------------
	// Metodos publicos
	//--------------------------------------
	public bool contiene(IntervalObject<T> otro, bool incluyeFrom = true, bool incluyeTo = true){
		bool res = false;
		
		if(typeof(T) == typeof(int)){
			//son necesarias operaciones de casting en primer lugar
			int mF = 0, mT = 0, oF = 0, oT = 0;
			Casting.TryCast(this.from, out mF);
			Casting.TryCast(this.to, out mT);
			Casting.TryCast(otro.From, out oF);
			Casting.TryCast(otro.To, out oT);
			
			res = (		((incluyeFrom && incluyeTo) && ((oF >= mF && oF <= mT) && (oT>= mF && oT <= mT)))
			       ||	((incluyeFrom && !incluyeTo) && ((oF >= mF && oF < mT) && (oT>= mF && oT < mT)))
			       ||	((!incluyeFrom && incluyeTo) && ((oF > mF && oF <= mT) && (oT> mF && oT <= mT)))
			       ||	((!incluyeFrom && !incluyeTo) && ((oF > mF && oF < mT) && (oT> mF && oT < mT)))
			       );
		}
		else if(typeof(T) == typeof(long)){
			//son necesarias operaciones de casting en primer lugar
			long mF = 0, mT = 0, oF = 0, oT = 0;
			Casting.TryCast(this.from, out mF);
			Casting.TryCast(this.to, out mT);
			Casting.TryCast(otro.From, out oF);
			Casting.TryCast(otro.To, out oT);
			
			res = (		((incluyeFrom && incluyeTo) && ((oF >= mF && oF <= mT) && (oT>= mF && oT <= mT)))
			       ||	((incluyeFrom && !incluyeTo) && ((oF >= mF && oF < mT) && (oT>= mF && oT < mT)))
			       ||	((!incluyeFrom && incluyeTo) && ((oF > mF && oF <= mT) && (oT> mF && oT <= mT)))
			       ||	((!incluyeFrom && !incluyeTo) && ((oF > mF && oF < mT) && (oT> mF && oT < mT)))
			       );
		}
		else if(typeof(T) == typeof(float)){
			//son necesarias operaciones de casting en primer lugar
			float mF = 0f, mT = 0f, oF = 0, oT = 0;
			Casting.TryCast(this.from, out mF);
			Casting.TryCast(this.to, out mT);
			Casting.TryCast(otro.From, out oF);
			Casting.TryCast(otro.To, out oT);
			
			res = (		((incluyeFrom && incluyeTo) && ((oF >= mF && oF <= mT) && (oT>= mF && oT <= mT)))
			       ||	((incluyeFrom && !incluyeTo) && ((oF >= mF && oF < mT) && (oT>= mF && oT < mT)))
			       ||	((!incluyeFrom && incluyeTo) && ((oF > mF && oF <= mT) && (oT> mF && oT <= mT)))
			       ||	((!incluyeFrom && !incluyeTo) && ((oF > mF && oF < mT) && (oT> mF && oT < mT)))
			       );
		}
		else if(typeof(T) == typeof(double)){
			//son necesarias operaciones de casting en primer lugar
			double mF = 0f, mT = 0f, oF = 0f, oT = 0f;
			Casting.TryCast(this.from, out mF);
			Casting.TryCast(this.to, out mT);
			Casting.TryCast(otro.From, out oF);
			Casting.TryCast(otro.To, out oT);
			
			res = (		((incluyeFrom && incluyeTo) && ((oF >= mF && oF <= mT) && (oT>= mF && oT <= mT)))
			       ||	((incluyeFrom && !incluyeTo) && ((oF >= mF && oF < mT) && (oT>= mF && oT < mT)))
			       ||	((!incluyeFrom && incluyeTo) && ((oF > mF && oF <= mT) && (oT> mF && oT <= mT)))
			       ||	((!incluyeFrom && !incluyeTo) && ((oF > mF && oF < mT) && (oT> mF && oT < mT)))
			       );
		}
		else if(typeof(T) == typeof(string)){
			//son necesarias operaciones de casting en primer lugar
			string mSF = "", mST = "", oSF = "", oST = "";
			int mF = 0, mT = 0, oF = 0, oT = 0;
			
			//priero casteamos a string
			Casting.TryCast(this.from, out mSF);
			Casting.TryCast(this.to, out mST);
			Casting.TryCast(otro.From, out oSF);
			Casting.TryCast(otro.To, out oST);
			
			//luego a int
			int.TryParse(mSF, out mF);
			int.TryParse(mST, out mT);
			int.TryParse(oSF, out oF);
			int.TryParse(oST, out oT);
			
			res = (		((incluyeFrom && incluyeTo) && ((oF >= mF && oF <= mT) && (oT>= mF && oT <= mT)))
			       ||	((incluyeFrom && !incluyeTo) && ((oF >= mF && oF < mT) && (oT>= mF && oT < mT)))
			       ||	((!incluyeFrom && incluyeTo) && ((oF > mF && oF <= mT) && (oT> mF && oT <= mT)))
			       ||	((!incluyeFrom && !incluyeTo) && ((oF > mF && oF < mT) && (oT> mF && oT < mT)))
			       );
		}
		
		return res;
	}
	
	public bool contiene(T escalar, bool incluyeFrom = true, bool incluyeTo = true){
		bool res = false;
		
		if(typeof(T) == typeof(int)){
			//son necesarias operaciones de casting en primer lugar
			int mF = 0, mT = 0, e = 0;
			Casting.TryCast(this.from, out mF);
			Casting.TryCast(this.to, out mT);
			Casting.TryCast(escalar, out e);
			
			res = (		((incluyeFrom && incluyeTo) && (e >= mF && e <= mT))
			       ||	((incluyeFrom && !incluyeTo) && (e >= mF && e < mT))
			       ||	((!incluyeFrom && incluyeTo) && (e > mF && e <= mT))
			       ||	((!incluyeFrom && !incluyeTo) && (e > mF && e < mT))
			       );
		}
		else if(typeof(T) == typeof(long)){
			//son necesarias operaciones de casting en primer lugar
			long mF = 0, mT = 0, e = 0;
			Casting.TryCast(this.from, out mF);
			Casting.TryCast(this.to, out mT);
			Casting.TryCast(escalar, out e);
			
			res = (		((incluyeFrom && incluyeTo) && (e >= mF && e <= mT))
			       ||	((incluyeFrom && !incluyeTo) && (e >= mF && e < mT))
			       ||	((!incluyeFrom && incluyeTo) && (e > mF && e <= mT))
			       ||	((!incluyeFrom && !incluyeTo) && (e > mF && e < mT))
			       );
		}
		else if(typeof(T) == typeof(float)){
			//son necesarias operaciones de casting en primer lugar
			float mF = 0f, mT = 0f, e = 0;
			Casting.TryCast(this.from, out mF);
			Casting.TryCast(this.to, out mT);
			Casting.TryCast(escalar, out e);
			
			res = (		((incluyeFrom && incluyeTo) && (e >= mF && e <= mT))
			       ||	((incluyeFrom && !incluyeTo) && (e >= mF && e < mT))
			       ||	((!incluyeFrom && incluyeTo) && (e > mF && e <= mT))
			       ||	((!incluyeFrom && !incluyeTo) && (e > mF && e < mT))
			       );
		}
		else if(typeof(T) == typeof(double)){
			//son necesarias operaciones de casting en primer lugar
			double mF = 0f, mT = 0f, e = 0;
			Casting.TryCast(this.from, out mF);
			Casting.TryCast(this.to, out mT);
			Casting.TryCast(escalar, out e);
			
			res = (		((incluyeFrom && incluyeTo) && (e >= mF && e <= mT))
			       ||	((incluyeFrom && !incluyeTo) && (e >= mF && e < mT))
			       ||	((!incluyeFrom && incluyeTo) && (e > mF && e <= mT))
			       ||	((!incluyeFrom && !incluyeTo) && (e > mF && e < mT))
			       );
		}
		else if(typeof(T) == typeof(string)){
			//son necesarias operaciones de casting en primer lugar
			string mSF = "", mST = "", oSF = "", oST = "";
			int mF = 0, mT = 0, e = 0;
			
			//priero casteamos a string
			Casting.TryCast(this.from, out mSF);
			Casting.TryCast(this.to, out mST);
			Casting.TryCast(escalar, out e);
			
			//luego a int
			int.TryParse(mSF, out mF);
			int.TryParse(mST, out mT);
			
			res = (		((incluyeFrom && incluyeTo) && (e >= mF && e <= mT))
			       ||	((incluyeFrom && !incluyeTo) && (e >= mF && e < mT))
			       ||	((!incluyeFrom && incluyeTo) && (e > mF && e <= mT))
			       ||	((!incluyeFrom && !incluyeTo) && (e > mF && e < mT))
			       );
		}
		
		return res;
	}
}
