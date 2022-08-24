using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Character��Parameter�̃R���g���[���[
/// </summary>
public class S2_ActorParamsController : MonoBehaviour
{
    public S2_Params parameter;

    /// <summary>Parameter�̃R�s�[��Ԃ�</summary>
    public S2_Params GetParameter()
    {
        S2_Params p = new S2_Params();
        p.id = parameter.id;
        p.lv = parameter.lv;
        p.hp = parameter.hp;
        p.hpmax = parameter.hpmax;
        p.str = parameter.str;
        p.exp = parameter.exp;
        p.xp = parameter.xp;
        p.def = parameter.def;
        return p;
    }

    /// <summary>Parameter���܂Ƃ߂Đݒ肷��</summary>
    public void SetParameter(S2_Params p)
    {
        parameter.id = p.id;
        parameter.lv = p.lv;
        parameter.hp = p.hp;
        parameter.hpmax = p.hpmax;
        parameter.str = p.str;
        parameter.exp = p.exp;
        parameter.xp = p.xp;
        parameter.def = p.def;
    }

    /// <summary>�_���[�W���v�Z����</summary>
    static int CalcDamage(int str, int def)
    {
        return Mathf.CeilToInt(str * Mathf.Pow(0.9375f, def));
    }

    /// <summary>�_���[�W���󂯂�</summary>
    public void Damaged(int str)
    {
        parameter.hp -= CalcDamage(str, parameter.def);
    }
}
