using UnityEngine;
using System;
using System.IO;

public class SceneExportManager : MonoBehaviour
{
    [Header("Export Settings")]
    public string exportName = "SceneExport.xml";
    public string exportPath = "Exports";
    public Transform startTransform;

    public void ExportScene()
    {
        string backupFolderPath = exportPath + "/BACKUP";

        // Vérifier si le fichier existe déjà
        if (File.Exists(exportPath + "/" + exportName))
        {
            // Créer le dossier BACKUP s'il n'existe pas
            if (!Directory.Exists(backupFolderPath))
            {
                Directory.CreateDirectory(backupFolderPath);
            }

            // Générer un nom unique pour le fichier de backup
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string backupFileName = $"{exportName}_{timestamp}.xml";
            string backupFilePath = Path.Combine(backupFolderPath, backupFileName);

            // Déplacer l'ancien fichier
            File.Move(exportPath + "/" + exportName, backupFilePath);

            Debug.Log($"Existing file moved to backup: {backupFilePath}");
        }

        // Exporter la nouvelle scène
        Debug.Log("Exporting scene...");
        SceneExportUtils.ExportSubHierarchy(startTransform, exportPath + "/" + exportName);
        Debug.Log($"Scene exported to {exportPath + "/" + exportName}");
    }
}
