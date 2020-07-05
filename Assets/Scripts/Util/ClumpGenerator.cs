//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[System.Serializable]
//public class ClumpGenerator : MonoBehaviour
//{
//    [System.Serializable]
//    public struct Clump
//    {
//        public string name;
//        public SpawnStyle spawnStyle;
//        public FrequencyDistance frequencyAndDistance;
//        public GameObject[] gameObjects;

//        [Tooltip("Size applicable of all clumps")]
//        [Range(0,2)] public float scale;

//        [Tooltip("The amount of spawning per spawn point.")]
//        public int amountPerPoint;
        
//        [Tooltip("Adjusts the surface with the bottom of this Clump.")]
//        public float groundOverlap;
        
//        [Tooltip("Minimum world y-pos this Clump is allowed to live in.")]
//        public float minHeight;
        
//        [Tooltip("When instantiating, there's a chance the object is inclinated (depends on the surface). This integer verifies" +
//            "how much are you willing to incline the object.")]
//        public int maxInclination;
        
//        [Tooltip("The instantiation offset from the original position. \n" +
//            "NOTE: The higher the spread, the higher chance a spawn will be skipped," +
//            "IF the layermask surface isn't infinite.")]
//        public int spread;

//    }

//    private readonly static float DEBUG_Y = 1f;
//    private readonly static int INITIAL_Y = 50;

//    // Manipulate params for visual understanding
//    public enum SpawnStyle { GoldenRatio, CircleSpiral, UniformDistribution, TrigonalPlanar, X, LeftSide, RightSide, Random1, Random2, PacMan }
//    public enum DrawType { SpawnLocation, None }
//    public DrawType draw;
//    public SpawnStyle style;
//    public FrequencyDistance frequencyAndDistance;

//    private int spawnPoints;
//    private float scale;

//    public LayerMask mask;
//    public Clump[] clumps;

//    private bool failedSpawn;

//    public void GenerateCollection()
//    {
//        failedSpawn = false;

//        foreach (Clump c in clumps)
//        {
//            // Override previous clumps if any.
//            for (int i = 0; i < transform.childCount; i++)
//                if (transform.GetChild(i).name.Equals(c.name))
//                    DestroyImmediate(transform.GetChild(i).gameObject);

//            // Null or zero exceptions. Idk how to call it.
//            if (c.amountPerPoint == 0)
//            {
//                Debug.Log(string.Format("{0} has 0 amount per point. \n Generation stopped.", c.name));
//                continue;
//            }
//            if (c.scale == 0)
//            {
//                Debug.Log(string.Format("{0} has scale of 0. \n Generation stopped", c.name));
//                continue;
//            }

//            // Instantiate clump parent.
//            GameObject clumpGroup = new GameObject(c.name);
//            clumpGroup.transform.SetParent(transform);
//            clumpGroup.transform.localScale = Vector3.one * c.scale;
//            clumpGroup.transform.position = new Vector3(transform.position.x, transform.position.y + INITIAL_Y, transform.position.z);
            
//            // Obtain Frequency and Distance for each clump
//            SpawnStyle style = c.spawnStyle;

//            spawnPoints = c.frequencyAndDistance.GetFrequency();
//            scale = c.frequencyAndDistance.GetDistance();

