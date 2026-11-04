using System.Collections;
using UnityEngine.Rendering;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Player
{
    public Image _panel;
    public TextMeshProUGUI _text;
    public Button _chooseSideButton;
}

[System.Serializable]
public class PlayerColor
{
    public Color _panelColor;
    public Color _textColor;
}

public enum BoardResult
{
    Unfinished,
    PlayerWon,
    EnemyWon,
    Draw
}

public class GameController : MonoBehaviour
{

    [SerializeField] private TicTacToeBoard[] _boards; //boards, 9 should be added
    [SerializeField] private EnemyAi _enemyAi;
    [SerializeField] private BoardsView _boardsView;
    [SerializeField] private SeamLineOverlay _seamLineOverlay;
    [SerializeField] private float _aiThinkTime = 0.6f;
    [SerializeField] private float _castBeat = 0.5f;

    [SerializeField] private float _finalMoveBeat = 0.6f;
    [SerializeField] private float _seamRevealDelay = 0.5f;
    [SerializeField] private float _overviewHold = 2f;

    private TextMeshProUGUI[] _allCells = new TextMeshProUGUI[81];
    private BoardResult[] _results = new BoardResult[9];
    private List<Block> _blocks = new List<Block>();

    public TextMeshProUGUI[] _buttonList;
    public GameObject _resultPanel;
    public TextMeshProUGUI _resultText;
    public GameObject _startInfo;
    public EnemyHealth _enemyHealth;
    public PlayerHealth _playerHealth;

    private string _playerSide;
    private string _aiSide;
    private bool _gameOver;
    private bool _lastMoverWasPlayer;
    private int _moveCount;
    private int _activeIndex;

    public bool _playerMove;
    public bool IsGameOver => _gameOver;

    public Player _playerX;
    public Player _playerO;
    public PlayerColor _activePlayerColor;
    public PlayerColor _inactivePlayerColor;

    public event System.Action<int, BoardResult> OnBoardFinished;
    public event System.Action<string, int[]> OnSeamLineMade;
    public event System.Action OnEncounterOver;
    public event System.Action<Block> OnPlayerBlocked;

    public static readonly int[][] WinPatterns =
    {
        new int[] { 0, 1, 2 }, // Top row
        new int[] { 3, 4, 5 }, // Middle row
        new int[] { 6, 7, 8 }, // Bottom row
        new int[] { 0, 3, 6 }, // Left column
        new int[] { 1, 4, 7 }, // Middle column
        new int[] { 2, 5, 8 }, // Right column
        new int[] { 0, 4, 8 }, // Diagonal from top-left to bottom-right
        new int[] { 2, 4, 6 }  // Diagonal from top-right to bottom-left
    };


    private void Awake()
    {
        _resultPanel.SetActive(false);
        SetGameControllerRef();
        SetPlayerColorsInactive();
        _boardsView.SnapToOverview();
        SetupBoards();
        LockAllBoards();
        _startInfo.SetActive(true);
        _playerMove = true;
    }

    private IEnumerator AiTurnCoroutine()
    {
        yield return new WaitForSeconds(_aiThinkTime);

        if (_gameOver || _playerMove) yield break;

        _enemyAi.MakeMove();
    }

    IEnumerator CastPowerCoroutine()
    {
        if(_enemyAi.TryCastpower())
        {
            yield return new WaitForSeconds(_castBeat);
        }
    }

    void SetGameControllerRef()
    {
        _enemyAi.SetGameControllerRef(this);

        for (int i = 0; i < _buttonList.Length; i++)
        {
            _buttonList[i].GetComponentInParent<GridSpaceButton>().SetGameControllerRef(this);
        }
    }

    public void SetStartingSide(string startingSide)
    {
        _playerSide = startingSide;

        _aiSide = (_playerSide == "X") ? "O" : "X"; // For when I forget how this works : in bracets is the if and then if true the first one will run else the second one will

        StartGame();
    }

