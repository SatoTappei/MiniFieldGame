using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    public string _name = "";
    public int _attack = 1;

    public void Attach(MapObjectBase obj)
    {
        obj._attack += _attack;
    }

    public void Detach(MapObjectBase obj)
    {
        obj._attack -= _attack;
    }

    public Weapon Merge(Weapon other)
    {
        var newWeapon = CreateInstance<Weapon>();
        newWeapon._name = _name;
        newWeapon._attack = _attack;
        if (other != null) newWeapon._attack += other._attack;
        return newWeapon;
    }

    public override string ToString()
    {
        return $"{_name} Ak+{_attack}";
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
