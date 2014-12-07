using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAP_GameName : CoreIAPManager {
	public List<Product> products;

	void Start()
	{
		Init(products);
	}
	public override void OnProcessingPurchasedProduct (string SKU)
	{
		switch(SKU) {
		case "100_coins":
			//Consumimos nuestra compra que llamara al evento "OnProductConsumed"
			Consume("100_coins");
			break;
			//		case BONUS_TRACK:
			//			GameData.UnlockBonusTrack();
			//			break;
		}
	}

	public override void OnProcessingConsumeProduct (string SKU)
	{
		switch(SKU) {
		case "100_coins":
			//Le damos al jugador su producto.
			//GameDataExample.AddCoins(100);
			AndroidMessage.Create("Añadido al jugador", "100 Monedas de oro");
			break;
		}
	}

	public override void UpdateProducts ()
	{
		base.UpdateProducts ();
	}
}
