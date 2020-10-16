using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField] private GameObject canvas;

    private GameObject tilePrefab;
    private Vector2 center;

    private AI ai;
    private Text winText;

    public int winner = 0;
    public Tile[,] grid;
    public int turns = 0;
    public bool gameover = false;

    private void Awake()
    {
        ai = GetComponent<AI>();
        tilePrefab = Resources.Load("Prefabs/Tile") as GameObject;
        winText = GameObject.Find("Canvas/WinText").GetComponent<Text>();
        winText.gameObject.SetActive(false);

        if (canvas == null)
        {
            canvas = GameObject.Find("Canvas");
        }

        Rect canvasRect = canvas.GetComponent<RectTransform>().rect;
        center = new Vector2(canvasRect.width * 0.5f, canvasRect.height * 0.5f);
    }

    void Start()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        grid = new Tile[3, 3];

        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                GameObject tileObj = Instantiate(tilePrefab);
                tileObj.transform.SetParent(canvas.transform);

                Rect buttonRect = tileObj.GetComponent<Image>().rectTransform.rect;
                tileObj.transform.position = center + new Vector2((x - 1) * buttonRect.width, (y - 1) * buttonRect.height);

                grid[x, y] = tileObj.GetComponent<Tile>();
                grid[x, y].InitializeTile(this);
            }
        }
    }

    public bool Evaluate()
    {
        if (WinCheck())
        {
            if (winner == 1)
            {
                winText.text = "CPU Win!";
            }
            winText.gameObject.SetActive(true);
            gameover = true;
            return false;
        }
        else if (turns >= 5)
        {
            winText.text = "It's a Draw.";
            winText.gameObject.SetActive(true);
            gameover = true;
            return false;
        }

        return true;
    }

    private Tile RandomAIDecision()
    {
        int marker = -2;
        Tile tile;
        do
        {
            int x = Random.Range(0, 3);
            int y = Random.Range(0, 3);
            marker = grid[x, y].owner;
            tile = grid[x, y];

        } while (marker != 0);

        return tile;
    }

    public void AiPlay()
    {
        Vector2Int move = ai.BestMove(grid);
        grid[move.x, move.y].PlaceAiMarker();
        //RandomAIDecision().PlaceAiMarker();
    }

    private bool WinCheck()
    {
        int checkWinner;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                //Horizontal win check
                if (x == 0)
                {
                    checkWinner = grid[x, y].owner;
                    if (checkWinner != 0)
                    {
                        if (grid[x, y].owner == grid[x + 1, y].owner && grid[x, y].owner == grid[x + 2, y].owner)
                        {
                            winner = checkWinner;
                            return true;
                        }
                    }
                }

                // Vertical win check
                if (y == 0)
                {
                    checkWinner = grid[x, y].owner;
                    if (checkWinner != 0)
                    {
                        if (grid[x, y].owner == grid[x, y + 1].owner && grid[x, y].owner == grid[x, y + 2].owner)
                        {
                            winner = checkWinner;
                            return true;
                        }
                    }
                }

                // Diagonal win check
                if (x == 0 && y == 0)
                {
                    checkWinner = grid[x, y].owner;
                    if (checkWinner != 0)
                    {
                        if (grid[x, y].owner == grid[1, 1].owner && grid[x, y].owner == grid[2, 2].owner)
                        {
                            winner = checkWinner;
                            return true;
                        }
                    }
                }
                else if (x == 0 && y == 2)
                {
                    checkWinner = grid[x, y].owner;
                    if (checkWinner != 0)
                    {
                        if (grid[x, y].owner == grid[1, 1].owner && grid[x, y].owner == grid[2, 0].owner)
                        {
                            winner = checkWinner;
                            return true;
                        }
                    }
                }

            }
        }

        return false;
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
