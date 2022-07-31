using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーのUIを更新する
/// </summary>
public class PlayerUI : MonoBehaviour
{
    [SerializeField] public Text LevelText;
    [SerializeField] public Text LifeText;
    [SerializeField] public Text AttackText;
    [SerializeField] public Text ExpText;
    [SerializeField] public Text FoodText;
    [SerializeField] public Text WeaponText;

    public Player Player { get; private set; }
    
    public void Set(Player player)
    {
        Player = player;
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (Player == null) return;
        LevelText.text = Player._level.ToString();
        LifeText.text = Player._life.ToString();
        AttackText.text = Player._attack.ToString();
        ExpText.text = Player._exp.ToString();
        FoodText.text = Player._food.ToString();

        if(Player.CurrentWeapon != null)
        {
            WeaponText.text = Player.CurrentWeapon.ToString();
        }
        else
        {
            WeaponText.text = "";
        }
    }
}
