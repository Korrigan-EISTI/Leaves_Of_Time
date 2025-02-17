using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Classe abstraite pour tous les objets collectables
public abstract class Items : MonoBehaviour
{
    // M�thode abstraite que chaque item devra impl�menter
    public abstract void ExecuteAction(GameObject player);

    // Quand le joueur entre en collision avec l'item
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ExecuteAction(other.gameObject); // Ex�cute l'action sp�cifique � l'item
            Destroy(gameObject); // D�truit l'item apr�s utilisation
        }
    }
}