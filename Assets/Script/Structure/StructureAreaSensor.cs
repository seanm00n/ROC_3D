using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureAreaSensor : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 10)
            StructureSpawn_Test.isArea = true;
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer == 10)
            StructureSpawn_Test.isArea = false;
    }
}
