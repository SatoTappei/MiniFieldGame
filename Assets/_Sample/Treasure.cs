using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �󔠂𐧌䂷��
/// </summary>
public class Treasure : MapObjectBase
{
    public enum Type
    {
        LifeUp,
        FoodUp,
        Weapon,
    }
    public Type _currentType = Type.LifeUp;
    public int _value = 5;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
