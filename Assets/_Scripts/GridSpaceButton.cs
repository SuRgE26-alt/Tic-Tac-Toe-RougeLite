using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridSpaceButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private TilesSO _tileProperty;

    private GameController _gameController;

    public bool _isFree => _button.interactable;

    void CheckAIInput()
    {
        
    }

    public void SetSpace()
    {
        if(_gameController._playerMove == true)
        {
            ClaimSpace(_gameController.GetPlayerSide());
        }
    }

    public void ClaimSpace(string side)
    {
        _buttonText.text = side;
        _button.interactable = false;
        ActivteTile(side);
        _gameController.EndTurn();
    }

    void ActivteTile(string side)
    {
        switch (_tileProperty._tileType)
        {
            case TileType.Damage:
                Debug.Log($"{side} Damage tile activated!");
                if(side == _gameController.GetPlayerSide())
                    _gameController._enemyHealth.TakeDamage(_tileProperty._attackTileDamage);
                else
                    _gameController._playerHealth.TakeDamage(_tileProperty._attackTileDamage);
                break;
            case TileType.Shield:
                Debug.Log($"{side} Shield tile activated!");
                break;
            case TileType.Heal:
                Debug.Log($"{side} Heal tile activated!");
                break;
            case TileType.None:
                Debug.Log($"{side} No special effect.");
                break;
        }
    }

    public void SetGameControllerRef(GameController controller)
    {
        _gameController = controller;
    }
}
