using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IAP_GameName : CoreIAPManager {
	//	void Start()
	//	{
	//		Init(products);
	//	}
	//
	//	public virtual void OnProcessingConsumeProduct (string SKU, bool success = true, bool deferred = false)
	//	{
	//		switch(SKU) {
	//		case "100_coins":
	//			//Le damos al jugador su producto.
	//			//GameDataExample.AddCoins(100);
	//#if UNITY_ANDROID
	//			AndroidMessage.Create("Añadido al jugador", "100 Monedas de oro");
	//#endif
	//#if UNITY_IPHONE
	//			IOSMessage.Create("Añadido al jugador", "100 Monedas de oro");
	//#endif
	//#if Unity_WP8
	//			WP8Message.Create("Añadido al jugador", "100 Monedas de oro");
	//#endif
	//			break;
	////		case "Arma1":
	////			//para los objetos no consumibles comprobar si se tiene (armas, logros, boosters, etc) cuando se inicia la
	////			//la tienda de android y wp8 chekean los purchases del usuario y llaman a este metodo para los objetos comprados
	////			//si no lo tiene se lo vuelve a dar (movil reiniciado, movil nuevo, etc) por si ya no lo tiene.
	////			if(!data.active("Arma1"))
	////				data.active("Arma1");
	//		}
	//	}
}
