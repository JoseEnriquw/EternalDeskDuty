using Assets.Scripts.Commons.GameManager;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objects : MonoBehaviour
{
    [SerializeField] private List<GameObject> objects;
    int _scene = 0;

    void Start()
    {
        _scene = GameManager.GetGameManager().GetRestartCount();
        MakePaintingVisible();
    }

    private void MakePaintingVisible()
    {       
        foreach (var obj in objects)
        {
            obj.SetActive(false);
        }
        
        for (int i = 0; i < _scene && i < objects.Count; i++)
        {
            objects[i].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
