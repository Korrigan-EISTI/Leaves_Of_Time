using UnityEngine;
using UnityEngine.UI;

public class CloseTutorial : MonoBehaviour
{
    [SerializeField] private GameObject tutorialCanvas; // Référence au TutorialCanvas
    [SerializeField] private GameObject accueilCanvas;  // Référence au Canvas "Accueil" 

    void Start()
    {
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(CloseTutorialCanvas);
        }
    }

    void CloseTutorialCanvas()
    {
        if (tutorialCanvas != null)
        {
            tutorialCanvas.SetActive(false); 
        }
        if (accueilCanvas != null)
        {
            accueilCanvas.SetActive(true); 
        }
    }
}