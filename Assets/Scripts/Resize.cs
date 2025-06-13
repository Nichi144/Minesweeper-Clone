using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Resize : MonoBehaviour
{
    public GameObject xLine;
    public GameObject yLine;
    public GameObject mine;
    public GameObject safe;
    public int NumOfXLines;
    public int NumOfYLines;
    public Camera cameras;

    private List<GameObject> totallines = new List<GameObject>();
    private List<GameObject> totalboxes = new List<GameObject>();
    public int Mines;
    public int Safe;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            float shiftX = 0f;
            if (NumOfXLines % 2 != 0)
            {
                shiftX -= 0.5f;
            }
            float shiftY = 0f;
            if (NumOfYLines % 2 != 0)
            {
                shiftY -= 0.5f;
            }
            GenerateGrid(NumOfXLines, NumOfYLines, shiftX, shiftY);
        }
    }

    public void Deleter()
    {
        // Delete any previous grid
        for (int pso = totallines.Count; pso > 0; --pso)
        {
            Destroy(totallines[pso - 1]);
        }
        totallines.Clear();

        // Delete any previous grid
        for (int po = totalboxes.Count; po > 0; --po)
        {
            Destroy(totalboxes[po - 1]);
        }
        totalboxes.Clear();
    }

    public void GenerateGrid(int x, int y, float xStart, float yStart)
    {

        Deleter();

        Mines = 10;
        if (y == 36) {
            
        } else if ((x > 10) && (y > 10))
        {
            Mines = 40;
        } 
        Safe = x*y - Mines;

        // Generate vertical lines
        yLine.transform.localScale = new Vector2(0.05f, 1f * y);
        List<GameObject> lines = new List<GameObject>();

        if (x % 2 == 0f) // If x is even
        {
            for (int i = 0; i <= x; i++)
            {
                Vector2 pos = new Vector2(i - (x / 2) + xStart, yStart);
                lines.Add(GameObject.Instantiate(yLine, pos, Quaternion.identity, GameObject.FindGameObjectWithTag("Lines").transform));
                lines[i].transform.position = pos;
            }
        }
        else  // If x is odd
        {
            for (int i = 0; i <= x; i++)
            {
                Vector2 pos = new Vector2(i - (x / 2) - 0.5f + xStart, yStart);
                lines.Add(GameObject.Instantiate(yLine, pos, Quaternion.identity, GameObject.FindGameObjectWithTag("Lines").transform));
                lines[i].transform.position = pos;
            }
        }




        // Generate horizontal lines
        xLine.transform.localScale = new Vector2(1f * x, 0.05f);
        List<GameObject> lines1 = new List<GameObject>();
        if (y % 2 == 0f) // If y is even
        {
            for (int q = 0; q <= y; q++)
            {
                Vector2 pos1 = new Vector2(xStart, q - (y / 2) + yStart);
                lines1.Add(GameObject.Instantiate(xLine, pos1, Quaternion.identity, GameObject.FindGameObjectWithTag("Lines").transform));
                lines1[q].transform.position = pos1;
            }
        }
        else  // If y is odd
        {
            for (int e = 0; e <= y; e++)
            {
                Vector2 pos1 = new Vector2(xStart, e - (y / 2) - 0.5f + yStart);
                lines1.Add(GameObject.Instantiate(xLine, pos1, Quaternion.identity, GameObject.FindGameObjectWithTag("Lines").transform));
            }
        }
        totallines.AddRange(lines);
        totallines.AddRange(lines1);


        GenerateBlocks(x, y, xStart, yStart);
    }

    public void GenerateBlocks(int x, int y, float xStart, float yStart)
    {

        float z = 0;
        if (y % 2 != 1)
        {
            z = 0.5f;
        }

        float k = 0;
        if (x % 2 != 1)
        {
            k = 0.5f;
        }

        // Generate Boxes
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; ++j)
            {

                Vector3 bpos = new Vector3(xStart + k + i - (x / 2), yStart + z + j - (y / 2));
                totalboxes.Add(GameObject.Instantiate(PickBlock(), bpos, Quaternion.identity, GameObject.FindGameObjectWithTag("Covers").transform));
            }
        }
    }

    public GameObject PickBlock()
    {
        int m = Random.Range(1, Mines + Safe);
        if (((m < Mines + 1) && Mines > 0) || (Safe == 0))
        {
            Mines -= 1;
            return mine;
        }
        Safe--;
        return safe;
    }
}
