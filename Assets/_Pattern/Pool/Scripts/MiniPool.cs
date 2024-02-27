﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPool<T> where T : Component
{
    private List<T> listActives = new List<T>();
    int index = 0;

    T prefab;
    Transform parent;

    public void OnInit(T prefab, int amount, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.index = 0;
        
        for (int i = 0; i < amount; i++)
        {
            T t = GameObject.Instantiate(prefab, parent);
            listActives.Add(t);
            //cach deu cac doi tuong UI do k tim thay cach a linh dung
            t.transform.position += new Vector3(311.667f * (i / 2), i % 2 == 1 ? -402.689f / 2 : 0f, 0f);
            Despawn(t);
        }
    }

    public T Spawn(Vector3 pos, Quaternion rot)
    {
        T go = Spawn();

        go.transform.SetPositionAndRotation(pos, rot);

        return go;
    }
    
    public T Spawn()
    {
        T go = index < listActives.Count ? listActives[index] : null; 

        if (go == null)
        {
            go = GameObject.Instantiate(prefab, parent);
            listActives.Add(go);
        }

        index++;
        go.gameObject.SetActive(true);

        return go;
    }

    public void Despawn(T obj)
    {
        if (obj.gameObject.activeSelf)
        {
            obj.gameObject.SetActive(false);
        }
    }

    public void Collect()
    {
        for (int i = 0; i < index; i++)
        {
            Despawn(listActives[i]);
        }
        index = 0;
    }

    public void Release()
    {
        Collect();
        for (int i = 0; i < listActives.Count; i++)
        {
            GameObject.Destroy(listActives[i].gameObject);
        }
        listActives.Clear();
        index = 0;
    }

}
