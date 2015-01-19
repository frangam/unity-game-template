using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ZombieSniperLevelLoader : BaseLevelLoaderController {
	//--------------------------------------
	// Setting Attributes
	//--------------------------------------
	[SerializeField]
	private ZSHostage[] 		pbHostages;
	
	[SerializeField]
	private ZSZombie[] 			pbZombies;
	
	[SerializeField]
	private FPSController		player;
	
	
	[SerializeField]
	private bool 				loadZombieData = true;
	
	//--------------------------------------
	// Private Attributes
	//--------------------------------------
	private List<ZSBuildingDoor>	buildingDoors;
	private List<ZSHostage> 		hostages;
	private List<ZSZombie> 			zombies;
	
	//--------------------------------------
	// Overriden Methods
	//--------------------------------------
	public override void loadLevel (int levelId){
		string content = getLevelContent(levelId);
		hostages = new List<ZSHostage>();
		buildingDoors = new List<ZSBuildingDoor>();
		zombies = new List<ZSZombie>();
		
		if(!string.IsNullOrEmpty(content)){
			ZSLevel level = new ZSLevel(content, loadZombieData);
			currentLevel = level;
			
			if(GameSettings.Instance.showTestLogs)
				Debug.Log(level);
			
			if(level != null){
				//player pos
				if(!player.testInitialPos){
					BaseLevelPosition playerPos = level.PlayerPosition;
					player.transform.position = playerPos.transform.position;
					player.transform.rotation = playerPos.transform.rotation;
				}
				
				//spawn hostages
				if(level.HostagesPositions != null){
					foreach(BaseLevelPosition h in level.HostagesPositions){
						int zhIndex = Random.Range(0, pbHostages.Length);
						ZSHostage zh = pbHostages[zhIndex];
						ZSHostage go = zh.Spawn(h.getARandomSpawnPosition(), zh.transform.rotation); //the game object
						go.init(h); //init position
						hostages.Add(go);
					}
				}
				
				//enable building doors
				if(level.BuildingDoorsPositions != null && level.BuildingDoorsPositions.Count > 0){
					foreach(BaseLevelPosition b in level.BuildingDoorsPositions){
						foreach(BaseLevelPosition d in BaseLevelPositionsManager.Instance.Positions){
							if(d.GetType() == typeof(ZSBuildingDoor)){
								ZSBuildingDoor bd = (ZSBuildingDoor) d;
								
								if(bd != null && b.Id == d.Id){
									bd.gameObject.SetActive(true);
									buildingDoors.Add(bd);
								}
								else if(bd != null && b.Id != d.Id){
									bd.gameObject.SetActive(false);
								}
							}
						}
					}
				}
				else{
					foreach(BaseLevelPosition d in BaseLevelPositionsManager.Instance.Positions){
						if(d.GetType() == typeof(ZSBuildingDoor)){
							ZSBuildingDoor bd = (ZSBuildingDoor) d;
							
							if(bd != null){
								bd.gameObject.SetActive(false);
							}
						}
					}
				}
				
				//spawn zombies
				if(level.ZombiesData != null){
					foreach(ZSZombieData z in level.ZombiesData){
						int zIndex = Random.Range(0, pbZombies.Length);
						
						ZSZombie zombie = pbZombies[zIndex];
						ZSZombie go = zombie.Spawn(z.Position.getARandomSpawnPosition(), zombie.transform.rotation); //the game object
						
						if(z.GetTargetType != null && z.TargetPosition != null){
							Transform target = getTargetToFollow(z.GetTargetType, z.TargetPosition.Id); //get target to follow
							go.init(z, target); //init
						}
						else
							go.init(z);
						
						zombies.Add(go);
					}
				}
				
			}
		}
		
		
		base.loadLevel(levelId);
	}
	
	public override void loadAllOfLevels (){
		string[] content = getContentOfAllLevels();
		levels = new List<BaseLevel>();
		
		foreach(string line in content){
			ZSLevel zl = new ZSLevel(line, loadZombieData);
			
			if(zl != null)
				levels.Add(zl);
		}
		
		base.loadAllOfLevels();
	}
	
	private ZSHostage getHostageByPosId(string id){
		ZSHostage hostage = null;
		
		foreach(ZSHostage h in hostages){
			if(h.Position.Id == id){
				hostage = h;
				break;
			}
		}
		
		return hostage;
	}
	
	private ZSBuildingDoor getBuildingByPosId(string id){
		ZSBuildingDoor res = null;
		
		foreach(ZSBuildingDoor b in buildingDoors){
			if(b.Id == id){
				res = b;
				break;
			}
		}
		
		return res;
	}
	
	private Transform getTargetToFollow(ZSZombieData.TargetType type, string id){
		Transform res = null;
		
		switch(type){
		case ZSZombieData.TargetType.HOSTAGE: 
			ZSHostage h = getHostageByPosId(id); 
			
			if(h != null)
				res = h.transform;
			break;
		case ZSZombieData.TargetType.BUILDING: 
			ZSBuildingDoor b = getBuildingByPosId(id); 
			
			if(b != null)
				res = b.transform;
			break;
		}
		
		return res;
	}
}
