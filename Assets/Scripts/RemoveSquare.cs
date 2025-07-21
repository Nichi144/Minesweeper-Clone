using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveSquare : MonoBehaviour
{
    public SizeLoader sizeLoader;

    void Awake()
    {
        sizeLoader = GameObject.Find("SceneMana").GetComponent<SizeLoader>();
    }
    public void DeleteMine()
    {
        sizeLoader.Mines -= 1;
        Destroy(gameObject);
    }

    public void DeleteSafe()
    {
        sizeLoader.Safe -= 1;
        Destroy(gameObject);
    }

}
