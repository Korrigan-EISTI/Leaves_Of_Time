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

        // V�rifier si le fichier existe d�j�
        if (File.Exists(exportPath + "/" + exportName))
        {
            // Cr�er le dossier BACKUP s'il n'existe pas
            if (!Directory.Exists(backupFolderPath))
            {
                Directory.CreateDirectory(backupFolderPath);
            }

            // G�n�rer un nom unique pour le fichier de backup
            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string backupFileName = $"{exportName}_{timestamp}.xml";
            string backupFilePath = Path.Combine(backupFolderPath, backupFileName);

            // D�placer l'ancien fichier
            File.Move(exportPath + "/" + exportName, backupFilePath);

            Debug.Log($"Existing file moved to backup: {backupFilePath}");
        }

        // Exporter la nouvelle sc�ne
        Debug.Log("Exporting scene...");
        SceneExportUtils.ExportSubHierarchy(startTransform, exportPath + "/" + exportName);
        Debug.Log($"Scene exported to {exportPath + "/" + exportName}");
    }
}