    void StartGame()
    {
        _gameOver = false;
        _activeIndex = 0;
        _playerMove = true;

        _playerHealth.ResetDefense();
        _enemyHealth.ResetDefense();
        SetPlayerSideButtons(false);
        _startInfo.SetActive(false);
        StartCoroutine(BeginBoardCoroutine());
    }

    IEnumerator BeginBoardCoroutine()
    {
        yield return _boardsView.ZoomToBoard(_activeIndex);
        yield return CastPowerCoroutine();

        HightlightCurrentTurn();
        _boards[_activeIndex].Unlock();
    }

    public string GetPlayerSide()
    {
        return _playerSide;
    }

    public string GetAISide()
    {
        return _aiSide;
    }

    public void EndTurn()
    {
        _lastMoverWasPlayer = _playerMove;

        if(_playerMove) TickBlocks();

        TicTacToeBoard board = _boards[_activeIndex];

        bool playerWon = board.HasWon(_playerSide);
        bool aiWon = board.HasWon(_aiSide);

        if(playerWon || aiWon || board.IsFull)
        {
            FinishBoard(playerWon, aiWon);
        }
        else
        {
            ChangeSides();
        }

        #region Old Win Check Code (Commented Out)

        //#region Horitzontal Wins

        //if (_buttonList[0].text == _playerSide && _buttonList[1].text == _playerSide && _buttonList[2].text == _playerSide)
        //{
        //    GameOver(_playerSide);
        //}
        //else if (_buttonList[3].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[5].text == _playerSide)
        //{
        //    GameOver(_playerSide);
        //}
        //else if (_buttonList[6].text == _playerSide && _buttonList[7].text == _playerSide && _buttonList[8].text == _playerSide)
        //{
        //    GameOver(_playerSide);
        //}

        //#endregion

        //#region Vertical Wins

        //else if (_buttonList[0].text == _playerSide && _buttonList[3].text == _playerSide && _buttonList[6].text == _playerSide)
        //{
        //    GameOver(_playerSide);
        //}
        //else if (_buttonList[1].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[7].text == _playerSide)
        //{
        //    GameOver(_playerSide);
        //}
        //else if (_buttonList[2].text == _playerSide && _buttonList[5].text == _playerSide && _buttonList[8].text == _playerSide)
        //{
        //    GameOver(_playerSide);
        //}

        //#endregion

        //#region Diagonal Wins

        //else if (_buttonList[0].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[8].text == _playerSide)
        //{
        //    GameOver(_playerSide);
        //}
        //else if (_buttonList[2].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[6].text == _playerSide)
        //{
        //    GameOver(_playerSide);
        //}

        //#endregion

        //#region Horitzontal Wins Computer

        //else if (_buttonList[0].text == _computerSide && _buttonList[1].text == _computerSide && _buttonList[2].text == _computerSide)
        //{
        //    GameOver(_computerSide);
        //}
        //else if (_buttonList[3].text == _computerSide && _buttonList[4].text == _computerSide && _buttonList[5].text == _computerSide)
        //{
        //    GameOver(_computerSide);
        //}
        //else if (_buttonList[6].text == _computerSide && _buttonList[7].text == _computerSide && _buttonList[8].text == _computerSide)
        //{
        //    GameOver(_computerSide);
        //}

        //#endregion

        //#region Vertical Wins Computer

        //else if (_buttonList[0].text == _computerSide && _buttonList[3].text == _computerSide && _buttonList[6].text == _computerSide)
        //{
        //    GameOver(_computerSide);
        //}
        //else if (_buttonList[1].text == _computerSide && _buttonList[4].text == _computerSide && _buttonList[7].text == _computerSide)
        //{
        //    GameOver(_computerSide);
        //}
        //else if (_buttonList[2].text == _computerSide && _buttonList[5].text == _computerSide && _buttonList[8].text == _computerSide)
        //{
        //    GameOver(_computerSide);
        //}

        //#endregion

        //#region Diagonal Wins Computer

        //else if (_buttonList[0].text == _computerSide && _buttonList[4].text == _computerSide && _buttonList[8].text == _computerSide)
        //{
        //    GameOver(_computerSide);
        //}
        //else if (_buttonList[2].text == _computerSide && _buttonList[4].text == _computerSide && _buttonList[6].text == _computerSide)
        //{
        //    GameOver(_computerSide);
        //}

        //#endregion

        #endregion

    }

