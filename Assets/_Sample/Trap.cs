using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MapObjectBase
{
    public enum Type
    {
        LifeDown,
        FoodDown,
    }

    public Type _currentType = Type.LifeDown;
    public int _value = 5;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
