using UnityEngine;
using System.Collections;

public class GameMoneyManager : PersistentSingleton<GameMoneyManager> {
	public bool purchaseWithMoney(int price){
		bool purchased = false;
		int money = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_MONEY);
		
		if(money >= price){
			money -= price;
			PlayerPrefs.SetInt(GameSettings.PP_TOTAL_MONEY, money);
			purchased = true;
		}
		
		return purchased;
	}
	
	public bool purchaseWithGems(int price){
		bool purchased = false;
		int gems = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_GEMS);
		
		if(gems >= price){
			gems -= price;
			PlayerPrefs.SetInt(GameSettings.PP_TOTAL_GEMS, gems);
			purchased = true;
		}
		
		return purchased;
	}
	
	public bool hasEnoughMoney(int price){
		bool res = false;
		
		if(price > 0){
			int money = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_MONEY);
			res = money >= price;
		}
		
		return res;
	}
	
	public bool hasEnoughGems(int price, bool priceInCoins = false){
		bool res = false;
		int finalPrice = !priceInCoins ? price: getEquivalentGemsInCoins(price);
		
		if(finalPrice > 0){
			int gems = PlayerPrefs.GetInt(GameSettings.PP_TOTAL_GEMS);
			res = gems >= finalPrice;
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
			res =	hasEnoughGems(pItem.GemsPrice);
		}
		
		return res;
	}
	
	public int getEquivalentGemsInCoins(int coins){
		int res = (int)coins/GameSettings.Instance.ONE_GEM_VALUE_IN_COINS;
		
		if(res <= 0)
			res = 1;
		
		return res;
	}
	
	public int getTotalMoney(){
		return PlayerPrefs.GetInt(GameSettings.PP_TOTAL_MONEY);
	}
	
	public int getTotalGems(){
		return PlayerPrefs.GetInt(GameSettings.PP_TOTAL_GEMS);
	}
	
	public bool payWithMoney(int price){
		bool purchased = hasEnoughMoney(price);
		
		if(purchased){
			int total = getTotalMoney();
			int final = total-price;
			PlayerPrefs.SetInt(GameSettings.PP_TOTAL_MONEY, final);
		}
		
		return purchased;
	}
	
	public bool payWithGems(int price, bool priceIsInCoins = false){
		int pPrice = priceIsInCoins ? getEquivalentGemsInCoins(price) : price;
		bool purchased = hasEnoughGems(price, priceIsInCoins);
		
		if(purchased){
			int total = getTotalGems();
			int final = total-pPrice;
			PlayerPrefs.SetInt(GameSettings.PP_TOTAL_GEMS, final);
		}
		
		return purchased;
	}
	
	public void addMoney(int quantity){
		int total = getTotalMoney();
		PlayerPrefs.SetInt(GameSettings.PP_TOTAL_MONEY, total+quantity);
	}
	
	public void addGems(int quantity){
		int total = getTotalGems();
		PlayerPrefs.SetInt(GameSettings.PP_TOTAL_GEMS, total+quantity);
	}
	
	public void resetMoney(){
		PlayerPrefs.SetInt(GameSettings.PP_TOTAL_MONEY, 0);
	}
	
	public void resetGems(){
		PlayerPrefs.SetInt(GameSettings.PP_TOTAL_GEMS, 0);
	}
}
