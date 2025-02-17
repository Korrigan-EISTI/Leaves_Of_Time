using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe abstraite pour tous les objets collectables
public abstract class Items : MonoBehaviour
{
    // Méthode abstraite que chaque item devra implémenter
    public abstract void ExecuteAction(GameObject player);

    // Quand le joueur entre en collision avec l'item
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExecuteAction(other.gameObject); // Exécute l'action spécifique à l'item
            Destroy(gameObject); // Détruit l'item après utilisation
        }
    }
}