//            // Spawn Style Picker. The Dark Maths.
//            switch (style)
//            {
//                case SpawnStyle.GoldenRatio:
//                    {
//                        float golden_ratio = (1 + Mathf.Sqrt(5)) * 0.5f;
//                        StandardGenerationLoop(c, golden_ratio, clumpGroup.transform);
//                        break;
//                    }
//                case SpawnStyle.CircleSpiral:
//                    {
//                        float circle_spiral = Mathf.Sqrt(Mathf.PI) * 2;
//                        StandardGenerationLoop(c, circle_spiral, clumpGroup.transform);
//                        break;
//                    }
//                case SpawnStyle.UniformDistribution:
//                    {
//                        float irrFraction = (Mathf.PI * 15) / (4 * spawnPoints);
//                        StandardGenerationLoop(c, irrFraction, clumpGroup.transform);
//                        break;
//                    }
//                case SpawnStyle.TrigonalPlanar:
//                    {
//                        float irrFraction = (Mathf.PI * 17) / (4 * spawnPoints);
//                        StandardGenerationLoop(c, irrFraction, clumpGroup.transform);
//                        break;
//                    }
//                case SpawnStyle.X:
//                    {
//                        float irrFraction = (Mathf.PI * 13) / (4 * spawnPoints);
//                        StandardGenerationLoop(c, irrFraction, clumpGroup.transform);
//                        break;
//                    }
//                case SpawnStyle.LeftSide:
//                    {
//                        float irrFraction = (Mathf.PI * 7) / (4 * spawnPoints);

//                        for (int i = 0; i < spawnPoints; i += 5)
//                        {
//                            float distance = (float)i / (spawnPoints - 1);  // [0,1] for trigs.
//                            float angle = 2 * Mathf.PI * irrFraction * i;

//                            float x = distance * -Mathf.Abs(Mathf.Cos(angle));
//                            float z = distance * -Mathf.Sin(angle);

//                            Vector3 pos = new Vector3(x, 0, z) * scale * transform.localScale.magnitude;
//                            Generate(c, pos + clumpGroup.transform.position, c.amountPerPoint, clumpGroup.transform);
//                        }
//                        break;
//                    }
//                case SpawnStyle.RightSide:
//                    {
//                        float irrFraction = (Mathf.PI * 7) / (4 * spawnPoints);

//                        for (int i = 0; i < spawnPoints; i += 5)
//                        {
//                            float distance = (float)i / (spawnPoints - 1);  // [0,1] for trigs.
//                            float angle = 2 * Mathf.PI * irrFraction * i;

//                            float x = distance * Mathf.Abs(Mathf.Cos(angle));
//                            float z = distance * Mathf.Sin(angle);

//                            Vector3 pos = new Vector3(x, 0, z) * scale * transform.localScale.magnitude;
//                            Generate(c, pos + clumpGroup.transform.position, c.amountPerPoint, clumpGroup.transform);
//                        }
//                        break;
//                    }
//                case SpawnStyle.Random1:
//                    {
//                        float arched = Mathf.Sqrt(Mathf.PI) * 0.004f;

//                        for (int i = 0; i < spawnPoints; i += 5)
//                        {
//                            float distance = (float)i / (spawnPoints - 1);  // [0,1] for trigs.
//                            float angle = 2 * Mathf.PI * arched * i;

//                            float x = distance * Mathf.Cos(angle * i);
//                            float z = distance * Mathf.Sin(angle * i);

//                            Vector3 pos = new Vector3(x, 0, z) * scale * transform.localScale.magnitude;
//                            Generate(c, pos + clumpGroup.transform.position, c.amountPerPoint, clumpGroup.transform);
//                        }
//                        break;
//                    }
//                case SpawnStyle.Random2:
//                    {
//                        float arched = Mathf.Sqrt(Mathf.PI) * 0.007f;

//                        for (int i = 0; i < spawnPoints; i += 5)
//                        {
//                            float distance = (float)i / (spawnPoints - 1);  // [0,1] for trigs.
//                            float angle = 2 * Mathf.PI * arched * i;

//                            float x = distance * Mathf.Cos(angle * i);
//                            float z = distance * Mathf.Sin(angle * i);

//                            Vector3 pos = new Vector3(x, 0, z) * scale * transform.localScale.magnitude;
//                            Generate(c, pos + clumpGroup.transform.position, c.amountPerPoint, clumpGroup.transform);
//                        }
//                        break;
//                    }
//                case SpawnStyle.PacMan:
//                    {
//                        float irrFraction = (Mathf.PI * 13) / (4 * spawnPoints);

