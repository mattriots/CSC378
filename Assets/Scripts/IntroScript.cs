using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroScript : MonoBehaviour
{
        public void LoadNextScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
