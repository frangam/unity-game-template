using UnityEngine;
using System.Collections;
using UnionAssets.FLE;

public class GameMoneyManager : PersistentSingleton<GameMoneyManager> {
	//--------------------------------------
	// Constants
	//--------------------------------------
	public const string MAXIMUM_MONEY_RAISED = "gt_max_money_raised";
	public const string MAXIMUM_GEMS_RAISED = "gt_max_gems_raised";
	
	//--------------------------------------
	// Static Attributes
	//--------------------------------------
	private static EventDispatcherBase _dispatcher  = new EventDispatcherBase ();
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public bool purchaseWithMoney(long price){
		bool purchased = false;
		long money = 0;
		string savedValue = PlayerPrefs.GetString(GameSettings.PP_TOTAL_MONEY);
		
		if(long.TryParse(savedValue, out money) && (money >= price)){
			money -= price;
			PlayerPrefs.SetString(GameSettings.PP_TOTAL_MONEY, money.ToString());
			purchased = true;
		}
		
		return purchased;
	}
	
	public bool purchaseWithGems(long price){
		bool purchased = false;
		long gems = 0;
		string savedValue = PlayerPrefs.GetString(GameSettings.PP_TOTAL_GEMS);
		
		if(long.TryParse(savedValue, out gems) && (gems >= price)){
			gems -= price;
			PlayerPrefs.SetString(GameSettings.PP_TOTAL_GEMS, gems.ToString());
			purchased = true;
		}
		
		return purchased;
	}
	
	public bool maximumAmountOfMoneyRaised(){
		bool res = false;
		long money = 0;
		string savedValue = PlayerPrefs.GetString(GameSettings.PP_TOTAL_MONEY);
		
		if(long.TryParse(savedValue, out money)){
			res = money >= long.MaxValue;
		}
		
		return res;
	}
	
	public bool maximumAmountOfGemsRaised(){
		bool res = false;
		long gems = 0;
		string savedValue = PlayerPrefs.GetString(GameSettings.PP_TOTAL_GEMS);
		
		if(long.TryParse(savedValue, out gems)){
			res = gems >= long.MaxValue;
		}
		
		return res;
	}
	
	public bool hasEnoughMoney(long price){
		bool res = false;
		
		if(price > 0){
			string savedValue = PlayerPrefs.GetString(GameSettings.PP_TOTAL_MONEY);
			long money = 0;
			
			if(long.TryParse(savedValue, out money)){
				res = money >= price;
			}
		}
		
		return res;
	}
	
	public bool hasEnoughGems(long price, bool priceInCoins = false){
		bool res = false;
		long finalPrice = !priceInCoins ? price: getEquivalentGemsInCoins(price);
		
		if(finalPrice > 0){
			string savedValue = PlayerPrefs.GetString(GameSettings.PP_TOTAL_GEMS);
			long gems = 0;
			
			if(long.TryParse(savedValue, out gems)){
				res = gems >= finalPrice;
			}
		}
		
		return res;
	}
	
	public bool hasEnoughMoney(UIBasePurchasableListItem pItem){
		bool res = false;
		
		if(pItem != null){
			res = hasEnoughMoney(pItem.Price);
		}
		
		return res;
	}
	
	public bool hasEnoughGems(UIBasePurchasableListItem pItem){
		bool res = false;
		
		if(pItem != null){
			res = hasEnoughGems(pItem.GemsPrice);
		}
		
		return res;
	}
	
	public long getEquivalentGemsInCoins(long coins){
		long res = coins/GameSettings.Instance.ONE_GEM_VALUE_IN_COINS;
		
		if(res <= 0)
			res = 1;
		
		return res;
	}
	
	public long getTotalMoney(){
		long res = 0;
		string savedValue = PlayerPrefs.GetString(GameSettings.PP_TOTAL_MONEY);
		
		long.TryParse(savedValue, out res);
		
		return res; 
	}
	
	public long getTotalGems(){
		long res = 0;
		string savedValue = PlayerPrefs.GetString(GameSettings.PP_TOTAL_GEMS);
		
		long.TryParse(savedValue, out res);
		
		return res; 
	}
	
	public bool payWithMoney(long price){
		bool purchased = hasEnoughMoney(price);
		
		if(purchased){
			long total = getTotalMoney();
			long final = total-price;
			PlayerPrefs.SetString(GameSettings.PP_TOTAL_MONEY, final.ToString());
		}
		
		return purchased;
	}
	
	public bool payWithGems(long price, bool priceIsInCoins = false){
		long pPrice = priceIsInCoins ? getEquivalentGemsInCoins(price) : price;
		bool purchased = hasEnoughGems(price, priceIsInCoins);
		
		if(purchased){
			long total = getTotalGems();
			long final = total-pPrice;
			PlayerPrefs.SetString(GameSettings.PP_TOTAL_GEMS, final.ToString());
		}
		
		return purchased;
	}
	
	
	public long howToAdd(long quantity){
		long total = getTotalMoney();
		long restToMax = long.MaxValue - total;
		long moneyToAdd = 0;
		
		if(quantity <= restToMax)
			moneyToAdd = total+quantity;
		else{			
			moneyToAdd = total+restToMax;
		}
		
		return moneyToAdd;
	}
	
	public void addMoney(long quantity){
		long total = getTotalMoney();
		long restToMax = long.MaxValue - total;
		long moneyToAdd = 0;
		
		if(quantity <= restToMax)
			moneyToAdd = total+quantity;
		else{
			//maximum amount of money raised
			_dispatcher.dispatch(MAXIMUM_MONEY_RAISED);
			
			moneyToAdd = total+restToMax;
		}
		
		PlayerPrefs.SetString(GameSettings.PP_TOTAL_MONEY, moneyToAdd.ToString());
	}
	
	public void addGems(long quantity){
		long total = getTotalGems();
		long restToMax = long.MaxValue - total;
		long gemsToAdd = 0;
		
		if(quantity <= restToMax)
			gemsToAdd = total+quantity;
		else{
			//maximum amount of gems raised
			_dispatcher.dispatch(MAXIMUM_GEMS_RAISED);
			
			gemsToAdd = total+restToMax;
		}
		
		PlayerPrefs.SetString(GameSettings.PP_TOTAL_GEMS, gemsToAdd.ToString());
	}
	
	public void resetMoney(){
		PlayerPrefs.SetString(GameSettings.PP_TOTAL_MONEY, "0");
	}
	
	public void resetGems(){
		PlayerPrefs.SetString(GameSettings.PP_TOTAL_GEMS, "0");
	}
}