    void FinishBoard(bool playerWon, bool aiWon)
    {
        BoardResult result = playerWon ? BoardResult.PlayerWon 
            : aiWon ? BoardResult.EnemyWon 
            : BoardResult.Draw;

        int finishedIndex = _activeIndex;

        _results[finishedIndex] = result;
        _boards[finishedIndex].Lock();
        ClearBlocks();
        Debug.Log($"Board {finishedIndex + 1} finished: {result}");
        OnBoardFinished?.Invoke(finishedIndex, result);

        List<int[]> seamLines = ScoreSeamLines(finishedIndex);
        StartCoroutine(BoardTransitionCoroutine(finishedIndex, seamLines));
    }

    List<int[]> ScoreSeamLines(int finishedBoard)
    {
        List<int[]> found = new List<int[]>();
        foreach (int[] line in BoardLines.SeamLinesByLastBoard[finishedBoard])
        {
            string owner = _allCells[line[0]].text;
            if ((owner == "")) continue;

            if (_allCells[line[1]].text == owner && _allCells[line[2]].text == owner)
            {
                found.Add(line);
            }
        }

        return found;
    }

    IEnumerator BoardTransitionCoroutine(int finishedIndex, List<int[]> seamLines)
    {
        SetPlayerColorsInactive();

        yield return new WaitForSeconds(_finalMoveBeat);
        yield return _boardsView.ZoomToOverview();

        foreach (int[] line in seamLines)
        {
            string owner = _allCells[line[0]].text;
            yield return _seamLineOverlay.Reveal(owner, new[] { _allCells[line[0]], _allCells[line[1]], _allCells[line[2]] });
            Debug.Log($"{owner} made a seam line: {line[0]}, {line[1]}, {line[2]}");
            OnSeamLineMade?.Invoke(owner, line);
            yield return new WaitForSeconds(_seamRevealDelay);
        }

        yield return new WaitForSeconds(_overviewHold);

        if(finishedIndex >= _boards.Length - 1)
        {
            EncounterOver();
            yield break;
        }

        _activeIndex = finishedIndex + 1;
        yield return _boardsView.ZoomToBoard(_activeIndex);
        yield return CastPowerCoroutine();
        StartNextBoard();
    }

    void StartNextBoard()
    {
        _playerMove = !_lastMoverWasPlayer;
        HightlightCurrentTurn();

        if (!_playerMove)
        {
            StartCoroutine(AiTurnCoroutine());
        }

        _boards[_activeIndex].Unlock();
    }

    public string[] GetBoardState()
    {
        return _boards[_activeIndex].GetState();
    }

    public void ClaimCell(int index, string side)
    {
        _boards[_activeIndex].ClaimCell(index, side);
    }

    void SetPlayerColors(Player newPlayer, Player oldPlayer)
    {
        newPlayer._panel.color = _activePlayerColor._panelColor;
        newPlayer._text.color = _activePlayerColor._textColor;
        oldPlayer._panel.color = _inactivePlayerColor._panelColor;
        oldPlayer._text.color = _inactivePlayerColor._textColor;
    }

    void GameOver(string winningPlayer)
    {
        _gameOver = true;
        SetBoardInteractable(false);

        if(winningPlayer == "Draw")
        {
            SetResultText("It's a Draw!");
            SetPlayerColorsInactive();
        }
        else
        {
            SetResultText(winningPlayer + " Wins!");
        }

    }

