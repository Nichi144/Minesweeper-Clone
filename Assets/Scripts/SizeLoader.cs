using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System.Collections;

public class SizeLoader : MonoBehaviour
{
    public int NumOfXLines, NumOfYLines = 8;
    public float shiftX, shiftY = 0;

    public int Mines = 1;
    private int totalMines = 1;
    public int Safe = 3;
    public bool actu = false;
    private bool actus = false;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject[] check = GameObject.FindGameObjectsWithTag("Numbers");
        if (check.Length > 1)
        {
            Destroy(check[0]);
        }
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Minesweeper" && actu)
        {
            
            totalMines = Mines;
            Safe = NumOfXLines * NumOfYLines - Mines;
            actu = false;
            actus = true;
        }
        if (actus)
        {
            if (Mines + 1 == totalMines)
            {
                StartCoroutine(Losers());
            }
            if (Safe == 0 && Mines == totalMines)
            {
                StartCoroutine(Winners());
            }
        }

        

    }
    // Update is called once per frame
    public void Easy(string name)
    {
        NumOfXLines = 8;
        NumOfYLines = 8;
        SceneManager.LoadScene(name);
    }
    public void Normal(string name)
    {
        NumOfXLines = 16;
        NumOfYLines = 14;
        SceneManager.LoadScene(name);
    }

    public void Hard(string name)
    {
        NumOfXLines = 30;
        NumOfYLines = 16;
        SceneManager.LoadScene(name);
    }

    private IEnumerator Losers()
    {
        actus = false;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Lose");
    }
    private IEnumerator Winners()
    {
        actus = false;
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Win");
    }
    
}
