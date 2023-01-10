using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Advanced")]
public class AdvancedSpell : Spell
{
    public string key;
    public float cooldown;
    public float cooldownRemaining;
    public Sprite UIImage;
    public bool requireEnemy = false;
}