    bool HasWon(string side)
    {
        foreach (var line in WinPatterns)
        {
            if (_buttonList[line[0]].text == side && 
                _buttonList[line[1]].text == side && 
                _buttonList[line[2]].text == side)
            {
                return true;
            }
        }
        return false;
    }

    void ChangeSides()
    {
        _playerMove = !_playerMove;
        HightlightCurrentTurn();

        if (_playerMove)
        {
            if (!PlayerHasPlayableCell()) _blocks.RemoveAll(b => b._mode == BlockMode.Lock);
            RefreshBoard();
        }

        if (!_playerMove)
        {
            StartCoroutine(AiTurnCoroutine());
        }
    }

    void EncounterOver()
    {
        _gameOver = true;
        LockAllBoards();
        SetPlayerColorsInactive();

        int player = 0, enemy = 0, draws = 0;
        foreach(BoardResult r in _results)
        {
            if (r == BoardResult.PlayerWon) player++;
            else if (r == BoardResult.EnemyWon) enemy++;
            else if(r == BoardResult.Draw) draws++;
        }

        SetResultText($"Encounter Over!\n You: {player} Enemy: {enemy} Draws: {draws}");
        OnEncounterOver?.Invoke();
    }

    public void SetResultText(string value)
    {
        _resultPanel.SetActive(true);
        _resultText.text = value;
    }

    public void RestartGame()
    {
        StopAllCoroutines();
        _boardsView.SnapToOverview();
        _seamLineOverlay.Clear();

        _gameOver = false;
        _activeIndex = 0;
        _lastMoverWasPlayer = false;

        for (int i = 0; i < _results.Length; i++)
        {
            _results[i] = BoardResult.Unfinished;
        }

        for(int i = 0; i < _boards.Length; i++)
        {
            _boards[i].Clear();
            _boards[i].Lock();
        }

        ClearBlocks();

        _resultPanel.SetActive(false);
        _startInfo.SetActive(true);

        SetPlayerSideButtons(true);
        SetPlayerColorsInactive();

        _playerMove = true;
    }

    void SetupBoards()
    {
        for(int b = 0; b < _boards.Length; b++)
        {
            _boards[b].Initialize(this);

            for(int c = 0; c < _boards[b].CellCount; c++)
            {
                _allCells[BoardLines.ToGlobal(b, c)] = _boards[b].GetCellText(c);
            }
        }

        ValidateBoards();

    }

    void ValidateBoards()
    {
        if(_boards.Length != 9)
        {
            Debug.LogError($"GameController needs exactly 9 board, found only {_boards.Length}");
            return;
        }

        var seen = new HashSet<TextMeshProUGUI>();
        for(int g = 0; g < _allCells.Length; g++)
        {
            if (_allCells[g] == null) Debug.LogError($"_allCells[{g}] is empty. Check that board's cell list");
            else if (!seen.Add(_allCells[g])) Debug.LogError($"_allCells[{g}] is a cell used twice");
        }
    }

    void TickBlocks()
    {
        if (_blocks.Count == 0) return;

        Debug.Log($"Tick: {_blocks.Count} blocks before");

        foreach (Block b in _blocks) b._movesLeft--;
        _blocks.RemoveAll(b => b._movesLeft <= 0);
        RefreshBoard();

        Debug.Log($"Tick: {_blocks.Count} blocks before");
    }

    bool BlockCovers(Block block, int cell)                  // only empty cells are ever covered
    {
        TicTacToeBoard board = _boards[_activeIndex];
        if (board.GetCellText(cell).text != "") return false;
        if (block._target == BlockTarget.RandomCells) return block._cells.Contains(cell);
        return board.GetTileType(cell) == block._tileType;
    }

    public bool IsLocked(int cell) { return IsCoveredBy(BlockMode.Lock, cell); }
    public bool IsNullified(int cell) { return IsCoveredBy(BlockMode.Nullify, cell); }

