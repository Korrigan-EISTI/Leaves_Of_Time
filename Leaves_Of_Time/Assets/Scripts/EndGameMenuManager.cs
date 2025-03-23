using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameMenuManager : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}