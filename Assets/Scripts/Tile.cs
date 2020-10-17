using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private Sprite playerSprite;
    [SerializeField] private Sprite aiSprite;

    private Image image;
    private Board board;

    public int owner = 0;

    public void InitializeTile(Board board)
    {
        image = GetComponent<Image>();
        this.board = board;
    }

    public void PlacePlayerMarker()
    {
        if (board.gameover)
            return;

        if (owner == 0)
        {
            owner = -1;
            image.sprite = playerSprite;
            board.turns++;
        }
        else
        {
            return;
        }

        if (board.Evaluate())
        {
            board.AiPlay();
        }
    }

    public void PlaceAiMarker()
    {
        if (owner == 0)
        {
            owner = 1;
            image.sprite = aiSprite;
        }
        board.Evaluate();
    }
}
