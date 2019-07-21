using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using Complete;

public class TankAgent : Agent
{
    RayPerception rayPer;
    Rigidbody agentRB;  //cached on initialization
    TankMovement tankMovement;
    TankShooting tankShooting;

    public float spawnAreaMarginMultiplier = 0.5f;

    [HideInInspector]
    public Bounds areaBounds;

    public GameObject ground;

    void Awake()
    {
        //academy = FindObjectOfType<PushBlockAcademy>(); //cache the academy
    }

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        areaBounds = ground.GetComponent<Collider>().bounds;
        rayPer = GetComponent<RayPerception>();
        agentRB = GetComponent<Rigidbody>();
        tankMovement = GetComponent<TankMovement>();
        tankShooting = GetComponent<TankShooting>();
    }

    public override void CollectObservations()
    {
        float rayDistance = 12f;
        float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f };
        var detectableObjects = new[] { "tank", "wall", "bullet" };
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        AddVectorObs(rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 1.5f, 0f));
    }

    /// <summary>
    /// Called every step of the engine. Here the agent takes an action.
    /// </summary>
    ///
    private int GetDecision(float input)
    {
        int action = Mathf.FloorToInt(input);

        int output = 0;
        switch (input)
        {
            case 1:
                output = 1;
                break;
            case 2:
                output = -1;
                break;
        }
        return output;
    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        float moveForward = GetDecision(vectorAction[0]);
        
        float turn = GetDecision(vectorAction[1]);

        // Penalty given each step to encourage agent to finish task quickly.

        tankMovement.UpdateAgent(moveForward, turn);

        tankShooting.UpdateAI(vectorAction[2]);
    }

    public override void AgentReset()
    {
        base.AgentReset();
        this.transform.position = GetRandomSpawnPos();
    }

    public Vector3 GetRandomSpawnPos()
    {
        bool foundNewSpawnLocation = false;
        Vector3 randomSpawnPos = Vector3.zero;
        while (foundNewSpawnLocation == false)
        {
            float randomPosX = Random.Range(-areaBounds.extents.x * spawnAreaMarginMultiplier,
                                areaBounds.extents.x * spawnAreaMarginMultiplier);

            float randomPosZ = Random.Range(-areaBounds.extents.z * spawnAreaMarginMultiplier,
                                            areaBounds.extents.z * spawnAreaMarginMultiplier);
            randomSpawnPos = ground.transform.position + new Vector3(randomPosX, 0f, randomPosZ);
            if (Physics.CheckBox(randomSpawnPos, new Vector3(2.5f, 0.01f, 2.5f)) == false)
            {
                foundNewSpawnLocation = true;
            }
        }
        return randomSpawnPos;
    }

}

