using UnityEngine;
using System.Collections;

[System.Serializable]
public class Score {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private string nameLocKey;
	
	[SerializeField]
	private string id;

	[SerializeField]
	private ScoreOrder order;

	[SerializeField]
	private bool useGreaterLimit;
	
	[SerializeField]
	private bool useLowerLimit;

	[SerializeField]
	[Tooltip("Optional")]
	private long greaterLimit;

	[SerializeField]
	[Tooltip("Optional")]
	private long lowerLimit;

	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private long value;

	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public string NameLocKey {
		get {
			return this.nameLocKey;
		}
		set {
			nameLocKey = value;
		}
	}

	public string Id {
		get {
			return this.id;
		}
		set {
			id = value;
		}
	}

	public ScoreOrder Order {
		get {
			return this.order;
		}
		set {
			order = value;
		}
	}

	public long GreaterLimit {
		get {
			return this.greaterLimit;
		}
		set {
			greaterLimit = value;
		}
	}

	public long LowerLimit {
		get {
			return this.lowerLimit;
		}
		set {
			lowerLimit = value;
		}
	}

	public long Value {
		get {
			return this.value;
		}
		set {
			value = value;
		}
	}

	public bool UseGreaterLimit {
		get {
			return this.useGreaterLimit;
		}
		set {
			useGreaterLimit = value;
		}
	}

	public bool UseLowerLimit {
		get {
			return this.useLowerLimit;
		}
		set {
			useLowerLimit = value;
		}
	}

	//--------------------------------------
	// Constructors
	//--------------------------------------
	public Score(){
		nameLocKey = "";
		id = "";
		order = ScoreOrder.DESCENDING;

		useGreaterLimit = false;
		greaterLimit = long.MaxValue;
		useLowerLimit = false;
		lowerLimit = long.MinValue;
	}

	public Score(string pId){
		nameLocKey = "";
		id = pId;
		order = ScoreOrder.DESCENDING;
		
		useGreaterLimit = false;
		greaterLimit = long.MaxValue;
		useLowerLimit = false;
		lowerLimit = long.MinValue;
	}
}
