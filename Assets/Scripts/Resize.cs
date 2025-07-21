using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Resize : MonoBehaviour
{
    public GameObject xLine;
    public GameObject yLine;
    public GameObject mine;
    public GameObject safe;
    public int NumOfXLines ;
    public int NumOfYLines;
    public Camera cameras;

    private List<GameObject> totallines = new List<GameObject>();
    private List<GameObject> totalboxes = new List<GameObject>();

    [SerializeField] private int Mines;
    [SerializeField] private int Safe;

    private TMP_Text temp;

    public SizeLoader sizeLoader;

    void Start()
    {
        sizeLoader = GameObject.Find("SceneMana").GetComponent<SizeLoader>();
        NumOfXLines = sizeLoader.NumOfXLines;
        NumOfYLines = sizeLoader.NumOfYLines;
        float shiftX = -2f;
        if (NumOfXLines % 2 != 0)
        {
            shiftX -= 0.5f;
        }
        float shiftY = 0.35f;
        if (NumOfYLines % 2 != 0)
        {
            shiftY -= 0.5f;
        }
        GenerateGrid(NumOfXLines, NumOfYLines, shiftX, shiftY);
    }

    public void Reseter()
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
        if (x == 30 && y == 16)
        {
            Mines = 99;
        }
        else if ((x > 10) && (y > 10))
        {
            Mines = 40;
        }
        Safe = x * y - Mines;
        sizeLoader.Mines = Mines;
        sizeLoader.actu = true;
        float multix = 1f;
        float multiy = 1f;

        if (x == 30 && y == 16)
        {
            multix = 0.46f;
            multiy = 0.50f;
            yStart -= 0.25f;
            xStart += 0.25f;
        }
        else if (x > 10)
        {
            multix = 0.65f;
            multiy = 0.60f;
            yStart -= 0.25f;
        }

        
        // Generate vertical lines
        yLine.transform.localScale = new Vector2(0.05f, multiy * y);
        List<GameObject> lines = new List<GameObject>();

        if (x % 2 == 0f) // If x is even
        {
            for (int i = 0; i <= x; i++)
            {
                Vector2 pos = new Vector2(multix * (i - (x / 2)) + xStart, yStart);
                lines.Add(GameObject.Instantiate(yLine, pos, Quaternion.identity, GameObject.FindGameObjectWithTag("Lines").transform));
                lines[i].transform.position = pos;
            }
        }
        else  // If x is odd
        {
            for (int i = 0; i <= x; i++)
            {
                Vector2 pos = new Vector2( multix * (i - (x / 2) - 0.5f ) + xStart, yStart);
                lines.Add(GameObject.Instantiate(yLine, pos, Quaternion.identity, GameObject.FindGameObjectWithTag("Lines").transform));
                lines[i].transform.position = pos;
            }
        }

        // Generate horizontal lines
        xLine.transform.localScale = new Vector2(multix * x, 0.05f);
        List<GameObject> lines1 = new List<GameObject>();
        if (y % 2 == 0f) // If y is even
        {
            for (int q = 0; q <= y; q++)
            {
                Vector2 pos1 = new Vector2(xStart, multiy * (q - (y / 2)) + yStart);
                lines1.Add(GameObject.Instantiate(xLine, pos1, Quaternion.identity, GameObject.FindGameObjectWithTag("Lines").transform));
                lines1[q].transform.position = pos1;
            }
        }
        else  // If y is odd
        {
            for (int e = 0; e <= y; e++)
            {
                Vector2 pos1 = new Vector2(xStart, multiy * (e - (y / 2) - 0.5f) + yStart);
                lines1.Add(GameObject.Instantiate(xLine, pos1, Quaternion.identity, GameObject.FindGameObjectWithTag("Lines").transform));
            }
        }
        totallines.AddRange(lines);
        totallines.AddRange(lines1);

        GenerateBlocks(x, y, xStart, yStart, multix, multiy);
    }

    public void GenerateBlocks(int x, int y, float xStart, float yStart, float multix, float multiy)
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
        // Blocks are generated in the follwing pattern: start from bottom left, 
        // go up that collumn until you can't, then move to the bottom block of the next collumn and repeat
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; ++j)
            {
                Vector3 bpos = new Vector3(xStart + multix * (k + i - (x / 2)), yStart + multiy * (z + j - (y / 2)));
                GameObject block = PickBlock();
                block.transform.localScale = new Vector2(multix, multiy);
                totalboxes.Add(GameObject.Instantiate(block, bpos, Quaternion.identity, GameObject.FindGameObjectWithTag("Covers").transform));
            }
        }
        DetectMines(x, y);
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

    public void DetectMines(int x, int y)
    {
        List<int> neighbors = new List<int>();
        
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; ++j) // In each tile
            {
                if (totalboxes[i * y + j].name == "MineTile(Clone)") // If it's a mine
                {
                    neighbors.Clear();
                    // Debug.Log("Mine - Collumn: " + i + " Row: " + (x - j)); // Print the mine location
                    if ((i * y + j) % y == y-1) // Top row number
                    {
                        neighbors = new List<int> { -y - 1, -y, -1, y - 1, y};
                    }
                    else if ((i * y + j) % y == 0) // Bottom row number
                    {
                        neighbors = new List<int> { -y, -y + 1, 1, y, y + 1};
                    }
                    else // Normal Tile
                    {
                        neighbors = new List<int>() { -y - 1, -y, -y + 1, -1, 1, y - 1, y, y + 1 };
                    }

                    for (int k = 0; k < neighbors.Count; k++) // Then check all neighbors
                    {

                        if (i * y + j + neighbors[k] >= 0 && i * y + j + neighbors[k] < totalboxes.Count) // If valid
                        {
                            if (totalboxes[i * y + j + neighbors[k]].name != "MineTile(Clone)") // And not a mine
                            {
                                temp = totalboxes[i * y + j + neighbors[k]].GetComponentInChildren<TMP_Text>();
                                int cur_text = int.Parse(temp.text) + 1;  // Add one mine score to this neighbor
                                temp.text = cur_text.ToString();
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }
        }

    }
}

