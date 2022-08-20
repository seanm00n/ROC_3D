using UnityEngine;

public class MobCriteria : MovingCriteria
{
    public override bool Check()
    {
        return transform.childCount == 0;
    }
}