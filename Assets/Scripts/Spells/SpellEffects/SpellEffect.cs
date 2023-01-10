using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffect : ScriptableObject
{
    public SpellEffectType spellEffectType;

    public virtual void OnCreationEffect(BulletController bulletController) { }

    public virtual void OnShotEffect(BulletController bulletController) { }

    public virtual void OnFlightEffect(BulletController bulletController) { }

    public virtual void OnCollisionEffect(MobController mobHit, BulletController bulletController) { }

    public virtual void OnCollisionEffect(GameObject objectHit, BulletController bulletController) { }

    public virtual void OnMobEffect(MobController affectedMob, Vector3 castDirection = default(Vector3)) { }

}