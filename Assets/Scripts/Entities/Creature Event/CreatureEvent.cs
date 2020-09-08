using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CreatureEvent
{
    public static void PlayEvent(Creature c)
    {
        GameObject o = GameObject.Instantiate(GameAssets.i.GetCreatureEvent(c));
    }
}
