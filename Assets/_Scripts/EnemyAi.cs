using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class EnemyAi : MonoBehaviour
{
    private GameController _gameController;
    [SerializeField] private EnemyDataSO _enemyData;

    //[Header("Difficulty Settings")]
    //[SerializeField, Range(1, 9)] private int _lookaheadDepth = 2;
    //[SerializeField, Range(0, 1)] private float _mistackeChance = 0.3f;
    

    public void MakeMove()
    {
        if (_gameController.IsGameOver) return;

        string[] board = _gameController.GetBoardState();
        string aiSide = _gameController.GetAISide();
        string playerSide = _gameController.GetPlayerSide();

        List<int> freeCells = GetFreeCells(board);
        if(freeCells.Count == 0) return;

        int chosenCell;

        if(Random.value < _enemyData._mistackeChance)
        {
            chosenCell = freeCells[Random.Range(0, freeCells.Count)];
            Debug.Log("AI made a stupid move..Lucky you!");
        }
        else
        {
            chosenCell = PickBestCell(board, freeCells, aiSide, playerSide);
            Debug.Log("AI made a smart move!");
        }

        GridSpaceButton chosenSpace = _gameController._buttonList[chosenCell].GetComponentInParent<GridSpaceButton>();
        chosenSpace.ClaimSpace(aiSide);

        //int value = Random.Range(0, _gameController._buttonList.Length);
        //var space = _gameController._buttonList[value].GetComponentInParent<GridSpaceButton>();

        //if ((space._isFree))
        //{
        //    space.ClaimSpace(_gameController.GetAISide());
        //}
    }

    private int PickBestCell(string[] board, List<int> freeCells, string aiSide, string playerSide)
    {
        int bestScore = int.MinValue;
        List<int> bestCells = new List<int>();

        foreach(int cell in freeCells)
        {
            board[cell] = aiSide;
            int score = Minimax(board, playerSide, _enemyData._lookaheadDepth - 1, aiSide, playerSide);
            board[cell] = "";

            if(score > bestScore)
            {
                bestScore = score;
                bestCells.Clear();
                bestCells.Add(cell);
            }
            else if(score == bestScore)
            {
                bestCells.Add(cell);
            }
        }

        return bestCells[Random.Range(0, bestCells.Count)];
    }

    private int Minimax(string[] board, string sideToMove, int depthLeft, string aiSide, string playerSide)
    {
        if (HasWon(board, aiSide)) return 10 + depthLeft;
        if(HasWon(board, playerSide)) return -10 - depthLeft;

        List<int> freeCells = GetFreeCells(board);
        if (freeCells.Count == 0) return 0;
        if (depthLeft <= 0) return 0;

        if(sideToMove == aiSide)
        {
            int best = int.MinValue;
            foreach(int cell in freeCells)
            {
                board[cell] = aiSide;
                best = Mathf.Max(best, Minimax(board, playerSide, depthLeft - 1, aiSide, playerSide));
                board[cell] = "";
            }
            return best;
        }
        else
        {
            int best = int.MaxValue;
            foreach(int cell in freeCells)
            {
                board[cell] = playerSide;
                best = Mathf.Min(best, Minimax(board, aiSide, depthLeft - 1, aiSide, playerSide));
                board[cell] = "";
            }
            return best;
        }
    }

    private static List<int> GetFreeCells(string[] board)
    {
        List<int> free = new List<int>();
        for(int i = 0; i < board.Length; i++)
        {
            if (board[i] == "") free.Add(i);
        }
        return free;
    }

    static bool HasWon(string[] board, string side)
    {
        foreach (int[] line in GameController.WinPatterns)
        {
            if (board[line[0]] == side && board[line[1]] == side && board[line[2]] == side)
                return true;
        }
        return false;
    }

    // Method to set the reference to the GameController
    public void SetGameControllerRef(GameController controller)
    {
        _gameController = controller;
    }
}

