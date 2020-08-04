using UnityEngine;

/*
 * TODO: Theorize a better wave to attain the wave data relative to ... ?
 */ 
public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public Material wave;

    Vector4 wA, wB, wC;
   
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this);

        UpdateWaveData();
    }

    public void UpdateWaveData()
    {
        // Get Wave Shader To Collect Its Properties For Our Own Sine Wave Shader
        wA = wave.GetVector("_WaveA");
        wB = wave.GetVector("_WaveB");
        wC = wave.GetVector("_WaveC");
    }

    public float getHeight(float x, float z)
    {
        Vector3 p = new Vector3(x, 0, z);
        float y = 0;
        
        y += GerstnerWave(wA, p);
        y += GerstnerWave(wB, p);
        y += GerstnerWave(wC, p);

        return y;
    }

    float GerstnerWave(Vector4 wave, Vector3 point)
    {
        float s = wave.z;
        float k = (2 * Mathf.PI) / wave.w;
        float c = Mathf.Sqrt(Mathf.Abs(Physics.gravity.y) / k);
        float a = s / k;

        Vector2 d = new Vector2(wave.x, wave.y);
        d.Normalize();

        float dot = Vector2.Dot(d, new Vector2(point.x, point.z));
        float f = k * (dot - c * Time.time);

        return a * Mathf.Sin(f);
    }
}
