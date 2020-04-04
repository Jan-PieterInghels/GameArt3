using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    private void Update()
    {
        if (Input.GetButtonDown("Start_Button"))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}