using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class TicTacToeBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] _cellTexts = new TextMeshProUGUI[9];

    private Button[] _buttons;
    private GridSpaceButton[] _gridButtons;

    private GameController _gameController;

    private bool _isActive;

    public TileType GetTileType(int index) => _gridButtons[index].CurrentTile;

    public void Initialize(GameController controller)
    {
        _gameController = controller;
        _buttons = new Button[_cellTexts.Length];
        _gridButtons = new GridSpaceButton[_cellTexts.Length];

        for (int i = 0; i < _cellTexts.Length; i++)
        {
            if (_cellTexts[i] == null)
            {
                Debug.LogError($"{name}: Cell Texts slot {i} is empty.", this);
                continue;
            }

            _buttons[i] = _cellTexts[i].GetComponentInParent<Button>();
            _gridButtons[i] = _cellTexts[i].GetComponentInParent<GridSpaceButton>();

            if (_buttons[i] == null || _gridButtons[i] == null)
            {
                Debug.LogError($"{name}: cell {i} ({_cellTexts[i].name}) has no Button or GridSpaceButton above it.", _cellTexts[i]);
                continue;
            }

            _gridButtons[i].SetGameControllerRef(controller);
            _gridButtons[i].SetCellIndex(i);
        }
    }

    public int CellCount => _cellTexts.Length;

    public TextMeshProUGUI GetCellText(int index)
    {
        return _cellTexts[index];
    }

    public void ClaimCell(int index, string side)
    {
        _gridButtons[index].ClaimSpace(side);
    }

    public string[] GetState()
    {
        string[] state = new string[_cellTexts.Length];
        for(int i = 0; i < _cellTexts.Length; i++)
        {
            state[i] = _cellTexts[i].text;
        }
        return state;
    }

    public bool IsFull
    {
        get
        {
            for(int i = 0; i < _cellTexts.Length; i++)
            {
                if (_cellTexts[i].text == "") return false;
            }
            return true;
        }
    }

    public bool HasWon(string side)
    {
        foreach(int[] line in GameController.WinPatterns)
        {
            if (_cellTexts[line[0]].text == side &&
                _cellTexts[line[1]].text == side &&
                _cellTexts[line[2]].text == side)
                return true;
        }
        return false;
    }

    public void Lock()
    {
        _isActive = false;

        for(int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].interactable = false;
        }
    }

    public void Unlock()
    {
        _isActive = true;
        Refresh();
    }

    public void Refresh()
    {
        for (int i = 0; i < _gridButtons.Length; i++)
        {
            if (_gameController.TryGetBlockAt(i, out BlockMode mode, out int movesLeft))
                _gridButtons[i].ShowBlock(mode, movesLeft);
            else
                _gridButtons[i].HideBlock();
        }

        if (!_isActive) return;

        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].interactable = (_cellTexts[i].text == "") && !_gameController.IsLocked(i);  
        }
    }

    public void Clear()
    {
        for(int i = 0; i < _cellTexts.Length; i++)
        {
            _cellTexts[i].text = "";
        }
    }

    public List<int> GetEmptyCells()
    {
        List<int> emptyCells = new List<int>();
        for(int i = 0; i < _cellTexts.Length; i++)
        {
            if (_cellTexts[i].text == "")
            {
                emptyCells.Add(i);
            }
        }
        return emptyCells;
    }
}
