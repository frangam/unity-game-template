using UnityEngine;
using System;

public class LevelScore : IComparable<LevelScore> {
	private int level;
	private int score;

	public LevelScore (int level, int score){
		this.level = level;
		this.score = score;
	}
	

	public int Level {
		get {
			return this.level;
		}
		set {
			level = value;
		}
	}

	public int Score {
		get {
			return this.score;
		}
		set {
			score = value;
		}
	}



	public int CompareTo (LevelScore other){
		int res = 0;

		if(level == other.level)
			res = score.CompareTo(other.score);
		else
			level.CompareTo (other.level);

		return res;
	}


	public override string ToString (){
		return string.Format ("[LevelScore: level={0}, score={1}]", level, score);
	}
	


}
