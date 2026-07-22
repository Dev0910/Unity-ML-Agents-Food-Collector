using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodButton : MonoBehaviour
{
    private bool canUseButton;
    private MeshRenderer meshRenderer;
    [SerializeField] private FoodSpawner foodSpawner;
    [SerializeField] private Material greencolor;
    [SerializeField] private Material redcolor;
    // Start is called before the first frame update
    void Start()
    {
        canUseButton = true;
        foodSpawner = transform.parent.GetComponentInChildren<FoodSpawner>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = greencolor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool CanUseButton()
    {
        return canUseButton;
    }

    public void UseButton()
    {
        if(canUseButton)
        {
            canUseButton = false;
            foodSpawner.SpawnFood();
            meshRenderer.material = redcolor;
        }
    }

    public void ResetButton()
    {
        canUseButton = true;
        meshRenderer.material = greencolor;
        transform.localPosition = new Vector3(Random.Range(-7.5f, 7.5f), -0.4f, Random.Range(-3.5f, 3.5f));
    }
}
