/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public static class ListsUtil {
//	/// <summary>
//	/// Shuffle the specified list.
//	/// 
//	/// Usage:
//	/// List<Product> products = GetProducts();
//	/// products.Shuffle();
//	/// </summary>
//	/// <param name="list">List.</param>
//	/// <typeparam name="T">The 1st type parameter.</typeparam>
//	public static void Shuffle<T>(this IList<T> list)  
//	{  
//		Random rng = new Random();  
//		int n = list.Count;  
//		while (n > 1) {  
//			n--;  
//			int k = rng.Next(n + 1);  
//			T value = list[k];  
//			list[k] = list[n];  
//			list[n] = value;  
//		}  
//	}

	/// <summary>
	/// Shuffle the specified list.
	/// 
	/// Usage:
	/// List<Product> products = GetProducts();
	/// products.Shuffle();
	/// </summary>
	/// <param name="list">List.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static void Shuffle<T>(this IList<T> list)
	{
		RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
		int n = list.Count;
		while (n > 1)
		{
			byte[] box = new byte[1];
			do provider.GetBytes(box);
			while (!(box[0] < n * (Byte.MaxValue / n)));
			int k = (box[0] % n);
			n--;
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}
