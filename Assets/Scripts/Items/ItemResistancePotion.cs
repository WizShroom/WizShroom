using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResistancePotion", menuName = "Items/Potions/Resistance")]
public class ItemResistancePotion : Item
{

    public List<ResistanceStat> resistancesToIncrease;

    public int increaseAmount;
    public float increaseDuration;

    public override bool UseItem(MobController mobController)
    {
        foreach (ResistanceStat resistanceStat in resistancesToIncrease)
        {
            ResistanceBuff buffToGive = new ResistanceBuff(resistanceStat.GetResistanceType().ToString(), increaseDuration, increaseAmount, resistanceStat.GetResistanceType(), mobController);
            mobController.ApplyBuff(buffToGive);
        }
        return true;
    }

}
