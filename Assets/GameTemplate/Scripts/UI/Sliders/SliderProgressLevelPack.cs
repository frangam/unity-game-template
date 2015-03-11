using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Slider))]
public class SliderProgressLevelPack : MonoBehaviour {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private Text lbProgress;
	
	[SerializeField]
	private bool totalProgressOfAllLevelPack = false;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private Slider sdProgress;
	private float progress; //values between 0-1 1(100%)
	private int totalLevelsPack;
	
	//--------------------------------------
	// Getters & Setters
	//--------------------------------------
	public float Progress {
		get {
			return this.progress;
		}
	}
	
	public int TotalLevelsPack {
		get {
			return this.totalLevelsPack;
		}
	}
	
	//--------------------------------------
	// Public Methods
	//--------------------------------------
	public virtual void Awake ()
	{
		sdProgress = GetComponent<Slider>();

		if(totalProgressOfAllLevelPack)
			loadProgress();
	}

	public virtual void loadProgress(BaseLevelPack levelPack = null){
		//total progress of all of the level packs
		if(totalProgressOfAllLevelPack && levelPack == null && LevelPacks.Instance.packs != null && LevelPacks.Instance.packs.Count > 0){
			totalLevelsPack = LevelPacks.Instance.packs.Count;
			progress = LevelPacks.Instance.totalProgress();
		
			if(sdProgress == null)
				sdProgress = GetComponent<Slider>();
			
			if(sdProgress != null)
				sdProgress.value = LevelPacks.Instance.totalProgress();
			if(lbProgress)
				lbProgress.text = LevelPacks.Instance.totalProgressPercetage().ToString() + "%"; //(progress*100/totalLevelsPack).ToString() + "%";
		}
		else if(!totalProgressOfAllLevelPack && levelPack != null){
			if(sdProgress == null)
				sdProgress = GetComponent<Slider>();
			
			if(sdProgress != null)
				sdProgress.value = levelPack.ProgressCompleted;
			if(lbProgress)
				lbProgress.text = levelPack.PercentageProgressCompleted.ToString() + "%";
		}
	}
}
