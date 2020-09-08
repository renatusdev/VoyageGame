using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    public static GameAssets i;

    public Transform player;
    public GameObject shipHitParticleSystem;

    [SerializeField]
    private SoundClip[] soundClips;
    private Dictionary<Sound, AudioClip> libSound;

    [SerializeField]
    private CreatureObject[] createEvents;
    private Dictionary<Creature, GameObject> libCreature;

    private void Awake()
    {
        if (i == null)
            i = this;
        else
            Destroy(this);

        SetUpSound();
        SetUpCreatureEvents();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void SetUpSound()
    {
        libSound = new Dictionary<Sound, AudioClip>();

        foreach (SoundClip sC in soundClips)
            libSound.Add(sC.sound, sC.clip);

        SoundManager.Initialize();
    }

    void SetUpCreatureEvents()
    {
        libCreature = new Dictionary<Creature, GameObject>();

        foreach (CreatureObject cE in createEvents)
            libCreature.Add(cE.creature, cE.prefab);
    }

    public GameObject GetCreatureEvent(Creature c) { return libCreature[c]; }
    public AudioClip GetClip(Sound s) { return libSound[s]; }
}

[System.Serializable]
public class SoundClip
{
    public string name;
    public Sound sound;
    public AudioClip clip;
}

[System.Serializable]
public class CreatureObject
{
    public string name;
    public Creature creature;
    public GameObject prefab;
}
