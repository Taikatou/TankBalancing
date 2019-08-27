using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CarAgent : Agent
{

    string [] detectableObjects = new[] { "Gate", "Spike", "Skid" };

    Car CarControl;

    private RayPerception _rayPer;

    public bool RewardTime = true;

    //variables visible in the inspector
    public Rigidbody rb;

    public override void InitializeAgent()
    {
        base.InitializeAgent();
        _rayPer = GetComponent<RayPerception>();
        CarControl = GetComponent<Car>();
        CarControl.ControlAI = true;
    }

    public override void CollectObservations()
    {
        const float rayDistance = 35f;
        float[] rayAngles = { 20f, 90f, 160f, 45f, 135f, 70f, 110f };
        float[] rayAngles1 = { 25f, 95f, 165f, 50f, 140f, 75f, 115f };
        float[] rayAngles2 = { 15f, 85f, 155f, 40f, 130f, 65f, 105f };

        AddVectorObs(transform.InverseTransformDirection(rb.velocity));

        AddVectorObs(_rayPer.Perceive(rayDistance, rayAngles, detectableObjects, 0f, 0f));
        AddVectorObs(_rayPer.Perceive(rayDistance, rayAngles1, detectableObjects, 0f, 5f));
        AddVectorObs(_rayPer.Perceive(rayDistance, rayAngles2, detectableObjects, 0f, 10f));
    }

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
        float turn = GetDecision(vectorAction[0]);

        CarControl.ControlRotation = turn;

        if (RewardTime)
        {
            float timePunishment = -1f / agentParameters.maxStep;
            // Penalty given each step to encourage agent to finish task quickly.
            AddReward(timePunishment);
        }
    }

    public void PassGate()
    {
        AddReward(1.0f);
    }
}
