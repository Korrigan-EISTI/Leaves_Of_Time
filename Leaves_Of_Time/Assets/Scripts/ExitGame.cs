using UnityEngine;
using UnityEngine.UI;

public class ExitGame : MonoBehaviour
{
    void Start()
    {
        // Récupérer le composant Button et ajouter un écouteur pour le clic
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(QuitGame);
        }
    }

    // Fonction pour quitter le jeu
    void QuitGame()
    {
        // Quitter l'application
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}