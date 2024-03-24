using System;
using UnityEngine;
using Workspace.FiniteStateMachine;
using Range = Workspace.FiniteStateMachine.Range;

namespace Workspace.Enemy.YuKFsmLogic
{
    // public class YuKPlayerInRange : BaseState<YuKState, IYuK, YuKPlayerInRange.PlayerInRangeProperty>
    // {
    //     [System.Serializable]
    //     public class PlayerInRangeProperty
    //     {
    //         [Header("检测角色进入的范围")] [SerializeField] [Tooltip("应该设置为角色面向的坐标")]
    //         private Transform frontPoint;
    //
    //         [SerializeField] [Tooltip("偏移面向的退出点 防止与idle的值发送冲突")]
    //         private float frontOffset;
    //
    //         [SerializeField] [Tooltip("应该设置为角色背向的坐标")]
    //         private Transform rearPoint;
    //
    //         [SerializeField] [Tooltip("偏移背向的退出点 防止与idle的值发送冲突")]
    //         private float rearOffset;
    //
    //
    //         [SerializeField] private float speed;
    //
    //
    //         public float Speed => speed;
    //
    //         public float FrontOffset => frontOffset;
    //         public float RearOffset => rearOffset;
    //
    //
    //         public Transform FrontPoint => frontPoint;
    //         public Transform RearPoint => rearPoint;
    //     }
    //
    //     public YuKPlayerInRange(IYuK resources, PlayerInRangeProperty privateRes) : base(resources, privateRes)
    //     {
    //     }
    //
    //     public override YuKState State => YuKState.PlayerInRange;
    //
    //     private Vector3 _direction;
    //
    //
    //     public override void OnEnter()
    //     {
    //         Resources.YukAnimator.Play("run");
    //     }
    //
    //     public override void OnUnityUpdate()
    //     {
    //         // 获取玩家向“我”走来的方向 向这个方向移动
    //         _direction = Resources.GetPlayerHorizontalDirection() != Vector3.left ? Vector3.left : Vector3.right;
    //
    //         // 判断玩家是否在范围内
    //         if (!Resources.InRangeAs(
    //                 new Range(PrivateRes.FrontPoint.position.x + PrivateRes.FrontOffset, PrivateRes.RearPoint.position.x + PrivateRes.RearOffset), null))
    //         {
    //             Resources.ChangeState(YuKState.Idle);
    //         }
    //     }
    //
    //     public override void OnUnityFixedUpdate()
    //     {
    //         Resources.Transform.position += _direction * (PrivateRes.Speed * Time.fixedDeltaTime);
    //     }
    //
    //     public override void OnExit()
    //     {
    //         _direction = Vector3.zero;
    //     }
    // }
}