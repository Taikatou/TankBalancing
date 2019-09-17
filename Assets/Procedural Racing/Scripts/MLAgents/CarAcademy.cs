using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;

public class CarAcademy : Academy
{
    public CarAgent car;
    public WorldGenerator worldGenerator;
    public override void AcademyReset()
    {
        worldGenerator.Destroy();
        worldGenerator.GenerateMap();
        car.Reset();
    }

    
}
