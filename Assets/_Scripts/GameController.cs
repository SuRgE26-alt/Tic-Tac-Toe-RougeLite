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

public class GameController : MonoBehaviour
{
    [SerializeField] private EnemyAi _enemyAi;

    public TextMeshProUGUI[] _buttonList;
    public GameObject _resultPanel;
    public TextMeshProUGUI _resultText;
    public GameObject _startInfo;

    private int _moveCount;
    private string _playerSide;
    private string _computerSide;

    public bool _playerMove;
    public float _delay;

    public Player _playerX;
    public Player _playerO;
    public PlayerColor _activePlayerColor;
    public PlayerColor _inactivePlayerColor;


    private void Awake()
    {
        _resultPanel.SetActive(false);
        SetGameControllerRefOnButtons();
        SetPlayerColorsInactive();
        _startInfo.SetActive(true);
        _playerMove = true;
    }

    private void Update()
    {
        if (_playerMove == false)
        {
            _delay += _delay * Time.deltaTime;
            if(_delay >= 100)
            {
                _enemyAi.MakeMove();
            }
        }
    }

    void SetGameControllerRefOnButtons()
    {
        for (int i = 0; i < _buttonList.Length; i++)
        {
            _buttonList[i].GetComponentInParent<GridSpaceButton>().SetGameControllerRef(this);
        }
    }

    public void SetStartingSide(string startingSide)
    {
        _playerSide = startingSide;

        if (_playerSide == "X")
        {
            _computerSide = "O";
            SetPlayerColors(_playerX, _playerO);
        }
        else
        {
            _computerSide = "X";
            SetPlayerColors(_playerO, _playerX);
        }

        StartGame();
    }

    void StartGame()
    {
        SetBoardInteractable(true);
        SetPlayerSideButtons(false);
        _startInfo.SetActive(false);
    }

    public string GetPlayerSide()
    {
        return _playerSide;
    }

    public string GetAISide()
    {
        return _computerSide;
    }

    public void EndTurn()
    {
        _moveCount++;

        #region Horitzontal Wins

        if (_buttonList[0].text == _playerSide && _buttonList[1].text == _playerSide && _buttonList[2].text == _playerSide)
        {
            GameOver(_playerSide);
        }
        else if (_buttonList[3].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[5].text == _playerSide)
        {
            GameOver(_playerSide);
        }
        else if (_buttonList[6].text == _playerSide && _buttonList[7].text == _playerSide && _buttonList[8].text == _playerSide)
        {
            GameOver(_playerSide);
        }

        #endregion

        #region Vertical Wins

        else if (_buttonList[0].text == _playerSide && _buttonList[3].text == _playerSide && _buttonList[6].text == _playerSide)
        {
            GameOver(_playerSide);
        }
        else if (_buttonList[1].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[7].text == _playerSide)
        {
            GameOver(_playerSide);
        }
        else if (_buttonList[2].text == _playerSide && _buttonList[5].text == _playerSide && _buttonList[8].text == _playerSide)
        {
            GameOver(_playerSide);
        }

        #endregion
        
        #region Diagonal Wins

        else if (_buttonList[0].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[8].text == _playerSide)
        {
            GameOver(_playerSide);
        }
        else if (_buttonList[2].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[6].text == _playerSide)
        {
            GameOver(_playerSide);
        }

        #endregion

        #region Horitzontal Wins Computer

        else if (_buttonList[0].text == _computerSide && _buttonList[1].text == _computerSide && _buttonList[2].text == _computerSide)
        {
            GameOver(_computerSide);
        }
        else if (_buttonList[3].text == _computerSide && _buttonList[4].text == _computerSide && _buttonList[5].text == _computerSide)
        {
            GameOver(_computerSide);
        }
        else if (_buttonList[6].text == _computerSide && _buttonList[7].text == _computerSide && _buttonList[8].text == _computerSide)
        {
            GameOver(_computerSide);
        }

        #endregion

        #region Vertical Wins Computer

        else if (_buttonList[0].text == _computerSide && _buttonList[3].text == _computerSide && _buttonList[6].text == _computerSide)
        {
            GameOver(_computerSide);
        }
        else if (_buttonList[1].text == _computerSide && _buttonList[4].text == _computerSide && _buttonList[7].text == _computerSide)
        {
            GameOver(_computerSide);
        }
        else if (_buttonList[2].text == _computerSide && _buttonList[5].text == _computerSide && _buttonList[8].text == _computerSide)
        {
            GameOver(_computerSide);
        }

        #endregion

        #region Diagonal Wins Computer

        else if (_buttonList[0].text == _computerSide && _buttonList[4].text == _computerSide && _buttonList[8].text == _computerSide)
        {
            GameOver(_computerSide);
        }
        else if (_buttonList[2].text == _computerSide && _buttonList[4].text == _computerSide && _buttonList[6].text == _computerSide)
        {
            GameOver(_computerSide);
        }

        #endregion

        else if (_moveCount >= 9)
        {
            GameOver("Draw"); // Draw check
        }
        else
        {
            ChangeSides(); // Change player sides if no win or draw
            _delay = 10f; // Reset delay for computer move
        }
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

    void ChangeSides()
    {
        _playerMove = (_playerMove == true) ? false : true;

        if (_playerMove == true)
        {
            SetPlayerColors(_playerX, _playerO);
        }
        else
        {
            SetPlayerColors(_playerO, _playerX);
        }
    }

    void SetResultText(string value)
    {
        _resultPanel.SetActive(true);
        _resultText.text = value;
    }

    public void RestartGame()
    {
        _moveCount = 0;
        _resultPanel.SetActive(false);
        _startInfo.SetActive(true);

        SetPlayerSideButtons(true);
        SetPlayerColorsInactive();

        _playerMove = true;
        _delay = 10f;

        for (int i = 0; i < _buttonList.Length; i++)
        {
            _buttonList[i].text = "";
        }
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

    void SetPlayerColorsInactive()
    {
        _playerX._panel.color = _inactivePlayerColor._panelColor;
        _playerX._text.color = _inactivePlayerColor._textColor;
        _playerO._panel.color = _inactivePlayerColor._panelColor;
        _playerO._text.color = _inactivePlayerColor._textColor;
    }
}
