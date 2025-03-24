using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject defaultCanvas; // Référence au Canvas par défaut
    public GameObject exitMenuCanvas; // Référence au Canvas ExitMenu
    private bool isExitMenuOpen = false; // Pour suivre l'état du menu

    void Start()
    {
        if (defaultCanvas != null)
        {
            defaultCanvas.SetActive(true);
        }
        if (exitMenuCanvas != null)
        {
            exitMenuCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // Gestion de la touche Échap pour afficher/masquer le menu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleExitMenu();
        }
    }

    private void ToggleExitMenu()
    {
        isExitMenuOpen = !isExitMenuOpen; 

        if (isExitMenuOpen)
        {
            // Affiche le ExitMenu et cache le Canvas par défaut
            if (exitMenuCanvas != null)
            {
                exitMenuCanvas.SetActive(true);
            }
            if (defaultCanvas != null)
            {
                defaultCanvas.SetActive(false);
            }
            // Pause le jeu
            Time.timeScale = 0; 
        }
        else
        {
            // Cache le ExitMenu et réaffiche le Canvas par défaut
            if (exitMenuCanvas != null)
            {
                exitMenuCanvas.SetActive(false);
            }
            if (defaultCanvas != null)
            {
                defaultCanvas.SetActive(true);
            }
            // Reprend le jeu
            Time.timeScale = 1; 
        }
    }

    // Fonction publique pour fermer le menu (appelée par le bouton "Resume")
    public void CloseExitMenu()
    {
        isExitMenuOpen = false;
        if (exitMenuCanvas != null)
        {
            exitMenuCanvas.SetActive(false);
        }
        if (defaultCanvas != null)
        {
            defaultCanvas.SetActive(true);
        }
        Time.timeScale = 1; // Reprend le jeu
    }

    // Méthode publique pour vérifier si le menu est ouvert (utilisée par EnvironmentTimeControl)
    public bool IsExitMenuOpen()
    {
        return isExitMenuOpen;
    }
}