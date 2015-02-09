using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIBaseListWindow : UIBaseWindow {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const char ATTRIBUTES_SEPARATOR = ',';

	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
//	[SerializeField]
//	protected bool							allowLoadFromPlayerPrefs = true;
//
//	[SerializeField]
//	protected string						PP_KEY = UIBaseListItem.PP_ITEM_LIST;
		
	[SerializeField]
	private bool 							loadItemsOnAwake = true;

	[SerializeField]
	protected string						textLoadFile;

	[SerializeField]
	protected List<UIBaseListItem> 			items;

	[SerializeField]
	private UIBaseButton 					btnPrevItem;

	[SerializeField]
	private UIBaseButton 					btnNextItem;

	[SerializeField]
	[Tooltip("Leave < 0 if we want to hanle it with the lenght of the items list. Set it if we want to specify another last item, i.e. when we want to show a list of the available missions to complete that in this case we will show only navigable unlocked levels.")]
	/// <summary>
	/// Leave < 0 if we want to hanle it with the lenght of the items list. Set it if we want to specify another last item, i.e. when we want to show a list of the available missions to complete that in this case we will show only navigable unlocked levels.
	/// </summary>
	protected int							indexLastItem = -1;

	[SerializeField]
	protected Text 							lbNameCurrentItem;

	[SerializeField]
	protected UIBaseSceneNavigationButton 	btnBack;


	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	protected int 							indexCurrentItem = 0;
	protected UIBaseListItem				currentItemSelected;


	//--------------------------------------
	// Getters/Setters
	//--------------------------------------
	public List<UIBaseListItem> Items {
		get {
			return this.items;
		}
	}

	public int IndexCurrentItem {
		get {
			return this.indexCurrentItem;
		}
	}

	public UIBaseListItem CurrentItemSelected {
		get {
			UIBaseListItem res = null;

			if(items != null && items.Count > indexCurrentItem)
				res = items[indexCurrentItem];

			return res;
		}
	}

	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void open ()
	{
		base.open ();

		initIndexCurrentItem();
		initIndexLastItem();
		
		if(loadItemsOnAwake){
			loadItems();
			currentItemSelected = items[indexCurrentItem];

			for(int i=0; i<items.Count; i++){
				//idle to the left
				if(i < indexCurrentItem)
					items[i].initIdle(false);

				//idle to the right
				else if(i > indexCurrentItem)
					items[i].initIdle(true);

				//current item
				//idle to the middle
				else
					items[i].setToIdle();
			}
		}
		
		//navigation Buttons
		updateNavigationButtons();
		
		showItem(currentItemSelected);
	}

	//--------------------------------------
	// Unity Methods
	//--------------------------------------
	#region Unity
	public override void Awake(){
		base.Awake();


	}
	#endregion


	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public UIBaseListItem getItemById(string Id){
		UIBaseListItem res = null;

		foreach(UIBaseListItem i in items){
			if(i.Id.Equals(Id)){
				res = i;
				break;
			}
		}

		return res;
	}

	public int getItemIndexById(string Id){
		int res = -1;
		
		for(int i=0; i< items.Count; i++){
			if(items[i].Id.Equals(Id)){
				res = i;
				break;
			}
		}
		
		return res;
	}

	public void updateNavigationButtons(){
		if(btnPrevItem)
			btnPrevItem.gameObject.SetActive(hasPrevious());
		if(btnNextItem)
			btnNextItem.gameObject.SetActive(hasNext());
	}

	public virtual void initIndexCurrentItem(){
		indexCurrentItem = 0;
	}

	/// <summary>
	/// Leave < 0 if we want to hanle it with the lenght of the items list. Set it if we want to specify another last item, i.e. when we want to show a list of the available missions to complete that in this case we will show only navigable unlocked levels.
	/// </summary>
	public virtual void initIndexLastItem(){
		indexLastItem = -1;
	}

	public virtual UIBaseListItem createListItem(string data){
		return new UIBaseListItem(data);
	}

	public virtual void loadItems(){
//		if(items == null || (items != null && items.Count == 0)){
		string[] allItems = FilesUtil.getLinesFromFile(textLoadFile);			
		items = new List<UIBaseListItem>();
		string newContent = "";
		string itemContent = "";
		string[] atts;
		string id = "";

		foreach(string w in allItems){
			itemContent = w;
			atts = w.Split(ATTRIBUTES_SEPARATOR);
			id = atts[0];

//			//Try to load list item from file
//			if(allowLoadFromPlayerPrefs && PlayerPrefs.HasKey(PP_KEY+id)){
//				newContent = PlayerPrefs.GetString(PP_KEY+id);
//
//				if(!string.IsNullOrEmpty(newContent))
//					itemContent = newContent;
//			}


			UIBaseListItem item = createListItem(itemContent);
			items.Add(item);
		}
//		}
	}

	public bool hasNext(){
		int lastItem = indexLastItem < 0 ? items.Count : Mathf.Clamp(indexLastItem, 0, items.Count);
		return indexCurrentItem+1 < lastItem;
	}

	public bool hasPrevious(){
		return indexCurrentItem-1 >= 0;
	}

	public virtual void nextItem(){
		if(hasNext()){
			items[indexCurrentItem].comeIn(false, false); //go out to left
			indexCurrentItem++;
			currentItemSelected = items[indexCurrentItem];
			showItem(currentItemSelected);
			currentItemSelected.comeIn(); //come in from right
			PlayerPrefs.SetString(GameSettings.PP_UNIQUE_LIST_CURRENT_ITEM_SELECTED_ID, currentItemSelected.Id); 

			//navigation Buttons
			updateNavigationButtons();
		}
	}
	
	public virtual void previousItem(){
		if(hasPrevious()){
			items[indexCurrentItem].comeIn(false); //go out to right
			indexCurrentItem--;
			currentItemSelected = items[indexCurrentItem];
			showItem(currentItemSelected);
			currentItemSelected.comeIn(true, false); //come in from left
			PlayerPrefs.SetString(GameSettings.PP_UNIQUE_LIST_CURRENT_ITEM_SELECTED_ID, currentItemSelected.Id); 

			//navigation Buttons
			updateNavigationButtons();
		}
	}
	
	public virtual void showItem(UIBaseListItem item){
		setLabelName(item);
	}

	public virtual void setLabelName(UIBaseListItem item){
		if(lbNameCurrentItem)
			lbNameCurrentItem.text = item.Name;
	}

	/// <summary>
	/// Selects current item shown or the given item
	/// </summary>
	/// <param name="item">Item.</param>
	public virtual void selectItem(UIBaseListItem item = null){

	}
}
