using UnityEngine;

/* #Funfact 
 * Water doesn't move horizontally, its the waves that carry themselves through the ocean.
 */
public class WaveManager : MonoBehaviour
{
    public static WaveManager singleton;

    public Transform wave;

    Vector4 wA, wB, wC;

   
    private void Awake()
    {
        if (singleton == null)
            singleton = this;
        else if (singleton != null)
            Destroy(this);
    }

    void Start()
    {
        // Get Wave Shader To Collect Its Properties For Our Own Sine Wave Shader
        Material wave = Resources.Load<Material>("Wave");

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
        float c = Mathf.Sqrt(9.82f / k);
        float a = s / k;

        Vector2 d = new Vector2(wave.x, wave.y);
        d.Normalize();

        float dot = Vector2.Dot(d, new Vector2(point.x, point.z));
        float f = k * (dot - c * Time.time);

        return a * Mathf.Sin(f);
    }
}
