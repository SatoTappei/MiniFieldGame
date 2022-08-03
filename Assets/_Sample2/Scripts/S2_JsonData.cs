using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S2_JsonData
{
    [System.Serializable]
    public class JsonData
    {
        [SerializeField]
        public PersonalData[] party = null;
    }

    [System.Serializable]
    public class PersonalData
    {
        [SerializeField]
        public string name = string.Empty;
        [SerializeField, Range(1, 100)]
        public int life = 1;
        [SerializeField, Range(0, 50)]
        public int attack = 0;
        [SerializeField]
        public AccessoryData[] accessories = null;
    }

    [System.Serializable]
    public struct AccessoryData
    {
        [SerializeField]
        public string name;
        [SerializeField, Range(0, 10)]
        public int defense;
    }

}
