/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GameTemplateMenu : EditorWindow {
	
	//--------------------------------------
	//  PUBLIC METHODS
	//--------------------------------------
	
	#if UNITY_EDITOR
	
	[MenuItem("GameTemplate/► Play-Stop", false, -10000000)]
	public static void PlayFromCurrentScene()
	{
		if ( EditorApplication.isPlaying == true ){
			EditorApplication.isPlaying = false;
			return;
		}
		
		EditorApplication.SaveCurrentSceneIfUserWantsTo();
		
		List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
		foreach(string scene in GTBuildSettingsConfig.Instance.CurrentBuildPack.build.ScenesNames)
			scenes.Add(new EditorBuildSettingsScene(scene, true));
		
		EditorBuildSettings.scenes = scenes.ToArray();
		//		EditorApplication.OpenScene(scenes[0].path);
		
		EditorApplication.isPlaying = true;
	}
	
	[MenuItem("GameTemplate/► Play-Stop from Scene 0", false, -10000000)]
	public static void PlayFromPrelaunchScene()
	{
		if ( EditorApplication.isPlaying == true ){
			EditorApplication.isPlaying = false;
			return;
		}
		
		EditorApplication.SaveCurrentSceneIfUserWantsTo();
		
		List<EditorBuildSettingsScene> scenes = new List<EditorBuildSettingsScene>();
		foreach(string scene in GTBuildSettingsConfig.Instance.CurrentBuildPack.build.ScenesNames)
			scenes.Add(new EditorBuildSettingsScene(scene, true));
		
		EditorBuildSettings.scenes = scenes.ToArray();
		EditorApplication.OpenScene(scenes[0].path);
		
		EditorApplication.isPlaying = true;
	}
	
	
	[MenuItem("GameTemplate/Game Settings", false, -40)]
	public static void GameTemplateGameSettings() {
		Selection.activeObject = GameSettings.Instance;
	}
	
	[MenuItem("GameTemplate/Level Settings/Level Packs Settings", false, -30)]
	public static void GameTemplateLevelSettingsLevelPacksSettings() {
		Selection.activeObject = LevelPacks.Instance;
	}
	
	[MenuItem("GameTemplate/Analytics Settings/Google Analytics Settings", false, -29)]
	public static void GameTemplateAnalyticsSettingsGoogleAnalyticsSettings() {
		Selection.activeObject = GTGoogleAnalyticsSettings.Instance;
	}

	
	//--------------------------------------
	//  Documentation
	//--------------------------------------
	
	[MenuItem("GameTemplate/Documentation/Getting Started", false, -10)]
	public static void GettingStarted() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.boccrtu9rp44";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/All Docs Folder", false, 100)]
	public static void GameTemplateDocumentationAllDocsFolder() {
		string url = "https://drive.google.com/drive/u/0/folders/0B9SJ925Use_uSjlXZFU2d2ZfODQ";
		Application.OpenURL(url);
	}
	
	//--------------------------------------
	//  Game Settings
	//--------------------------------------	
	[MenuItem("GameTemplate/Documentation/Game Settings/Game Info")]
	public static void GameSettingsGameInfo() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.rzctmeoh3bqf";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Game Settings/App Links")]
	public static void GameSettingsAppLinks() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.lucy8bjlbfte";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Game Settings/Money Setup")]
	public static void GameSettingsMoneySetup() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.raol53p9wujm";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Game Settings/Character Control")]
	public static void GameSettingsCharacterControl() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.byo606orm9ke";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Game Settings/Ads Showing Settings")]
	public static void GameSettingsAdsShowingSettings() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.iscok2yxg05y";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Game Settings/Social Network Setup")]
	public static void GameSettingsSocialNetworkSetup() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.we2be264z1jg";
		Application.OpenURL(url);
	}
	
	[MenuItem("GameTemplate/Documentation/Game Settings/Rankings Setup")]
	public static void GameSettingsRankingsSetup() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.w86125vsifo2";
		Application.OpenURL(url);
	}
	
	[MenuItem("GameTemplate/Documentation/Game Settings/Achievements Setup")]
	public static void GameSettingsAchievementsSetup() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.48ndukc79rxb";
		Application.OpenURL(url);
	}
	
	//--------------------------------------
	//  Scene Setup
	//--------------------------------------	
	[MenuItem("GameTemplate/Documentation/Scene Setup/GameObjects Structure")]
	public static void SceneSetupGameObjectsStructure() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.cxhhvivonts2";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Scene Setup/Input Back Button")]
	public static void SceneSetupInputBackButton() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.r26uacg5sru0";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Scene Setup/ScreenController")]
	public static void SceneSetupScreenController() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.bu18imkkjhpt";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Scene Setup/Game Settings")]
	public static void SceneSetupGameSettings() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.bfc7myvis2sk";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Scene Setup/UI Controller")]
	public static void SceneSetupUIController() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.ot7dwwx2ypfq";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Scene Setup/Game Controller")]
	public static void SceneSetupGameController() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.wi5cdl57j418";
		Application.OpenURL(url);
	}
	
	//--------------------------------------
	//  UI
	//--------------------------------------	
	[MenuItem("GameTemplate/Documentation/UI/Getting Started")]
	public static void UIGettingStarted() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.d2b18jrfmexe";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Localization/Label")]
	public static void UILocalizationLabel() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.g50r16bnto2d";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Localization/Overview")]
	public static void UILocalizationOverview() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.vbxvxywj7wje";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Localization/Localization File")]
	public static void UILocalizationLocalizationFile() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.7cnuk7lz92ba";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Localization/UILocalize Component")]
	public static void UILocalizationUILocalizeComponent() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.pfhdzjkifx11";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Button/Getting Started")]
	public static void UIButtonGettingStarted() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.58jejuht5pdt";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Button/Triggers")]
	public static void UIButtonTriggers() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.uynguvz9h38r";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Button/Creating New Buttons")]
	public static void UIButtonCreatingNewButtons() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.eau7zx4km6ne";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Button/UIBaseButton")]
	public static void UIButtonUIBaseButton() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.nbd57svde5up";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Button/UIBaseSceneNavigationButton")]
	public static void UIButtonUIBaseSceneNavigationButton() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.i40buc63qcmv";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Button/UIBaseGameLogicButton")]
	public static void UIButtonUIBaseGameLogicButton() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.u3wxyh8y973e";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Button/BtnLanguage")]
	public static void UIButtonBtnLanguage() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.bpxzbx4wmh4n";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Button/BtnExit")]
	public static void UIButtonBtnExit() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.tps7yr3zvxak";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Button/BtnRateUs")]
	public static void UIButtonBtnRateUs() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.ytprv5nm9ri6";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Button/UIButtonOpenWin")]
	public static void UIButtonUIButtonOpenWin() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.dc8epnlwkdpi";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Button/UIButtonStartGame")]
	public static void UIButtonUIButtonStartGame() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.7gxsxifuwx39";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Window/Creating Windows")]
	public static void UIWindowCreatingWindows() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.u89k0nb6wogs";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/UI/Window/UIBaseWindow")]
	public static void UIWindowUIBaseWindow() {
		string url = "https://docs.google.com/document/d/1sJOc9ZO6JdmtG4RWK-EQUsf14zl-voUyS605XZ2SLbE/edit#heading=h.sop8ecehw0yl";
		Application.OpenURL(url);
	}
	
	//--------------------------------------
	//  Achievements Documentation
	//--------------------------------------
	[MenuItem("GameTemplate/Documentation/Achievements/Getting Started")]
	public static void AchievementsGettingStarted() {
		string url = "https://docs.google.com/document/d/17kOcOnMRrLfwEKrG9xs-shzGYEzBCSOTwJe8akRC0vo/edit#heading=h.ujroj168a0eg";
		Application.OpenURL(url);
	}
	
	[MenuItem("GameTemplate/Documentation/Achievements/Workflow")]
	public static void AchievementsWorkflow() {
		string url = "https://docs.google.com/document/d/17kOcOnMRrLfwEKrG9xs-shzGYEzBCSOTwJe8akRC0vo/edit#heading=h.4rrfds5d75hd";
		Application.OpenURL(url);
	}
	
	[MenuItem("GameTemplate/Documentation/Achievements/Use")]
	public static void AchievementsSetup() {
		string url = "https://docs.google.com/document/d/17kOcOnMRrLfwEKrG9xs-shzGYEzBCSOTwJe8akRC0vo/edit#heading=h.wynqvbtoesnn";
		Application.OpenURL(url);
	}
	
	//--------------------------------------
	//  Nomenclature & Standards Documentation
	//--------------------------------------
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Comments Template Use")]
	public static void NommenclatureAndStandardsCommentsTemplateUse() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.5flaugq43c77";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Classes/Class Structure")]
	public static void NommenclatureAndStandardsClassesClassStructure() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.ts6y0irny05h";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Classes/Nomenclature")]
	public static void NommenclatureAndStandardsClassesNomenclature() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.95rdei9kwsjf";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Classes/Attributes")]
	public static void NommenclatureAndStandardsClassesAttributes() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.usvto6tucq7c";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Classes/Getters & Setters")]
	public static void NommenclatureAndStandardsClassesGettersAndSetters() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.w6e26cp4z8bw";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Classes/Methods")]
	public static void NommenclatureAndStandardsClassesMethods() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.16w89dro13t";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Classes/Inheritance")]
	public static void NommenclatureAndStandardsClassesInheritance() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.87910p5jhl92";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Variables/Nomenclature")]
	public static void NommenclatureAndStandardsVariablesNomenclature() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.nwqh7g6b5iay";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Variables/Declaration")]
	public static void NommenclatureAndStandardsVariablesDeclaration() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.843ep4ak9s7p";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Enum/Nomenclature")]
	public static void NommenclatureAndStandardsEnumNomenclature() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.a1brd99zc7nc";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Enum/Declaration")]
	public static void NommenclatureAndStandardsEnumDeclaration() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.ulf7g7cjkejt";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Enum/Extern")]
	public static void NommenclatureAndStandardsEnumExtern() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.qliay8viig69";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Enum/Intern")]
	public static void NommenclatureAndStandardsEnumIntern() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.eoi9t9ibloz3";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Bool Expressions/Basic Rule")]
	public static void NommenclatureAndStandardsBoolExpressionsBasicRule() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.b9m2l6hit61e";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Bool Expressions/Stabdard Style")]
	public static void NommenclatureAndStandardsBoolExpressionsStandardStyle() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.asy4du3q83at";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Objects Auto Obtaining/Getting Started")]
	public static void NommenclatureAndStandardsObjectsAutoObtainingGettingStarted() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.r2l1ae2pekkf";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Objects Auto Obtaining/Obtaining Method")]
	public static void NommenclatureAndStandardsObjectsAutoObtainingObtainingMethod() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.iueovf6nlgo9";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Objects Auto Obtaining/Checks")]
	public static void NommenclatureAndStandardsObjectsAutoObtainingChecks() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.has2rnhya46e";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Objects Auto Obtaining/Obtaining Player")]
	public static void NommenclatureAndStandardsObjectsAutoObtainingPlayer() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.poegga7auih2";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Extensions/Introduction")]
	public static void NommenclatureAndStandardsExtensionsIntroduction() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.285fyy26rf79";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Extensions/Creation Process")]
	public static void NommenclatureAndStandardsExtensionsCreationProcess() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.phtg59o76r";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Extensions/Use")]
	public static void NommenclatureAndStandardsExtensionsUse() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.y450umntyl6f";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Debug Logs")]
	public static void NommenclatureAndStandardsDegugLogs() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.n8tb1pp7jsp0";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Events")]
	public static void NommenclatureAndStandardsEvents() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.lkr1wwq5spz7";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Nomenclature & Standards/Coroutines")]
	public static void NommenclatureAndStandardsCoroutines() {
		string url = "https://docs.google.com/document/d/1w3WNzRjEWl5lmuezFPhp94IzN4Z1dXHxB9qJJy1kMzE/edit#heading=h.gz29hmn29c1w";
		Application.OpenURL(url);
	}
	
	
	//--------------------------------------
	//  Team Work Documentation
	//--------------------------------------
	[MenuItem("GameTemplate/Documentation/Team Work/Clone Project")]
	public static void TeamWorkCloneProject() {
		string url = "https://docs.google.com/document/d/1i75de9wdUAZAAn9QEg4w82lS1dUXYnp91V8Vd0AGTYI/edit#heading=h.9jpmt3qexj6";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Team Work/Working Method")]
	public static void TeamWorkWorkingMethod() {
		string url = "https://docs.google.com/document/d/1i75de9wdUAZAAn9QEg4w82lS1dUXYnp91V8Vd0AGTYI/edit#heading=h.eqyagqsl640t";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Team Work/Solving Conflicts")]
	public static void TeamWorkSolvingConflicts() {
		string url = "https://docs.google.com/document/d/1i75de9wdUAZAAn9QEg4w82lS1dUXYnp91V8Vd0AGTYI/edit#heading=h.zag2lel58z3l";
		Application.OpenURL(url);
	}
	[MenuItem("GameTemplate/Documentation/Team Work/Switching Branch Troubles")]
	public static void TeamWorkSwitchingBranchTorubles() {
		string url = "https://docs.google.com/document/d/1i75de9wdUAZAAn9QEg4w82lS1dUXYnp91V8Vd0AGTYI/edit#heading=h.iivx76qx3kiq";
		Application.OpenURL(url);
	}
	
	#endif
	
}
