using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//allows you to create an object through the assets menu 
// (where you can create a C# script in the Unity Editor)

[CreateAssetMenu(menuName = "Values/IntegerValue")]

public class IntegerValue : ScriptableObject
{
    public int InitValue;
}
