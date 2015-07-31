using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[System.Serializable]
public class UIBaseListItem {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string PP_ITEM_LIST = "pp_list_item_";
	public const char ATTRIBUTES_SEPARATOR = ',';
	public const char LIST_SEPARATOR = '|';
	public const char LIST_CONTAINER_SEPARATOR = '*';
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	[Tooltip("Must be provided")]
	private int index = -1;
	
	[SerializeField]
	[Tooltip("Must be provided")]
	private string id;
	
	[SerializeField]
	private string name;
	
	[SerializeField]
	[Tooltip("Text file from load the item list")]
	private string textFile;
	
	[SerializeField]
	private Text lbName;
	
	[SerializeField]
	private Sprite icon;
	
	[SerializeField]
	private Animator anim;
	
	[SerializeField]
	private string idle = "idleMiddle";
	
	[SerializeField]
	private string idleRight = "idleRight";
	
	[SerializeField]
	private string idleLeft = "idleLeft";
	
	[SerializeField]
	private string outToLeftTrigger = "outToLeft";
	
	[SerializeField]
	private string outToRightTrigger = "outToRight";
	
	[SerializeField]
	private string inFromRightTrigger = "inFromRight";
	
	[SerializeField]
	private string inFromLeftTrigger = "inFromLeft";
	
	
	
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	
	
	
	
	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public int Index{
		get {
			return this.index;
		}
		set{
			this.index = value;
		}
	}
	
	public string Id {
		get {
			return this.id;
		}
		set{
			this.id = value;
		}
	}
	
	public string Name {
		get {
			return this.name;
		}
	}
	
	public Sprite Icon {
		get {
			return this.icon;
		}
	}
	
	public Animator Anim {
		get {
			return this.anim;
		}
		set {
			anim = value;
		}
	}
	
	public Text LbName {
		get {
			return this.lbName;
		}
	}
	
	//--------------------------------------
	// Constructors
	//--------------------------------------
	/// <summary>
	/// Initializes a new instance of the <see cref="UIBaseListItem"/> class.
	/// 
	/// ID,NAME
	/// </summary>
	/// <param name="data">Data.</param>
	/// <param name="anim">Animator.</param>
	public UIBaseListItem(string data, Animator anim = null){
		this.anim = anim;
		string[] atts = data.Split(ATTRIBUTES_SEPARATOR);
		
		init(atts);
	}
	public UIBaseListItem(string pId, string pName, Animator anim = null){
		this.anim = anim;
		id = pId;
		name = pName;
	}
	//	//--------------------------------------
	//	// Unity Methods
	//	//--------------------------------------
	//	#region Unity
	//	public virtual void Awake(){
	////		anim = GetComponent<Animator>();
	//
	//		if(anim == null){
	////			anim = GetComponentInChildren<Animator>();
	//		}
	//
	//		setPPKey();
	//
	//	}
	//	#endregion
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	/// <summary>
	/// Init the specified data.
	/// 
	/// ID,NAME
	/// </summary>
	/// <param name="data">Data.</param>
	public void init(string[] data){
		if(data.Length > 1){
			id = data[0];
			name = data[1];
		}
	}
	
	public virtual void setToIdle(){
		if(anim != null){
			anim.SetTrigger(idle);
		}
	}
	
	public virtual void initIdle(bool right = true){
		if(right && anim != null)
			anim.SetTrigger(idleRight);
		else if(!right && anim != null)
			anim.SetTrigger(idleLeft);
	}
	
	public virtual void comeIn(bool comeIn = true, bool fromRight = true){
		if(anim != null){
			anim.ResetTrigger(idle);
			
			if(comeIn){
				string animTrigger = fromRight ? inFromRightTrigger : inFromLeftTrigger;
				anim.SetTrigger(animTrigger);
			}
			else{
				string animTrigger = fromRight ? outToRightTrigger : outToLeftTrigger;
				anim.SetTrigger(animTrigger);
			}
		}
	}
	
	public virtual string getContent(){
		string stats = "";
		bool matchWithIndex = index >= 0;
		
		//Load from text initial file
		//		if(!allowLoadFromPlayerPrefs || (allowLoadFromPlayerPrefs && !PlayerPrefs.HasKey(ppKey))){
		//Try to load list item from file
		TextAsset text = Resources.Load(textFile) as TextAsset;
		
		if(text == null){
			Debug.LogError("You must provide a correct list items filename");
		}
		else{
			string[] allItems = text.text.Split('\n');
			
			foreach(string w in allItems){
				string[] atts = w.Split(ATTRIBUTES_SEPARATOR);
				
				if(atts.Length > 0 
				   && ((!matchWithIndex && atts[0] == Id) || (matchWithIndex && atts[0].Equals(index.ToString())))){
					stats = matchWithIndex ? w.Replace(atts[0]+",", Id+",") : w;
					break;
				}
			}
		}
		//		}
		//		//Load from PlayerPrefs
		//		else if(allowLoadFromPlayerPrefs){
		//			stats = PlayerPrefs.GetString(ppKey);
		//		}
		
		return stats;
	}
	
	/// <summary>
	/// Load function.
	/// 
	/// ID,NAME
	/// </summary>
	public virtual void load(){
		string stats = getContent();
		
		//load stats
		if(!string.IsNullOrEmpty(stats)){
			string[] att = stats.Split(ATTRIBUTES_SEPARATOR);
			name = att[1];
		}
	}
	
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="UIBaseListItem"/> with this format: id.name
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="UIBaseListItem"/>.</returns>
	public override string ToString(){
		return Id +ATTRIBUTES_SEPARATOR+ name;
	}
	
	
	
	public virtual void select(){
		
	}
	
	public virtual void show(){
		if(lbName)
			lbName.text = name;
	}
}
