using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(SceneExportManager))]
public class SceneExporter : Editor
{
    private bool isExporting = false; // Indicateur pour l'�tat d'export

    public override void OnInspectorGUI()
    {
        // Dessiner l'inspecteur Unity d'origine
        DrawDefaultInspector();

        // R�cup�rer la r�f�rence au script cible
        SceneExportManager manager = (SceneExportManager)target;

        // D�finir le style du bouton
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

        if (isExporting)
        {
            // Couleur orange pour l'�tat "exporting"
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.normal.background = MakeTexture(2, 2, new Color(1.0f, 0.5f, 0.0f)); // Orange
            buttonStyle.fontStyle = FontStyle.Bold;
        }
        else
        {
            // Couleur Unity par d�faut
            buttonStyle = GUI.skin.button;
        }

        // Texte du bouton en fonction de l'�tat
        string buttonText = isExporting ? "Exporting..." : "Export XML";

        // Ajouter le bouton
        if (GUILayout.Button(buttonText, buttonStyle))
        {
            // Lancer l'export si non d�j� en cours
            if (!isExporting)
            {
                isExporting = true;
                manager.ExportScene();
                isExporting = false;
            }
        }
    }

    // G�n�rer une texture unie pour le fond du bouton
    private Texture2D MakeTexture(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; i++)
        {
            pix[i] = col;
        }

        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }
}