//                        for (int i = 0; i < spawnPoints; i += 5)
//                        {
//                            float distance = (float)i / (spawnPoints - 1);  // [0,1] for trigs.
//                            float angle = 2 * Mathf.PI * irrFraction * i;

//                            float x = distance * Mathf.Abs(Mathf.Cos(angle));
//                            float z = distance * Mathf.Sin(angle);

//                            Vector3 pos = new Vector3(x, 0, z) * scale * transform.localScale.magnitude;
//                            Generate(c, pos + clumpGroup.transform.position, c.amountPerPoint, clumpGroup.transform);
//                        }
//                        break;
//                    }
//            }
//        }
//    }

//    void StandardGenerationLoop(Clump c, float irrFraction, Transform clumpGroup)
//    {
//        int i = 0;
//        int counter = 0;

//        while(counter < spawnPoints)
//        {
//            // If the generation failed, skip a few points.
//            if (failedSpawn) { i += 5; failedSpawn = false; }

//            i += 5;
//            float distance = (float)i / (spawnPoints - 1);  // [0,1] for trigs.
//            float angle = 2 * Mathf.PI * irrFraction * i;
            
//            float x = distance * Mathf.Cos(angle);
//            float z = distance * Mathf.Sin(angle);

//            Vector3 pos = new Vector3(x, 0, z) * scale * transform.localScale.magnitude;
//            Generate(c, pos + clumpGroup.position, c.amountPerPoint, clumpGroup);

//            counter++;
//        }
//    }

//    void Generate(Clump clump, Vector3 pos, int amountPerPoint, Transform parent)
//    {
//        // Instantiate with random offsets
//        for(int i = 0; i < amountPerPoint; i++)
//        {
//            float xOffset = ((float)RandomnessGenerator.rnd.NextDouble() * 2 - 1) * clump.spread;
//            float zOffset = ((float)RandomnessGenerator.rnd.NextDouble() * 2 - 1) * clump.spread;

//            Vector3 offset = new Vector3(xOffset, 0, zOffset);
//            Vector3 newPos = pos + offset;

//            GameObject obj = Instantiate(clump.gameObjects[RandomnessGenerator.rnd.Next(0, clump.gameObjects.Length)], newPos, Quaternion.identity,parent);

//            if (!SetInclination(obj.transform, clump))
//            {
//                failedSpawn = true;
//                break;
//            }
//        }
//    }

//    bool SetInclination(Transform obj, Clump clump)
//    {
//        // Cast a downwards ray.
//        RaycastHit hit;
//        Physics.Raycast(obj.position, Vector3.down, out hit, 1000, mask);

//        // Delete if ray is null, because that implies the object is below terrain.
//        if (hit.collider == null)                                               { DestroyImmediate(obj.gameObject); return false; }

//        // Angle between surface normal and object's transform.down.
//        float inclAngle = Vector3.Angle(hit.normal, Vector3.up);

//        // Delete if object inclination is above its maximum inclination.
//        if (inclAngle >= clump.maxInclination)                                  { DestroyImmediate(obj.gameObject); return false; }

//        // Object height overlap adjustment.
//        obj.position = hit.point + hit.normal.normalized * clump.groundOverlap;

//        // Delete if object y position is below its minimum height.
//        if (obj.transform.position.y + clump.groundOverlap <= clump.minHeight)  { DestroyImmediate(obj.gameObject); return false; }

//        // Random y-rot;
//        obj.rotation = Quaternion.Euler(obj.rotation.eulerAngles.x, RandomnessGenerator.rnd.Next(0,360), obj.rotation.eulerAngles.z);

//        // Inclination.
//        Quaternion incl = Quaternion.FromToRotation(transform.up, hit.normal);

//        // Set Rotation
//        obj.rotation = incl * obj.rotation;

//        return true;
//    }

//    private void OnDrawGizmos()
//    {
//        if (draw.Equals(DrawType.SpawnLocation))
//        {
//            Gizmos.color = Color.red;
            
//            spawnPoints = frequencyAndDistance.GetFrequency();
//            scale = frequencyAndDistance.GetDistance();

