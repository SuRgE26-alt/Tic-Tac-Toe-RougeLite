using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public TextMeshProUGUI[] _buttonList;
    public GameObject _resultPanel;
    public TextMeshProUGUI _resultText;


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

        #region Horitzontal Wins

        if (_buttonList[0].text == _playerSide && _buttonList[1].text == _playerSide && _buttonList[2].text == _playerSide)
        {
            GameOver();
        }
        else if (_buttonList[3].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[5].text == _playerSide)
        {
            GameOver();
        }
        else if (_buttonList[6].text == _playerSide && _buttonList[7].text == _playerSide && _buttonList[8].text == _playerSide)
        {
            GameOver();
        }

        #endregion

        #region Vertical Wins

        else if (_buttonList[0].text == _playerSide && _buttonList[3].text == _playerSide && _buttonList[6].text == _playerSide)
        {
            GameOver();
        }
        else if (_buttonList[1].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[7].text == _playerSide)
        {
            GameOver();
        }
        else if (_buttonList[2].text == _playerSide && _buttonList[5].text == _playerSide && _buttonList[8].text == _playerSide)
        {
            GameOver();
        }

        #endregion
        
        #region Diagonal Wins

        else if (_buttonList[0].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[8].text == _playerSide)
        {
            GameOver();
        }
        else if (_buttonList[2].text == _playerSide && _buttonList[4].text == _playerSide && _buttonList[6].text == _playerSide)
        {
            GameOver();
        }

        #endregion

        ChangeSides();
    }

    void GameOver()
    {

        for(int i = 0; i < _buttonList.Length; i++)
        {
            _buttonList[i].GetComponentInParent<Button>().interactable = false;
        }


        _resultPanel.SetActive(true);
        _resultText.text = _playerSide + " Wins!";
    }

    void ChangeSides()
    {
        _playerSide = (_playerSide == "X") ? "O" : "X";
    }
}
