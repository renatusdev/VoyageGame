using UnityEngine;

/*
 * Wave Analysis:
 * 
 * - Direction Organization.
 *      Wave A: Determines overall direction
 *      Wave B & C: Axis of direction must have same sign (min value +-0.3).
 *      
 *      Waves Direction Example (South)
 *      WaveA.dir = (0,-1)
 *      WaveB.dir = (0.2, -0.7)
 *      WaveC.dir = (-0.3, -0.3)
 */
public class WaveManager : MonoBehaviour
{
    public static WaveManager instance;

    public Material waveMat;
    public Vector3 currWind;

    Vector4 wA, wB, wC;

    public bool debug;
   
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(this);

        UpdateWind();
        UpdateWaveData();
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            for(int y = 0; y <= 100; y += 5)
            {
                for (int x = 0; x <= 100; x += 5)
                {
                    Gizmos.DrawWireSphere(new Vector3(x, GetHeight(x,y), y), 1);
                }   
            }
        }   
    }

    public void UpdateWaveData()
    {
        wA = waveMat.GetVector("_WaveA");
        wB = waveMat.GetVector("_WaveB");
        wC = waveMat.GetVector("_WaveC");
    }

    public float GetHeight(float x, float z)
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

    // Eventually should have lerping
    private void UpdateWind()
    {
        if (currWind != WindManager.instance.GetDirection())
        {
            currWind = WindManager.instance.GetDirection();

            Vector4 wave = waveMat.GetVector("_WaveA");
            waveMat.SetVector("_WaveA", new Vector4(currWind.x, currWind.z, wave.z, wave.w));

            wave = waveMat.GetVector("_WaveB");
            waveMat.SetVector("_WaveB", new Vector4(Mathf.Sign(currWind.x) * UnityEngine.Random.Range(0.3f, 0.7f), Mathf.Sign(currWind.z) * UnityEngine.Random.Range(0.3f, 0.7f), wave.z, wave.w));

            wave = waveMat.GetVector("_WaveC");
            waveMat.SetVector("_WaveC", new Vector4(UnityEngine.Random.Range(-0.7f, 0.7f), UnityEngine.Random.Range(-0.7f, 0.7f), wave.z, wave.w));

            UpdateWaveData();
        }
    }
}