//            float distance;
//            float angle;

//            switch (style)
//            {
//                case SpawnStyle.GoldenRatio:
//                {
//                    float golden_ratio = (1 + Mathf.Sqrt(5)) * 0.5f;

//                    for (int i = 0; i < spawnPoints; i += 5)
//                    {   
//                        distance = (float)i / (spawnPoints - 1);  // [0,1] for trigs.
//                        angle = 2 * Mathf.PI * golden_ratio * i;

//                        float x = distance * Mathf.Cos(angle);
//                        float z = distance * Mathf.Sin(angle);

//                        Gizmos.DrawSphere((new Vector3(x, DEBUG_Y, z) * scale * transform.localScale.magnitude) + transform.position, 1);
//                    }
//                    break;
//                }
//                case SpawnStyle.CircleSpiral:
//                {
//                    float circle_spiral = Mathf.Sqrt(Mathf.PI) * 2;

//                    for (int i = 0; i < spawnPoints; i += 5)
//                    {
//                        distance = (float)i / (spawnPoints - 1);  
//                        angle = 2 * Mathf.PI * circle_spiral * i;

//                        float x = distance * Mathf.Cos(angle);
//                        float z = distance * Mathf.Sin(angle);

//                        Gizmos.DrawSphere((new Vector3(x, DEBUG_Y, z) * scale * transform.localScale.magnitude) + transform.position, 1);
//                    }
//                    break;
//                }
//                case SpawnStyle.UniformDistribution:
//                {
//                    float irrFraction = (Mathf.PI * 15)/(4*spawnPoints);

//                    for (int i = 0; i < spawnPoints; i += 5)
//                    {
//                        distance = (float)i / (spawnPoints - 1);  
//                        angle = 2 * Mathf.PI * irrFraction * i;

//                        float x = distance * Mathf.Cos(angle);
//                        float z = distance * Mathf.Sin(angle);

//                        Gizmos.DrawSphere((new Vector3(x, DEBUG_Y, z) * scale * transform.localScale.magnitude) + transform.position, 1);
//                    }
//                    break;
//                }
//                case SpawnStyle.TrigonalPlanar:
//                {
//                    float irrFraction = (Mathf.PI * 17) / (4 * spawnPoints);

//                    for (int i = 0; i < spawnPoints; i += 5)
//                    {
//                        distance = (float)i / (spawnPoints - 1);  
//                        angle = 2 * Mathf.PI * irrFraction * i;

//                        float x = distance * Mathf.Cos(angle);
//                        float z = distance * Mathf.Sin(angle);

//                        Gizmos.DrawSphere((new Vector3(x, DEBUG_Y, z) * scale * transform.localScale.magnitude) + transform.position, 1);
//                    }
//                    break;
//                }
//                case SpawnStyle.X:
//                {
//                    float irrFraction = (Mathf.PI * 13) / (4 * spawnPoints);

//                    for (int i = 0; i < spawnPoints; i += 5)
//                    {
//                        distance = (float)i / (spawnPoints - 1);  
//                        angle = 2 * Mathf.PI * irrFraction * i;

//                        float x = distance * Mathf.Cos(angle);
//                        float z = distance * Mathf.Sin(angle);

//                        Gizmos.DrawSphere((new Vector3(x, DEBUG_Y, z) * scale * transform.localScale.magnitude) + transform.position, 1);
//                    }
//                    break;
//                }
//                case SpawnStyle.LeftSide:
//                {
//                    float irrFraction = (Mathf.PI * 7) / (4 * spawnPoints);

//                    for (int i = 0; i < spawnPoints; i += 5)
//                    {
//                        distance = (float)i / (spawnPoints - 1);  
//                        angle = 2 * Mathf.PI * irrFraction * i;

//                        float x = distance * -Mathf.Abs(Mathf.Cos(angle));
//                        float z = distance * -Mathf.Sin(angle);

