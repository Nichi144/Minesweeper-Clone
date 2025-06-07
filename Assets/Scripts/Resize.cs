using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resize : MonoBehaviour
{
    public GameObject xLine;
    public GameObject yLine;
    public GameObject mine;
    public GameObject safe;
    public int NumOfXLines;
    public int NumOfYLines;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            float shiftX = 0f;
            if (NumOfXLines % 2 != 0)
            {
                shiftX -= 0f;
            }
            float shiftY = 0f;
            if (NumOfYLines % 2 != 0)
            {
                shiftY -= 0f;
            }
            GenerateGrid(NumOfXLines, NumOfYLines, shiftX, shiftY);
        }
    }



    public void GenerateGrid(int x, int y, float xStart, float yStart)
    {
        // Generate vertical lines
        yLine.transform.localScale = new Vector2(0.05f, 1f * y);
        List<GameObject> lines = new List<GameObject>();
        for (int i = 0; i <= x; i++)
        {
            Vector2 pos = new Vector2(xStart - (x / 2) + i  - 0.5f, yStart + 0.5f);
            lines.Add(GameObject.Instantiate(yLine));
            lines[i].transform.position = pos;
        }

        // Generate horizontal lines
        xLine.transform.localScale = new Vector2(1f * x, 0.05f);
        List<GameObject> lines1 = new List<GameObject>();
        for (int q = 0; q <= y; q++)
        {
            Vector2 pos1 = new Vector2(xStart- 0.5f, yStart - (y / 2) + q);
            lines1.Add(GameObject.Instantiate(xLine));
            lines1[q].transform.position = pos1;
        }
        // GenerateBlocks(x, y, xStart, yStart);
    }

    public void GenerateBlocks(int x, int y, float xStart, float yStart)
    {
        
        List<GameObject> lines = new List<GameObject>();
        for (int i = 0; i < x; i++)
        {
            for (int q = 0; q < y; q++)
            {
                Vector2 pos1 = new Vector2(xStart - (x/2) + i + 0.5f, yStart - (y / 2) + q + 0.5f);
                lines.Add(GameObject.Instantiate(safe));
                lines[(i*y)+q -1].transform.position = pos1;
            }
           
        }

        
        

    }
}
