using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public GameObject plane; 
    public GameObject cubePrefab; 
    public int gridWidth = 10; 
    public int gridHeight = 10; 
    public float cubeSize = 1f; 

    private List<Vector3> cubePositions = new List<Vector3>();

    void Start()
    {
        SpawnGrid();
    }

    void SpawnGrid()
    {
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                float xPosition = plane.transform.position.x + (i * cubeSize);
                float zPosition = plane.transform.position.z + (j * cubeSize);
                Vector3 cubePosition = new Vector3(xPosition, 0, zPosition);

              
                GameObject cube = Instantiate(cubePrefab, cubePosition, Quaternion.identity);
                cubePositions.Add(cubePosition);
            }
        }
    }

    void OnDrawGizmos()
    {
   
        foreach (Vector3 position in cubePositions)
        {
            Gizmos.DrawCube(position, Vector3.one * cubeSize);
        }
    }
}
