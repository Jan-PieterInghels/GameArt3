using UnityEngine;

public class GetCharacter : MonoBehaviour
{
    [Range(1, 2)] [SerializeField] private int _playerNumber = 1;

    private GameController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        DestroyAllChildobjects();

        if (_controller.PlayerCharacter[_playerNumber] != null)
        {
            GameObject go = Instantiate(_controller.PlayerCharacter[_playerNumber], transform);
            PlayerBehaviour beh = go.GetComponent<PlayerBehaviour>();
            beh.PlayerNumber = _playerNumber;
            beh.Initialize();
        }
    }

    private void DestroyAllChildobjects()
    {
        GameObject[] childObjects = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            childObjects[i] = transform.GetChild(i).gameObject;
        }
        foreach (var child in childObjects)
        {
            Destroy(child);
        }
    }
}
