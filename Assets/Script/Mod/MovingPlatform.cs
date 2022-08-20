using DG.Tweening;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public MovingCriteria criteria;
    public Vector3 targetPosition;
    public float moveDuration;
    public Ease moveEase;

    private Tweener transformTween;

    private void Update()
    {
        if (criteria.Check() && transformTween == null)
        {
            transformTween = transform.DOLocalMove(targetPosition, moveDuration)
                .SetEase(moveEase)
                .OnComplete(KillTween);
        }
    }

    private void KillTween()
    {
        transformTween?.Kill();
    }
}