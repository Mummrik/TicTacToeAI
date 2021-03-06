﻿using System;
using UnityEngine;

public class AI : MonoBehaviour
{
    public bool isMovesLeft(Tile[,] board)
    {
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (board[x, y].owner == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public Vector2Int BestMove(Tile[,] board)
    {
        int bestScore = int.MinValue;
        Vector2Int nextMove = Vector2Int.zero;
        for (int x = 0; x < 3; x++)
        {
            for (int y = 0; y < 3; y++)
            {
                if (board[x, y].owner == 0)
                {
                    board[x, y].owner = 1;
                    int score = Minimax(board, 0, false, int.MinValue, int.MaxValue);
                    board[x, y].owner = 0;

                    if (score > bestScore)
                    {
                        bestScore = score;
                        nextMove = new Vector2Int(x, y);
                    }
                }
            }
        }
        return nextMove;
    }

    public enum WinEnum
    {
        Lose = -1,
        None,
        Tie = 0,
        Win
    }
    public WinEnum WinCheck(Tile[,] board)
    {
        // Horizontal
        for (int x = 0; x < 3; x++)
        {
            if (board[x, 0].owner == board[x, 1].owner && board[x, 1].owner == board[x, 2].owner)
            {
                if (board[x, 0].owner == -1 || board[x, 0].owner == 1)
                {
                    return (WinEnum)board[x, 0].owner;
                }
            }
        }

        // Vertical
        for (int y = 0; y < 3; y++)
        {
            if (board[0, y].owner == board[1, y].owner && board[1, y].owner == board[2, y].owner)
            {
                if (board[0, y].owner == -1 || board[0, y].owner == 1)
                {
                    return (WinEnum)board[0, y].owner;
                }
            }
        }

        // Diagonal 1
        if (board[0, 0].owner == board[1, 1].owner && board[1, 1].owner == board[2, 2].owner)
        {
            if (board[0, 0].owner == -1 || board[0, 0].owner == 1)
            {
                return (WinEnum)board[0, 0].owner;
            }
        }

        // Diagonal 2
        if (board[2, 0].owner == board[1, 1].owner && board[1, 1].owner == board[0, 2].owner)
        {
            if (board[2, 0].owner == -1 || board[2, 0].owner == 1)
            {
                return (WinEnum)board[2, 0].owner;
            }
        }

        if (isMovesLeft(board))
        {
            return WinEnum.Tie;
        }

        return WinEnum.None;
    }

    public int Minimax(Tile[,] board, int depth, bool isMaximizing, int alpha, int beta)
    {
        WinEnum condition = WinCheck(board);

        if (condition != WinEnum.None)
        {
            switch (condition)
            {
                case WinEnum.Lose:
                    return -10 + depth;
                case WinEnum.Tie:
                    return 0;
                case WinEnum.Win:
                    return 100 - depth;
            }
        }

        if (isMaximizing)
        {
            int bestScore = int.MinValue;

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (board[x, y].owner == 0)
                    {
                        board[x, y].owner = 1;
                        int score = Minimax(board, depth + 1, !isMaximizing, alpha, beta);
                        bestScore = Math.Max(bestScore, score);
                        alpha = Math.Max(alpha, bestScore);
                        board[x, y].owner = 0;

                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
            }
            return bestScore;
        }
        else
        {
            int bestScore = int.MaxValue;

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (board[x, y].owner == 0)
                    {
                        board[x, y].owner = -1;
                        int score = Minimax(board, depth + 1, !isMaximizing, alpha, beta);
                        bestScore = Math.Min(bestScore, score);
                        beta = Math.Min(beta, bestScore);
                        board[x, y].owner = 0;
                        if (beta <= alpha)
                        {
                            break;
                        }
                    }
                }
            }
            return bestScore;
        }

    }
}