//                        Gizmos.DrawSphere((new Vector3(x, DEBUG_Y, z) * scale * transform.localScale.magnitude) + transform.position, 1);
//                    }
//                    break;
//                }
//                case SpawnStyle.RightSide:
//                {
//                    //float irrFraction = (Mathf.PI * 13) / (4 * spawnPoints);
//                    float irrFraction = (Mathf.PI * 7) / (4 * spawnPoints);

//                    for (int i = 0; i < spawnPoints; i += 5)
//                    {
//                        distance = (float)i / (spawnPoints - 1);  
//                        angle = 2 * Mathf.PI * irrFraction * i;

//                        float x = distance * Mathf.Abs(Mathf.Cos(angle));
//                        float z = distance * Mathf.Sin(angle);

//                        Gizmos.DrawSphere((new Vector3(x, DEBUG_Y, z) * scale * transform.localScale.magnitude) + transform.position, 1);
//                    }
//                    break;
//                }
//                case SpawnStyle.Random1:
//                {
//                    float arched = Mathf.Sqrt(Mathf.PI) * 0.004f;

//                    for (int i = 0; i < spawnPoints; i += 5)
//                    {
//                        distance = (float)i / (spawnPoints - 1);  
//                        angle = 2 * Mathf.PI * arched * i;

//                        float x = distance * Mathf.Cos(angle * i);
//                        float z = distance * Mathf.Sin(angle * i);

//                        Gizmos.DrawSphere((new Vector3(x, DEBUG_Y, z) * scale * transform.localScale.magnitude) + transform.position, 1);
//                    }
//                    break;
//                }
//                case SpawnStyle.Random2:
//                {
//                    float arched = Mathf.Sqrt(Mathf.PI) * 0.007f;

//                    for (int i = 0; i < spawnPoints; i += 5)
//                    {
//                        distance = (float)i / (spawnPoints - 1);  
//                        angle = 2 * Mathf.PI * arched * i;

//                        float x = distance * Mathf.Cos(angle * i);
//                        float z = distance * Mathf.Sin(angle * i);

//                        Gizmos.DrawSphere((new Vector3(x, DEBUG_Y, z) * scale * transform.localScale.magnitude) + transform.position, 3);
//                    }
//                    break;
//                }
//                case SpawnStyle.PacMan:
//                {
//                    float irrFraction = (Mathf.PI * 13) / (4 * spawnPoints);

//                    for (int i = 0; i < spawnPoints; i += 5)
//                    {
//                        distance = (float)i / (spawnPoints - 1);
//                        angle = 2 * Mathf.PI * irrFraction * i;

//                        float x = distance * Mathf.Abs(Mathf.Cos(angle));
//                        float z = distance * Mathf.Sin(angle);

//                        Gizmos.DrawSphere((new Vector3(x, DEBUG_Y, z) * scale * transform.localScale.magnitude) + transform.position, 1);
//                    }
//                    break;
//                }
//            }
//        }
//    }
//}
//[System.Serializable]
//public struct FrequencyDistance
//{
//    private readonly static int highFreq = 200;
//    private readonly static int midFreq = 100;
//    private readonly static int lowFreq = 50;
    
//    private readonly static int highDist = 50;
//    private readonly static int midDist = 35;
//    private readonly static int lowDist = 20;

//    public enum FrequencyType { HighFreq, MidFreq, LowFreq }
//    public enum DistanceType { HighDist, MidDist, LowDist }
//    public FrequencyType frequency_type;
//    public DistanceType distance_type;

//    public int GetFrequency()
//    {
//        if (frequency_type.Equals(FrequencyType.HighFreq))
//            return highFreq;
//        else if (frequency_type.Equals(FrequencyType.MidFreq))
//            return midFreq;
//        else 
//            return lowFreq;
//    }

//    public int GetDistance()
//    {
//        if (distance_type.Equals(DistanceType.HighDist))
//            return highDist;
//        else if (distance_type.Equals(DistanceType.MidDist))
//            return midDist;
//        else
//            return lowDist;
//    }
//}