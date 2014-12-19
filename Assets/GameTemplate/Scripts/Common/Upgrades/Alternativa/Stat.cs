using UnityEngine;
using System.Collections;

[System.Serializable]
public class Stat {

	[SerializeField]private string _name;
	[SerializeField]private int _level;
	[SerializeField]private int _maxLevel;
	[SerializeField]private float _value;
	[SerializeField]private float _maxValue;

	public string Name
	{
		get { return _name; }
		set { _name = value; }
	}
	public int Level
	{
		get { return _maxLevel; }
		set { _maxLevel = value; }
	}
	public int MaxLevel
	{
		get { return _level; }
		set { _level = value; }
	}
	public float Value
	{
		get { return _value; }
		set { _value = value; }
	}
	public float MaxValue
	{
		get { return _maxValue; }
		set { _maxValue = value; }
	}
	public void LevelUp()
	{
		if(Level < MaxLevel)
		{
			Level++;
			Value = (Level/MaxLevel) * MaxValue;
		}
	}
}
