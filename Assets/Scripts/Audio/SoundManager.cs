using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    private readonly static float defaultVolume = 0.2f;
    
    private static AudioSource source;

    public static void Initialize()
    {
        GameObject sound = new GameObject("Sound");
        source = sound.AddComponent<AudioSource>();
    }

    // Plays a 3D sound at a given position with the default volume (0.2f).
    public static void PlaySound(Sound sound, Vector3 pos)
    {
        GameObject obj  = new GameObject(sound.ToString());
        AudioSource src = obj.AddComponent<AudioSource>();
        AudioClip clip = GameAssets.i.GetClip(sound);

        obj.transform.position = pos;
        src.clip = clip;

        // 3D Settings
        src.maxDistance = 100;
        src.minDistance = 1;
        src.rolloffMode = AudioRolloffMode.Linear;
        src.dopplerLevel = 0;
        src.volume = defaultVolume;

        src.Play();
        GameObject.Destroy(obj, clip.length);
    }

    // Plays a 3D sound at a given position with a given volume
    public static void PlaySound(Sound sound, Vector3 pos, float volume)
    {
        GameObject obj = new GameObject(sound.ToString());
        AudioSource src = obj.AddComponent<AudioSource>();
        AudioClip clip = GameAssets.i.GetClip(sound);

        obj.transform.position = pos;
        src.clip = clip;

        // 3D Settings
        src.maxDistance = 100;
        src.minDistance = 1;
        src.rolloffMode = AudioRolloffMode.Linear;
        src.dopplerLevel = 0;
        src.volume = volume;

        src.Play();
        GameObject.Destroy(obj, clip.length);
    }

    // Plays a 2D sound with the default volume (0.2f).
    public static void PlaySound(Sound sound) { source.PlayOneShot(GameAssets.i.GetClip(sound), defaultVolume); }

    // Plays a 2D sound with a given volume
    public static void PlaySound(Sound sound, float volume) {  source.PlayOneShot(GameAssets.i.GetClip(sound), volume); }
}