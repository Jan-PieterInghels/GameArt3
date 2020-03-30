using UnityEngine;

public class GetCharacter : MonoBehaviour
{
    [Range(1, 2)] [SerializeField] private int _playerNumber = 1;

    // Start is called before the first frame update
    void Start()
    {
        DestroyAllChildobjects();

        if (GameController.PlayerCharacter[_playerNumber] != null)
        {
            GameObject go = Instantiate(GameController.PlayerCharacter[_playerNumber], transform);
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
