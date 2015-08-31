/***************************************************************************
Project:    Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System.Collections;

/// <summary>
/// Util class for store a several generic sound ids there are in almost games
/// 
/// Extend from it to have more ids
/// </summary>
public static class BaseSoundIDs {
	public const string MENU_MUSIC			 				= "bsi_000000";
	public const string GAME_MUSIC			 				= "bsi_000001";
	public const string PAUSE_MUSIC							= "bsi_000002";
	public const string GAMEOVER_MUSIC			 			= "bsi_000003";
	public const string WINNERMUSIC			 				= "bsi_000004";
	public const string MISSION_COMPLETED_MUSIC			 	= "bsi_000005";
	public const string MISSION_FAILED_MUSIC			 	= "bsi_000006";
	public const string ALL_MISSIONS_COMPLETED_MUSIC		= "bsi_000007";
	public const string SHOP_MUSIC							= "bsi_000008";
	
	
	public const string UI_BUTTON_CLICK_FX			 		= "bsi_000100";
	public const string UI_CANCEL_BUTTON_CLICK_FX			= "bsi_000101";
	public const string UI_BACK_BUTTON_CLICK_FX			 	= "bsi_000102";
	public const string UI_OPEN_WINDOW_FX			 		= "bsi_000103";
	public const string UI_OPEN_CONFIRMATION_WINDOW_FX		= "bsi_000104";
	public const string UI_CLOSE_WINDOW_FX					= "bsi_000105";
	public const string INCREASE_SCORE_FX					= "bsi_000106";
	public const string COLLECT_COINS_FX					= "bsi_000107";
	public const string COLLECT_GEMS_FX						= "bsi_000108";
	public const string GET_A_STAR_FX						= "bsi_000109";
	public const string PURCHASE_ITEM_FX					= "bsi_000110";
	public const string CLAIM_BILLS_FX						= "bsi_000111";
	
	
	public const string BASIC_HIT_FX						= "bsi_000200";
	public const string BASIC_PLAYER_LOST_LIFE_FX			= "bsi_000201";
}
