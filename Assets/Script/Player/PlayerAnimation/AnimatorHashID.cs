using UnityEngine;

namespace ROC
{
    internal static class AnimatorHashID
    {
        internal static readonly int OnAttackID = Animator.StringToHash("OnAttack");
        internal static readonly int AngleID = Animator.StringToHash("Angle");
        internal static readonly int AttackID = Animator.StringToHash("Attack");
        internal static readonly int JumpID = Animator.StringToHash("Jump");
        internal static readonly int OnAirID = Animator.StringToHash("OnAir");
        internal static readonly int JumpWaitingID = Animator.StringToHash("JumpWaiting");
        internal static readonly int VerticalID = Animator.StringToHash("Vertical");
        internal static readonly int HorizontalID = Animator.StringToHash("Horizontal");
        internal static readonly int FrontAttackID = Animator.StringToHash("FrontAttack");
        internal static readonly int HandUpID = Animator.StringToHash("HandUp");
    }
}
