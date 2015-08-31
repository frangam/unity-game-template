/***************************************************************************
Project:     Game Template
Copyright (c) Frills Games
Author:       Francisco Manuel Garcia Moreno (garmodev@gmail.com)
***************************************************************************/
using UnityEngine;
using System;
using System.Collections;

public class MyGenericType : IEquatable<MyGenericType>, IComparable<MyGenericType> {
	#region OPERATORS
	
	public static bool operator !=(MyGenericType c1, MyGenericType c2)
	{
		return false;
	}
	
	public static bool operator <(MyGenericType c1, MyGenericType c2)
	{
		return false;
	}
	
	public static bool operator <=(MyGenericType c1, MyGenericType c2)
	{
		return false;
	}
	
	public static bool operator ==(MyGenericType c1, MyGenericType c2)
	{
		return false;
	}
	
	public static bool operator >=(MyGenericType c1, MyGenericType c2)
	{
		return false;
	}
	
	public static bool operator >(MyGenericType c1, MyGenericType c2)
	{
		return false;
	}
	
	public static MyGenericType operator +(MyGenericType c1, MyGenericType c2)
	{
		return null;
	}
	public static MyGenericType operator -(MyGenericType c1, MyGenericType c2)
	{
		return null;
	}
	public static MyGenericType operator *(MyGenericType c1, MyGenericType c2)
	{
		return null;
	}
	public static MyGenericType operator /(MyGenericType c1, MyGenericType c2)
	{
		return null;
	}
	public static MyGenericType operator ++(MyGenericType c1)
	{
		return null;
	}
	
	public static MyGenericType operator --(MyGenericType c1)
	{
		return null;
	}
	
	#endregion
	
	#region Conversions
	public static implicit operator MyGenericType(byte d)
	{
		return null;
	} 
	
	public static explicit operator MyGenericType(short d)
	{
		return null;
	}
	
	public static implicit operator MyGenericType(int d)
	{
		return null;
	} 
	
	public static explicit operator MyGenericType(long d)
	{
		return null;
	}
	
	public static explicit operator MyGenericType(float d)
	{
		return null;
	}
	
	public static explicit operator MyGenericType(double d)
	{
		return null;
	} 

	public static explicit operator sbyte(MyGenericType d)
	{            return 0;        }
	public static explicit operator short(MyGenericType d)
	{            return 0;        }
	public static explicit operator int(MyGenericType d)
	{            return 0;        }
	public static explicit operator long(MyGenericType d)
	{            return 0;        }
	
	
	public static explicit operator byte(MyGenericType d)
	{            return 0;        }
	public static explicit operator ushort(MyGenericType d)
	{            return 0;        }
	public static explicit operator uint(MyGenericType d)
	{            return 0;        }
	public static explicit operator ulong(MyGenericType d)
	{            return 0;        }
	
	public static explicit operator float(MyGenericType d)
	{            return 0;        }
	public static explicit operator double(MyGenericType d)
	{            return 0; 
	}
	
	#endregion
	
	#region IComparable<ExpandDud> Members
	
	int IComparable<MyGenericType>.CompareTo(MyGenericType other)
	{
		return 0;
	}
	
	#endregion
	
	#region IEquatable<ExpandDud> Members
	
	bool IEquatable<MyGenericType>.Equals(MyGenericType other)
	{
		return false;
	}
	
	#endregion
	
	public override bool Equals(object obj)
	{
		//Removes compiler warning about ==
		return base.Equals(obj);
	}
	
	public override int GetHashCode()
	{
		//Removes compiler warning about .Equals
		return base.GetHashCode();
	}
}
