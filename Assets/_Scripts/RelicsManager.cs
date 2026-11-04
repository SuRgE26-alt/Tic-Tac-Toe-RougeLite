using UnityEngine;

public class RelicsManager : MonoBehaviour
{
    RelicsManager _relicsManager;



    private void Awake()
    {
        _relicsManager = this;
        if (_relicsManager == null)
        {
            _relicsManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
