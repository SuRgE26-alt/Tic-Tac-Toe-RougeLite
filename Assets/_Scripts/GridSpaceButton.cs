using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridSpaceButton : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private TilesSO _tileProperty;

    [Header("Block visuals")]
    [SerializeField] private GameObject _blockOverlay;      // shown when the cell is locked
    [SerializeField] private GameObject _nullifyOverlay;    // shown when the tile effect is nullified
    [SerializeField] private Color _lockColor = new Color(0f, 0f, 0f, 0.6f);
    [SerializeField] private Color _nullifyColor = new Color(0.6f, 0.2f, 0.8f, 0.4f);

    private GameController _gameController;
    private int _cellIndex;

    public TileType CurrentTile => _tileProperty._tileType;

    public bool _isFree => _button.interactable;

    void CheckAIInput()
    {
        
    }

    public void ShowBlock(BlockMode mode, int movesLeft)
    {
        bool isLock = (mode == BlockMode.Lock);

        SetOverlay(_blockOverlay, isLock, _lockColor);
        SetOverlay(_nullifyOverlay, !isLock, _nullifyColor);
    }

    public void HideBlock()
    {
        if (_blockOverlay != null) _blockOverlay.SetActive(false);
        if (_nullifyOverlay != null) _nullifyOverlay.SetActive(false);
    }

    void SetOverlay(GameObject overlay, bool show, Color color)
    {
        if (overlay == null) return;

        overlay.SetActive(show);
        if (!show) return;

        Image image = overlay.GetComponent<Image>();
        if (image != null) image.color = color;
    }

    public void SetCellIndex(int cellIndex)
    {
        _cellIndex = cellIndex;
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
        bool nullified = side == _gameController.GetPlayerSide() && _gameController.IsNullified(_cellIndex);

        _buttonText.text = side;
        _button.interactable = false;

        if (nullified) Debug.Log($"{side}: tile effect has been nullified on cell {_cellIndex}");
        ActivteTile(side);
        _gameController.EndTurn();
    }

    void ActivteTile(string side)
    {
        switch (_tileProperty._tileType)
        {
            case TileType.Damage:
                //Debug.Log($"{side} Damage tile activated!");
                if(side == _gameController.GetPlayerSide())
                    _gameController._enemyHealth.TakeDamage(_tileProperty._attackTileDamage);
                else
                    _gameController._playerHealth.TakeDamage(_tileProperty._attackTileDamage);
                break;
            case TileType.Shield:
                //Debug.Log($"{side} Shield tile activated!");
                break;
            case TileType.Heal:
                //Debug.Log($"{side} Heal tile activated!");
                break;
            case TileType.None:
                //Debug.Log($"{side} No special effect.");
                break;
        }
    }

    public void SetGameControllerRef(GameController controller)
    {
        _gameController = controller;
    }
}
