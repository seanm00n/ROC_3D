using UnityEngine;

public class Floor1Criteria : MovingCriteria
{
    public override bool Check()
    {
        return Input.GetKeyDown(KeyCode.P);
    }
}