using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubePositionManager : MonoBehaviour
{
    [SerializeField] private float spawnOffsetY = -0.062f;

    [SerializeField] private GameObject fishPrefab1;
    [SerializeField] private GameObject fishPrefab2;
    [SerializeField] private GameObject fishPrefab3;
    [SerializeField] private GameObject fishPrefab4;
    [SerializeField] private GameObject fishPrefab5;

    [SerializeField] private List<GameObject> cubePositions = new List<GameObject>();

    [SerializeField] private List<GameObject> fishPrefabs = new List<GameObject>();

    [SerializeField] private List<GameObject> spawnedFish = new List<GameObject>();

    [SerializeField] private Vector3 spawnOffset = Vector3.zero;

    [SerializeField] private GameObject fishParent;

    private List<Vector3> spawnedFishPositions = new List<Vector3>();

    [SerializeField] private Dictionary<GameObject, GameObject> fishToCubeMap = new Dictionary<GameObject, GameObject>();


    void Start()
    {
        AddFishtoList();
    }

    private void AddFishtoList()
    {
        fishPrefabs.Add(fishPrefab1);
        fishPrefabs.Add(fishPrefab2);
        fishPrefabs.Add(fishPrefab3);
        fishPrefabs.Add(fishPrefab4);
        fishPrefabs.Add(fishPrefab5);
    }

    public void StoreCubePositions()
    {
        cubePositions.Clear();

        foreach (Transform row in transform)
        {
            foreach (Transform cube in row)
            {
                cubePositions.Add(cube.gameObject); // Store the GameObject instead of position
            }
        }
    }

    public void SpawnFishRandomly()
    {
        System.Random random = new System.Random();

        List<int> targetedIndices = new List<int>();

        for (int i = 0; i < 14; i++)
        {
            int index;
            Vector3 spawnPosition;
            do
            {
                index = random.Next(cubePositions.Count);
                spawnPosition = cubePositions[index].transform.position + spawnOffset;
            } while (IsPositionOccupied(spawnPosition) || targetedIndices.Contains(index));

            targetedIndices.Add(index);

            int fishIndex = random.Next(fishPrefabs.Count);

            Quaternion spawnRotation = Quaternion.Euler(0, 0, -90);

            GameObject fish = Instantiate(fishPrefabs[fishIndex], spawnPosition, spawnRotation);
            fish.transform.SetParent(fishParent.transform);
            spawnedFish.Add(fish);
            fishToCubeMap[fish] = cubePositions[index];

            cubePositions[index].SetActive(true);

        }

        targetedIndices.Clear();
    }


    public void DisableCubeForFish(GameObject fish)
    {
        if (fishToCubeMap.TryGetValue(fish, out GameObject cube))
        {
            cube.SetActive(false);
        }
    }





    private bool IsPositionOccupied(Vector3 position)
    {
        foreach (GameObject cube in cubePositions)
        {
            if (Vector3.Distance(cube.transform.position, position) < 0.01f) // Use a small threshold
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator ChangeSpawnOffsetAfterDelay()
    {
        yield return new WaitForSeconds(5f);

        spawnOffset.y = spawnOffsetY;

        List<Vector3> targetPositions = new List<Vector3>();
        foreach (GameObject fish in spawnedFish)
        {
            Vector3 targetPosition = new Vector3(fish.transform.position.x, fish.transform.position.y + spawnOffset.y, fish.transform.position.z);
            targetPositions.Add(targetPosition);
        }

        float duration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            for (int i = 0; i < spawnedFish.Count; i++)
            {
                GameObject fish = spawnedFish[i];
                Vector3 startPosition = fish.transform.position;
                Vector3 targetPosition = targetPositions[i];

                fish.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < spawnedFish.Count; i++)
        {
            spawnedFish[i].transform.position = targetPositions[i];
        }

        Debug.Log("Spawn offset changed to: " + spawnOffset);
    }

    public void ChangeSpawnOffset(GameObject firstObject, GameObject secondObject)
    {
        if (firstObject != null)
        {
            //Debug.Log("ChangeSpawnOffset called for: " + firstObject.name);
            firstObject.transform.position += new Vector3(0, 0.068f, 0);
        }
    }

    public void StartChangeSpwanedFishPositionCoroutine()
    {
        StartCoroutine(ChangeSpawnOffsetAfterDelay());
    }

    public void MultiTargetLost()
    {
        foreach (GameObject fish in spawnedFish)
        {
            Destroy(fish);
        }
        spawnedFish.Clear();

        spawnedFishPositions.Clear();
        cubePositions.Clear();
    }
}
