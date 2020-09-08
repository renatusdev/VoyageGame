using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    public int hp;
    public float speed;
    public Animator anim;
    public SpriteRenderer sR;

    public virtual void Hurt(int amount)
    {
        hp -= amount;
        if (!Die())
            anim.SetBool("isHurting", true);
    }
    
    public virtual void Hurting()
    {
        sR.color = Color.Lerp(sR.color, Color.red, Time.deltaTime * 2);

        if (Mathf.RoundToInt(sR.color.b) == 0)
            sR.color = Color.white;
    }

    public virtual bool Die()
    {
        if (!(hp <= 0))
            return false;

        hp = 0;
        anim.SetBool("isDying", true);

        Debug.Log(anim.runtimeAnimatorController.animationClips[0].length);

        Destroy(this.gameObject, anim.runtimeAnimatorController.animationClips[0].length);

        return true;

        // Temp
        //Destroy(this.gameObject, anim.get);
    }
}