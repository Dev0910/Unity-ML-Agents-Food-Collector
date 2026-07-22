using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class MoveTowardsGoal : Agent
{
    [SerializeField] private FoodButton foodButton;
    [SerializeField] private FoodSpawner foodSpawner;

    //[SerializeField] private Transform targetTransform;
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material lossMaterial;
    [SerializeField] private MeshRenderer plainMesh;

    private Rigidbody agentRigidbody;
    private void Awake()
    {
        agentRigidbody = GetComponent<Rigidbody>();
        foodButton = transform.parent.GetComponentInChildren<FoodButton>();
        foodSpawner = transform.parent.GetComponentInChildren<FoodSpawner>();
    }
    public override void OnEpisodeBegin()
    {
        transform.localPosition = new Vector3(Random.Range(-7.5f, 7.5f), 0, Random.Range(-3.5f, 3.5f));
        //targetTransform.localPosition = new Vector3(Random.Range(-7.5f, 7.5f), 0, Random.Range(-3.5f, 3.5f));
        foodButton.ResetButton();
        foodSpawner.DestroyFood();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(foodButton.CanUseButton() ? 1 : 0);

        Vector3 dirToFoodButton = (foodButton.transform.localPosition - this.transform.localPosition).normalized;
        sensor.AddObservation(dirToFoodButton.x);
        sensor.AddObservation(dirToFoodButton.z);

        sensor.AddObservation(foodSpawner.HasFoodSpawned() ? 1 : 0);

        if(foodSpawner.HasFoodSpawned())
        {
            Vector3 dirToFood = (foodSpawner.GetLastFoodTransform().localPosition - this.transform.localPosition).normalized;
            sensor.AddObservation(dirToFood.x);
            sensor.AddObservation(dirToFood.z);
        }
        else
        {
            sensor.AddObservation(0f);
            sensor.AddObservation(0f);
        }
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        float moveX = actions.DiscreteActions[0];
        float moveZ = actions.DiscreteActions[1];

        Vector3 addForce = new Vector3(0,0,0);
        switch(moveX)
        {
            case 0: addForce.x = 0f;break;
            case 1: addForce.x = -1f;break;
            case 2: addForce.x = +1f;break;
        }

        switch(moveZ)
        {
            case 0: addForce.z = 0f; break;
            case 1: addForce.z = -1f; break;
            case 2: addForce.z = +1f; break;
        }

        float moveSpeed = 5f;
        agentRigidbody.velocity = addForce * moveSpeed + new Vector3(0f, agentRigidbody.velocity.y, 0f);

        bool isUseButtonDown = actions.DiscreteActions[2] == 1;
        if (isUseButtonDown)
        {
            Collider[] colliderArray = Physics.OverlapBox(transform.localPosition, Vector3.one*2f);
            foreach(Collider collider in colliderArray)
            {
                if(collider.TryGetComponent<FoodButton>(out FoodButton foodButton))
                {
                    if(foodButton.CanUseButton())
                    {
                        foodButton.UseButton();
                        AddReward(1f);
                    }
                }
            }
        }
        //AddReward(-1f/MaxStep);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteAction = actionsOut.DiscreteActions;

        switch(Mathf.RoundToInt(Input.GetAxisRaw("Horizontal")))
        {
            case -1: discreteAction[0] = 1; break;
            case 0: discreteAction[0] = 0; break;
            case 1: discreteAction[0] = 2; break;
        }
        switch (Mathf.RoundToInt(Input.GetAxisRaw("Vertical")))
        {
            case -1: discreteAction[1] = 1; break;
            case 0: discreteAction[1] = 0; break;
            case 1: discreteAction[1] = 2; break;
        }
        discreteAction[2] = Input.GetKeyDown(KeyCode.Space) ? 1 : 0;
        if (discreteAction[2] == 1)
        {
            print("E Pressed");
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Food>(out Food food))
        {
            SetReward(1f);
            plainMesh.material = winMaterial;
            Destroy(other.gameObject);
            EndEpisode();
        }
        if(other.TryGetComponent<Wall>(out Wall wall))
        {
            SetReward(-0.5f);
            plainMesh.material = lossMaterial;
            EndEpisode();
        }
    }
}
