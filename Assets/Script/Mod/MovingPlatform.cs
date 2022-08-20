using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public MovingCriteria criteria;
    public Vector3 targetPosition;

    private void Update()
    {
        if (criteria.Check())
        {
            // TODO: Animate me!
            transform.position = targetPosition;
        }
    }
}