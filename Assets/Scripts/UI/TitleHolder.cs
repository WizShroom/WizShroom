using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TitleHolder", menuName = "TitleHolder")]
public class TitleHolder : ScriptableObject
{
    public string titleName;
    public TitleInformations titleInformations;

    public void ApplyStatBoost(MobController targetMob)
    {
        foreach (Stat statBuff in titleInformations.statBuffs)
        {
            targetMob.IncreaseStatValue(statBuff.GetStatType(), statBuff.GetValue());
        }
    }

    public void RemoveStatBoost(MobController targetMob)
    {
        foreach (Stat statBuff in titleInformations.statBuffs)
        {
            targetMob.IncreaseStatValue(statBuff.GetStatType(), -statBuff.GetValue());
        }
    }

}

[System.Serializable]
public struct TitleInformations
{
    public string titleDescription;
    public List<Stat> statBuffs;
}