    bool IsCoveredBy(BlockMode mode, int cell)
    {
        foreach (Block b in _blocks)
            if (b._mode == mode && BlockCovers(b, cell)) return true;
        return false;
    }

    bool PlayerHasPlayableCell()
    {
        TicTacToeBoard board = _boards[_activeIndex];
        for (int i = 0; i < board.CellCount; i++)
            if (board.GetCellText(i).text == "" && !IsLocked(i)) return true;
        return false;
    }

    void RefreshBoard() { _boards[_activeIndex].Refresh(); }
    void ClearBlocks() { _blocks.Clear(); foreach (TicTacToeBoard board in _boards) board.Refresh(); }

    public bool TryGetBlockAt(int cell, out BlockMode mode, out int movesLeft)
    {
        mode = BlockMode.Lock;
        movesLeft = 0;
        bool found = false;

        foreach (Block b in _blocks)
        {
            if (!BlockCovers(b, cell)) continue;

            // if two blocks cover one cell, Lock wins over Nullify
            if (!found || (b._mode == BlockMode.Lock && mode != BlockMode.Lock))
            {
                mode = b._mode;
                movesLeft = b._movesLeft;
                found = true;
            }
        }
        return found;
    }

    public bool AddBlock(BlockTarget target, BlockMode mode, int cellCount, TileType tileType, int duration)
    {
        TicTacToeBoard board = _boards[_activeIndex];
        Block block = new Block
        {
            _target = target,
            _mode = mode,
            _tileType = tileType,
            _movesLeft = duration,
            _cells = new List<int>()
        };

        if (target == BlockTarget.RandomCells)
        {
            List<int> empty = board.GetEmptyCells();
            for (int i = 0; i < cellCount && empty.Count > 0; i++)
            {
                int pick = Random.Range(0, empty.Count);
                block._cells.Add(empty[pick]);
                empty.RemoveAt(pick);
            }
        }

        bool coversSomething = false;
        for (int i = 0; i < board.CellCount; i++)
            if (BlockCovers(block, i)) coversSomething = true;
        if (!coversSomething) return false;                 // nothing to block, skip the cast

        Debug.Log($"AddBlock {target}/{mode}, {duration} moves. Blocks before: {_blocks.Count}");

        _blocks.Add(block);
        if (mode == BlockMode.Lock && !PlayerHasPlayableCell())   // safety rule
        {
            _blocks.Remove(block);
            return false;
        }

        RefreshBoard();
        OnPlayerBlocked?.Invoke(block);
        return true;
    }

    public bool AddBlock(EnemyPowerSO power)
    {
        return AddBlock(power._target, power._mode, power._cellCount, power._tileType, power._duration);
    }

    void HightlightCurrentTurn()
    {
        string sideToMove = _playerMove ? _playerSide : _aiSide;

        if (sideToMove == "X") SetPlayerColors(_playerX, _playerO);
        else SetPlayerColors(_playerO, _playerX);
    }

    void SetBoardInteractable(bool toggle)
    {
        for (int i = 0; i < _buttonList.Length; i++)
        {
            _buttonList[i].GetComponentInParent<Button>().interactable = toggle;
        }
    }

    void SetPlayerSideButtons(bool toggle)
    {
        _playerX._chooseSideButton.interactable = toggle;
        _playerO._chooseSideButton.interactable = toggle;
    }

    void LockAllBoards()
    {
        for(int i = 0; i < _boards.Length; i++)
        {
            _boards[i].Lock();
        }
    }

    void SetPlayerColorsInactive()
    {
        _playerX._panel.color = _inactivePlayerColor._panelColor;
        _playerX._text.color = _inactivePlayerColor._textColor;
        _playerO._panel.color = _inactivePlayerColor._panelColor;
        _playerO._text.color = _inactivePlayerColor._textColor;
    }
}
