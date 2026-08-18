using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI[] _buttonList;
    public GameObject _resultPanel;
    public TextMeshProUGUI _resultText;

    private int _moveCount;
    private string _playerSide;

    private void Awake()
    {
        _resultPanel.SetActive(false);
        SetGameControllerRefOnButtons();
        _playerSide = "X";
    }

    void SetGameControllerRefOnButtons()
    {
        for (int i = 0; i < _buttonList.Length; i++)
        {
            _buttonList[i].GetComponentInParent<GridSpaceButton>().SetGameControllerRef(this);
        }
    }

    public string GetPlayerSide()
    {
        return _playerSide;
    }

    public void EndTurn()
    {
        _moveCount++;

        #region Horitzontal Wins

        if (_buttonList[0].text == _playerSide && _buttonList[1].text == _playerSide && _buttonList[2].text == _playerSide)
        {
            GameOver(false);
        }
        else if (_buttonList[3].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[5].text == _playerSide)
        {
            GameOver(false);
        }
        else if (_buttonList[6].text == _playerSide && _buttonList[7].text == _playerSide && _buttonList[8].text == _playerSide)
        {
            GameOver(false);
        }

        #endregion

        #region Vertical Wins

        else if (_buttonList[0].text == _playerSide && _buttonList[3].text == _playerSide && _buttonList[6].text == _playerSide)
        {
            GameOver(false);
        }
        else if (_buttonList[1].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[7].text == _playerSide)
        {
            GameOver(false);
        }
        else if (_buttonList[2].text == _playerSide && _buttonList[5].text == _playerSide && _buttonList[8].text == _playerSide)
        {
            GameOver(false);
        }

        #endregion
        
        #region Diagonal Wins

        else if (_buttonList[0].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[8].text == _playerSide)
        {
            GameOver(false);
        }
        else if (_buttonList[2].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[6].text == _playerSide)
        {
            GameOver(false);
        }

        #endregion

        if(_moveCount >= 9)
        {
            GameOver(true);
        }

        ChangeSides();
    }

    void GameOver(bool isDraw)
    {
        SetBoardInteractable(false);

        if(isDraw == true)
        {
            SetResultText("It's a Draw!");
        }
        else
        {
            SetResultText(_playerSide + " Wins!");
        }

    }

    void ChangeSides()
    {
        _playerSide = (_playerSide == "X") ? "O" : "X";
    }

    void SetResultText(string value)
    {
        _resultPanel.SetActive(true);
        _resultText.text = value;
    }

    public void RestartGame()
    {
        _playerSide = "X";
        _moveCount = 0;
        _resultPanel.SetActive(false);

        SetBoardInteractable(true);

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
}
