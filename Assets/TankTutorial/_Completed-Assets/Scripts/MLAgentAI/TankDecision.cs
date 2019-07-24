using System.Collections.Generic;
using MLAgents;
using UnityEngine;

public class TankDecision : Decision
{
    public override float[] Decide(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        float RotationDriveMode = vectorObs[0];

        float force = vectorObs[1];

        for (int index = 2; index < vectorObs.Count; index++)
        {
            
        }

        float[] decisions = {1.0f, 1.0f, 1.0f};
        return decisions;
    }

    public override List<float> MakeMemory(List<float> vectorObs, List<Texture2D> visualObs, float reward, bool done, List<float> memory)
    {
        return null;
    }
}
