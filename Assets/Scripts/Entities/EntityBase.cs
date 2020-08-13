using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBase : MonoBehaviour
{
    public int hp;
    public float speed;
    public SpriteRenderer sR;

    public virtual void Hurt(int amount)
    {

    }

    public virtual void Die()
    {
        if (!(hp <= 0))
            return;

        hp = 0;
    }

    public virtual void Recover(int amount)
    {

    }
}