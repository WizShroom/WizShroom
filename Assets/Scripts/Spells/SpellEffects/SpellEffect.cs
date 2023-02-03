using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellEffect : ScriptableObject
{

    //#TODO: Spell compatibility, spells can be mixed into one, effects have a whitelist of other effects that are able to be mixed to, donator check if its 
    //effects can be mixed not the receiver. 
    public SpellEffectType spellEffectType;

    public int spellEffectLevel;

    public virtual void OnCreationEffect(BulletController bulletController) { }

    public virtual void OnShotEffect(BulletController bulletController) { }

    public virtual void OnFlightEffect(BulletController bulletController) { }

    public virtual void OnCollisionEffect(MobController mobHit, BulletController bulletController, Vector3 hitDirection) { }

    public virtual void OnCollisionEffect(GameObject objectHit, BulletController bulletController) { }

    public virtual void OnMobEffect(MobController casterMob, MobController affectedMob, Vector3 castDirection = default(Vector3)) { }

    public virtual void OnWorldEffect(Vector3 castPosition, Vector3 startPosition = default(Vector3)) { }

    public virtual IEnumerator ResetStoppedAgent(MobController mobToReset, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        mobToReset.navMeshAgent.isStopped = false;
    }

}