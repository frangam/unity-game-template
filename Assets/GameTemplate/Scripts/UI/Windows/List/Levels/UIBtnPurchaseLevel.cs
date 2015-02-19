using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnionAssets.FLE;

public class UIBtnPurchaseLevel : UIBasePurchaseButton {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private int idLevel;
	
	[SerializeField]
	private Text lbIdLevel;
	
	[SerializeField]
	private string sceneToGoWhenPress;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	
	private bool levelLoaded = false;
	
	//--------------------------------------
	// Getters && Setters
	//--------------------------------------
	public int IdLevel {
		get {
			return this.idLevel;
		}
	}
	
	public bool LevelLoaded {
		get {
			return this.levelLoaded;
		}
	}
	
	
	public BasePurchasableLevel Level{
		get{
			return UILevel.Level;
		}
	}
	
	public UIBasePurchasableLevel UILevel{
		get{
			return ((UIBasePurchasableLevel) Item);
		}
	}
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void Awake ()
	{
		base.Awake ();
		BaseLevelLoaderController.dispatcher.addEventListener(BaseLevelLoaderController.ALL_LEVELS_LOADED, OnAllLevelsLoaded);
		
		if(lbIdLevel)
			lbIdLevel.text = idLevel.ToString();
		
		if(!levelLoaded)
			loadLevelProperties();
	}
	
	//	public override void SetLocked ()
	//	{
	//		int lastLevel = PlayerPrefs.GetInt(ppKeyLastLevelPurchased);
	//		Locked = idLevel > lastLevel;
	//	}
	
	public override void OnDestroy ()
	{
		base.OnDestroy ();
		BaseLevelLoaderController.dispatcher.removeEventListener(BaseLevelLoaderController.ALL_LEVELS_LOADED, OnAllLevelsLoaded);
		
	}
	protected override void doPress ()
	{
		base.doPress ();
		
		if(Item != null && Item.Purchased){
			selectLevel();
			ScreenLoaderVisualIndicator.Instance.LoadScene(sceneToGoWhenPress);
		}
		
	}
	
	public virtual void selectLevel(){
		PlayerPrefs.SetInt(GameSettings.PP_SELECTED_LEVEL, idLevel);
	}
	
	public virtual void loadLevelProperties(){
		if(BaseLevelLoaderController.Instance.Levels != null && BaseLevelLoaderController.Instance.Levels.Count > 0){
			foreach(BaseLevel l in BaseLevelLoaderController.Instance.Levels){
				if(l.Id.Equals(idLevel.ToString())){
					BasePurchasableLevel level = l as BasePurchasableLevel;
					
					if(level != null){
						Item = new UIBasePurchasableLevel(PPKEY, level, "");
						levelLoaded = true;
					}
					
					break;
				}
			}
			
			showInformation();
		}
	}
	
	//--------------------------------------
	//  EVENTS
	//--------------------------------------
	public void OnAllLevelsLoaded(CEvent e){
		if(!levelLoaded)
			loadLevelProperties();
	}
}
