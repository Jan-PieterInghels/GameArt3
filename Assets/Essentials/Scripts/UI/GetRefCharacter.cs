using UnityEngine;

public class GetRefCharacter : MonoBehaviour
{
    [Range(1,2)] [SerializeField] int _playerNumber;

    public void ChangeCharacter()
    {
        DestroyAllChildobjects();

        if(GameController.PlayerCharacter[_playerNumber] != null)
            InstatiatePrefab(GameController.PlayerCharacter[_playerNumber]);
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

    private void InstatiatePrefab(GameObject obj)
    {
        GameObject spawnObj = obj;
        Transform spawnPos;
        spawnPos = transform;

        if (obj != null)
        {
            spawnPos.position = new Vector3(spawnPos.position.x, -3.5f, spawnPos.position.z);
            spawnObj.transform.localScale = new Vector3(1, 1, 1);

            if (spawnObj.name == "Character_Brian")
            {
                spawnObj.transform.localScale /= 1.5f;
                spawnPos.position = new Vector3(spawnPos.position.x, spawnPos.position.y + 1, spawnPos.position.z);
            }
        }
        Instantiate(spawnObj, spawnPos);
    }
}
