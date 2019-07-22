using Complete;
using MLAgents;
using UnityEngine;

public class TankAgent : Agent
{
    RayPerception rayPer;
    Rigidbody agentRB;  //cached on initialization
    TankMovement tankMovement;
    TankShooting tankShooting;
    public Transform startPosition;

    public string Name;

    TankAcademy tankAcademy;

    void Awake()
    {
        tankAcademy = FindObjectOfType<TankAcademy>(); //cache the academy
    }

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        rayPer = GetComponent<RayPerception>();
        agentRB = GetComponent<Rigidbody>();
        tankMovement = GetComponent<TankMovement>();
        tankShooting = GetComponent<TankShooting>();
    }

    public override void CollectObservations()
    {
        float rayDistance = 100f;
        float[] rayAngles = { 0f, 45f, 90f, 135f, 180f, 110f, 70f, 225f, 270f, 315f};
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
        Rigidbody rBody = GetComponent<Rigidbody>();
        rBody.MovePosition(startPosition.position);
        TankHealth t = GetComponent<TankHealth>();
        t.ResetHealth();
    }
}

