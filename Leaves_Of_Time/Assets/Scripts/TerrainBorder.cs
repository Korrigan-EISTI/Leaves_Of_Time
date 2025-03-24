using UnityEngine;
using System.Collections.Generic;

public class TerrainBorder : MonoBehaviour
{
    public Terrain terrain;
    public List<GameObject> treePrefabs; // Liste de prefabs d'arbres
    public int outerDensity = 60; // Densité d'arbres sur le bord extérieur
    public int innerDensity = 20; // Densité minimale sur la couche la plus intérieure
    public int depthLayers = 3; // Nombre de couches d'arbres en profondeur
    public float depthSpacing = 3f; // Distance entre chaque ligne d'arbres en profondeur
    public Transform parent;

    void Start()
    {
        if (terrain == null)
        {
            Debug.LogError("Terrain non assigné !");
            return;
        }

        if (treePrefabs == null || treePrefabs.Count == 0)
        {
            Debug.LogError("Aucun prefab d'arbre assigné !");
            return;
        }

        CreateInvisibleWalls();
        PlaceTrees();
    }

    void CreateInvisibleWalls()
    {
        float width = terrain.terrainData.size.x;
        float length = terrain.terrainData.size.z;
        float height = terrain.terrainData.size.y;

        Vector3[] positions = {
            new Vector3(0, height / 2, length / 2),     
            new Vector3(width, height / 2, length / 2),  
            new Vector3(width / 2, height / 2, 0),  
            new Vector3(width / 2, height / 2, length)   
        };

        Vector3[] scales = {
            new Vector3(1, height, length),  
            new Vector3(1, height, length),  
            new Vector3(width, height, 1), 
            new Vector3(width, height, 1)   
        };

        for (int i = 0; i < 4; i++)
        {
            GameObject wall = new GameObject("InvisibleWall_" + i);
            wall.transform.position = positions[i];

            BoxCollider collider = wall.AddComponent<BoxCollider>();
            collider.size = scales[i];

            wall.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
    }

    void PlaceTrees()
    {
        float width = terrain.terrainData.size.x;
        float length = terrain.terrainData.size.z;

        for (int layer = 0; layer < depthLayers; layer++)
        {
            float depthOffset = layer * depthSpacing;

            int treeDensity = Mathf.RoundToInt(Mathf.Lerp(outerDensity, innerDensity, (float)layer / (depthLayers - 1)));

            for (int i = 0; i < treeDensity; i++)
            {
                float x = Random.Range(0, width);
                float z = Random.Range(0, length);

                PlaceTree(x, 0 + depthOffset);       // Bord avant
                PlaceTree(x, length - depthOffset);  // Bord arrière
                PlaceTree(0 + depthOffset, z);       // Bord gauche
                PlaceTree(width - depthOffset, z);   // Bord droit
            }
        }
    }

    void PlaceTree(float x, float z)
    {
        if (treePrefabs.Count == 0) return;

        GameObject treePrefab = treePrefabs[Random.Range(0, treePrefabs.Count)];

        // hauteur en fonction du terrain
        float y = terrain.SampleHeight(new Vector3(x, 0, z)) - 0.5f; // Ajustement pour éviter qu'il flotte

        GameObject tree = Instantiate(treePrefab, new Vector3(x, y, z), Quaternion.identity, parent);

        // rotation aléatoire
        tree.transform.Rotate(0, Random.Range(0, 360), 0);

        // échelle aléatoire
        float scaleVariation = Random.Range(0.8f, 1.2f);
        tree.transform.localScale *= scaleVariation;
    }
}
