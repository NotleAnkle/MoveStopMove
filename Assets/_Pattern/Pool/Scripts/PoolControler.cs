using _Framework.Pool.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class PoolControler : MonoBehaviour
{
    [Header("---- POOL CONTROLER TO INIT POOL ----")]
    [Header("Put object pool to list Pool or Resources/Pool")]
    [Header("Preload: Init Poll")]
    [Header("Spawn: Take object from pool")]
    [Header("Despawn: return object to pool")]
    [Header("Collect: return objects type to pool")]
    [Header("CollectAll: return all objects to pool")]

    [Space]
    [Header("Pool")]
    public List<PoolAmount> Pool;

    [Header("Particle")]
    public ParticleAmount[] Particle;


    public void Awake()
    {
        for (int i = 0; i < Pool.Count; i++)
        {
            SimplePool.Preload(Pool[i].prefab, Pool[i].amount, Pool[i].root, Pool[i].collect);
        }

        for (int i = 0; i < Particle.Length; i++)
        {
            ParticlePool.Preload(Particle[i].prefab, Particle[i].amount, Particle[i].root);
            ParticlePool.Shortcut(Particle[i].particleType, Particle[i].prefab);
        }
    }
}

#if UNITY_EDITOR

[CustomEditor(typeof(PoolControler))]
public class PoolControlerEditor : Editor
{
    PoolControler pool;

    private void OnEnable()
    {
        pool = (PoolControler)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Create Quick Root"))
        {
            for (int i = 0; i < pool.Pool.Count; i++)
            {
                if (pool.Pool[i].root == null)
                {
                    Transform tf = new GameObject(pool.Pool[i].prefab.poolType.ToString()).transform;
                    tf.parent = pool.transform;
                    pool.Pool[i].root = tf; 
                }
            }
            
            for (int i = 0; i < pool.Particle.Length; i++)
            {
                if (pool.Particle[i].root == null)
                {
                    Transform tf = new GameObject(pool.Particle[i].particleType.ToString()).transform;
                    tf.parent = pool.transform;
                    pool.Particle[i].root = tf; 
                }
            }
        }

        if (GUILayout.Button("Get Prefab Resource"))
        {
            GameUnit[] resources = Resources.LoadAll<GameUnit>("Pool");

            for (int i = 0; i < resources.Length; i++)
            {
                bool isDuplicate = false;
                for (int j = 0; j < pool.Pool.Count; j++)
                {
                    if (resources[i].poolType == pool.Pool[j].prefab.poolType)
                    {
                        isDuplicate = true;
                        break;
                    }
                }

                if (!isDuplicate)
                {
                    Transform root = new GameObject(resources[i].name).transform;

                    PoolAmount newPool = new PoolAmount(root, resources[i], SimplePool.DEFAULT_POOL_SIZE, true);

                    pool.Pool.Add(newPool);
                }
            }
        }
    }
}

#endif

[System.Serializable]
public class PoolAmount
{
    [Header("-- Pool Amount --")]
    public Transform root;
    public GameUnit prefab;
    public int amount;
    public bool collect;

    public PoolAmount (Transform root, GameUnit prefab, int amount, bool collect)
    {
        this.root = root;
        this.prefab = prefab;
        this.amount = amount;
        this.collect = collect;
    }
}


[System.Serializable]
public class ParticleAmount
{
    public Transform root;
    public ParticleType particleType;
    public ParticleSystem prefab;
    public int amount;
}


public enum ParticleType
{
    BeamUpBlue = 0,
    BloodExplosion = 1,
}

public enum PoolType
{
    None = 0,

    Bot = 1,

    W_Hammer = 10,
    W_Kinfe = 11,
    W_Boomerang = 12,

    BL_Hammer = 30,
    BL_Knife = 31,
    BL_Boomerang = 32,

    SKIN_Normal = 50,
    SKIN_Devil = 51,
    SKIN_Angle = 52,
    SKIN_Witch = 53,
    SKIN_Deadpool = 54,
    SKIN_Thor = 55,

    H_Arrow = 70,
    H_Cap = 71,
    H_Cowboy = 72,
    H_Crown = 73,
    H_Ear = 74,
    H_Flower = 75,
    H_Hair = 76,
    H_Hat = 77,
    H_Hat_Cap = 78,
    H_Hat_Yellow = 79,
    H_Headphone = 80,
    H_Horn = 81,
    H_Mustache = 82,

    ACC_Shield_1 = 90,
    ACC_Shield_2 = 91,

    TargetIndicator = 100,
}


