using UnityEngine;

public class CursorManager : MonoBehaviour
{
    public Canvas[] canvases; // Assignez ici tous les Canvas que vous voulez vérifier
    private bool isOldCanvasActive = false;

    void Update()
    {
        // Vérifie si au moins un des Canvas est actif
        bool anyCanvasActive = false;
        foreach (Canvas canvas in canvases)
        {
            if (canvas.gameObject.activeSelf)
            {
                anyCanvasActive = true;
                break;
            }
            else
            {
                int z = 0;
            }
        }

        if (anyCanvasActive != isOldCanvasActive)
        {
            UpdateCursorState(anyCanvasActive);
        }
    }

    private void UpdateCursorState(bool showCursor)
    {
        if (showCursor)
        {
            // Affiche le curseur et le déverrouille
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // Cache le curseur et le verrouille
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        isOldCanvasActive = showCursor;
    }
}
