using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void RunGame()
    {
        Debug.Log("Bouton 'Lancer' cliqué ! Tentative de chargement de la scène 'Platforming'.");
        SceneManager.LoadScene("Platforming");
    }
}