using System;
using System.Collections.Generic;
using DG.Tweening;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Workspace.FiniteStateMachine;
using Workspace.FiniteStateMachine.ExpandInterface;
using Workspace.FsmObjects.Arms.KettleFsmLogic;
using Workspace.Player;

namespace Workspace.FsmObjects.Arms
{
    public interface IKettle : IPlayerPosition
    {
        Transform Transform { get; }
        Transform FloatPoint { get; }

        Transform Target { get; set; }
        string TagName { get; set; }

        UnityEngine.Camera MainCamera { get; }
        void ChangeState(KettleState state);

        float GetPlayerFloatDistance();
      //  float GetTargetDistance([CanBeNull] string tagName = null);
        Vector2 GetTargetDistanceXY([CanBeNull] string tagName = null);


        Vector3 GetTargetOffsetPosition([CanBeNull] string tagName = null);

        Vector2 FindOffset([CanBeNull] string tagName);

        void UpDateTarget(GameObject target);


        void SetTargetAsPlayer();
    }

    public class Kettle : FsmBehaviour<KettleState, IKettle>, IKettle
    {
        [Serializable]
        public class OffsetProperty
        {
            [SerializeField] private string name;
            [SerializeField] private Vector2 offset;

            public string Name => name;

            public Vector2 Offset => offset;
        }

        public Transform Transform => transform;

        public Transform FloatPoint => floatPoint;
        public UnityEngine.Camera MainCamera => mainCamera;

        [SerializeField] private UnityEngine.Camera mainCamera;

        [Tooltip("水壶的移动目标点")] [SerializeField] private Transform floatPoint;


        [Header("默认(浮空)")] [SerializeField] private KettleIdle.IdleProperty idleProperty;

        [Header("悬浮偏移")] [SerializeField] private List<OffsetProperty> offsetProperties;

        [Header("移动向玩家")] [SerializeField] private KettleMoveTo.MoveToProperty moveToProperty;

        [Header("攻击")] [SerializeField] private KettleAttack.AttackProperty attackProperty;
        // [SerializeField] private KettleMoveToEnemy.MoveToEnemyProperty moveToEnemyProperty;


        public Transform Target { get; set; }
        public string TagName { get; set; }


        protected override void Awake()
        {
            base.Awake();
            StateMachine = new StateMachine<KettleState, IKettle>(this);

            StateMachine.Add(new KettleIdle(this, idleProperty));
            StateMachine.Add(new KettleMoveTo(this, moveToProperty));
            StateMachine.Add(new KettleAttack(this, attackProperty));
            // StateMachine.Add(new KettleMoveToEnemy(this, moveToEnemyProperty));
        }


        private void Start()
        {
            TagName = Player.gameObject.tag;
            Target = FloatPoint;

            StateMachine.ChangeState(KettleState.Idle);
        }

        public float GetPlayerFloatDistance() => Vector3.Distance(FloatPoint.position, CurrentPosition);

        public float GetTargetDistance(string tagName)
        {
            if (string.IsNullOrEmpty(tagName) || Player.CompareTag(tagName)) return GetPlayerFloatDistance();

            var target = GetTargetOffsetPosition(tagName);

            return Vector3.Distance(CurrentPosition, target);
        }

        public Vector2 GetTargetDistanceXY(string tagName = null)
        {
            var target = GetTargetOffsetPosition(tagName);
            return new Vector2(Mathf.Abs(CurrentPosition.x - target.x), Mathf.Abs(CurrentPosition.y - target.y));
        }


        public Vector3 GetTargetOffsetPosition(string tagName)
        {
            if (Player.CompareTag(tagName))
            {
                return Target.position;
            }

            var offset = FindOffset(tagName);
            return new Vector3(Target.position.x + offset.x, Target.position.y + offset.y);
        }

        public Vector2 FindOffset(string tagName)
        {
            if (string.IsNullOrEmpty(tagName) || Player.CompareTag(tagName)) return playerOffset;

            var result = offsetProperties.Find(item => item.Name == tagName);

            return result?.Offset ?? throw new NullReferenceException($"未定义的名称\"{tagName}\"!");
        }

        public void UpDateTarget(GameObject target)
        {
            Target = target.transform;
            TagName = target.tag;
        }

        public void SetTargetAsPlayer()
        {
            TagName = Player.gameObject.tag;
            Target = FloatPoint;
        }
    }
}