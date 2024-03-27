using DG.Tweening;
using UnityEngine;
using Workspace.EditorAttribute;
using Workspace.FiniteStateMachine;

namespace Workspace.FsmObjects.Arms.KettleFsmLogic
{
    public class KettleAttack : BaseState<KettleState, IKettle, KettleAttack.AttackProperty>
    {
        [System.Serializable]
        public class AttackProperty
        {
            [SerializeField] [RewriteName("旋转角度")] private float rotateAngle;

            [Space] [SerializeField] [RewriteName("向下持续时间")]
            private float downDuration;

            [SerializeField] [RewriteName("向下运动模式")]
            private Ease downEase;

            [Space] [SerializeField] [RewriteName("向上提持续时间")]
            private float upDuration;

            [SerializeField] [RewriteName("向上提运动模式")]
            private Ease upEase;

            [Space] [SerializeField] [RewriteName("等待此时间后提起")]
            private float waitForTimeSecond;

            [Space] [SerializeField] [RewriteName("返回玩家的触发距离")]
            private float returnPlayerDistance;

            public float ReturnPlayerDistance => returnPlayerDistance;

            public float DownDuration => downDuration;

            public float UpDuration => upDuration;

            public Ease DownEase => downEase;

            public Ease UpEase => upEase;

            public float WaitForTimeSecond => waitForTimeSecond;


            public float RotateAngle => rotateAngle;
        }

        public KettleAttack(IKettle resources, AttackProperty privateRes) : base(resources, privateRes)
        {
        }

        public override KettleState State => KettleState.Attack;


        public override void OnEnter()
        {
            var sequence = DOTween.Sequence();

            sequence.Append(Resources.Transform.DORotate(new Vector3(0, 0, PrivateRes.RotateAngle), PrivateRes.DownDuration).SetEase(PrivateRes.DownEase));

            sequence.AppendCallback(OnPull);

            sequence.AppendInterval(PrivateRes.WaitForTimeSecond);

            sequence.AppendCallback(OnPushUp);

            sequence.Append(Resources.Transform.DORotate(Vector3.zero, PrivateRes.UpDuration).SetEase(PrivateRes.UpEase));

            sequence.AppendCallback(OnActionEnd);
        }


        public override void OnUnityUpdate()
        {
        }

        // 浇水
        private void OnPull()
        {
        }

        // 收
        private void OnPushUp()
        {
        }


        private void OnActionEnd()
        {
            Resources.SetTargetAsPlayer();

            Resources.ChangeState(Resources.GetPlayerFloatDistance() < PrivateRes.ReturnPlayerDistance ? KettleState.MoveTo : KettleState.Idle);
        }

        public override void OnExit()
        {
        }
    }
}