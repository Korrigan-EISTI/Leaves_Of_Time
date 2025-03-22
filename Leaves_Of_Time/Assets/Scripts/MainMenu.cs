using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void RunGame()
    {
        Debug.Log("Bouton 'Lancer' cliqué ! Tentative de chargement de la scène '3C_Presentation'.");
        SceneManager.LoadScene("3C_Presentation");
    }
}