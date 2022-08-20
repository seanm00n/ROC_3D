using DG.Tweening;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public MovingCriteria criteria;
    public Vector3 targetPosition;
    public float moveDuration;
    public Ease moveEase;

    private void Update()
    {
        if (criteria.Check())
        {
            transform.DOMove(targetPosition, moveDuration)
                .SetEase(moveEase);
        }
    }
}