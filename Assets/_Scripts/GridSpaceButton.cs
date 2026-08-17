using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridSpaceButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;

    private GameController _gameController;

    public void SetSpace()
    {
        _buttonText.text = _gameController.GetPlayerSide();
        _button.interactable = false;
        _gameController.EndTurn();
    }

    public void SetGameControllerRef(GameController controller)
    {
        _gameController = controller;
    }
}
