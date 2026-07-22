using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    private bool hasfoodSpawned;
    private Transform lastFoodTransform;
    [SerializeField] private GameObject foodPrefab;
    GameObject food;
    // Start is called before the first frame update

    public bool HasFoodSpawned()
    {
        return hasfoodSpawned;
    }
    public Transform GetLastFoodTransform()
    {
        return lastFoodTransform;
    }
    public void SpawnFood()
    {
        //Vector3 randPos = new Vector3(Random.Range(-7.5f, 7.5f), 0, Random.Range(-3.5f, 3.5f));
        Vector3 randPos = new Vector3(-6.5f, 0f, 0f);
        food = Instantiate(foodPrefab);
        food.transform.localPosition = randPos;
    }
    public void DestroyFood()
    {
        Destroy(food);
    }
}
