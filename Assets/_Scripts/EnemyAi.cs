using UnityEngine;
using UnityEngine.UI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private GameController _gameController;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeMove()
    {
        int value = Random.Range(0, _gameController._buttonList.Length);
        var space = _gameController._buttonList[value].GetComponentInParent<GridSpaceButton>();

        if ((space._isFree))
        {
            space.ClaimSpace(_gameController.GetAISide());
        }
    }
}
