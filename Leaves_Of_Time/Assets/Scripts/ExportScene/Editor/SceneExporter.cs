using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[CustomEditor(typeof(SceneExportManager))]
public class SceneExporter : Editor
{
    private bool isExporting = false; // Indicateur pour l'état d'export

    public override void OnInspectorGUI()
    {
        // Dessiner l'inspecteur Unity d'origine
        DrawDefaultInspector();

        // Récupérer la référence au script cible
        SceneExportManager manager = (SceneExportManager)target;

        // Définir le style du bouton
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);

        if (isExporting)
        {
            // Couleur orange pour l'état "exporting"
            buttonStyle.normal.textColor = Color.white;
            buttonStyle.normal.background = MakeTexture(2, 2, new Color(1.0f, 0.5f, 0.0f)); // Orange
            buttonStyle.fontStyle = FontStyle.Bold;
        }
        else
        {
            // Couleur Unity par défaut
            buttonStyle = GUI.skin.button;
        }

        // Texte du bouton en fonction de l'état
        string buttonText = isExporting ? "Exporting..." : "Export XML";

        // Ajouter le bouton
        if (GUILayout.Button(buttonText, buttonStyle))
        {
            // Lancer l'export si non déjà en cours
            if (!isExporting)
            {
                isExporting = true;
                manager.ExportScene();
                isExporting = false;
            }
        }
    }

    // Générer une texture unie pour le fond du bouton
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
