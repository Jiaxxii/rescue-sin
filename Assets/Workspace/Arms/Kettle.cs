using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Workspace.FiniteStateMachine;
using Workspace.FiniteStateMachine.ExpandInterface;
using Workspace.Player;

namespace Workspace.Arms
{
    public interface IKettle : IPlayerPosition
    {
        Transform Transform { get; }
        Transform FloatPoint { get; }


        float GetPlayerFloatDistance();

        void ChangedState(KettleState kettleState);
    }

    public class Kettle : FsmBehaviour<KettleState, IKettle>, IKettle
    {
        public Transform Transform => transform;

        public Transform FloatPoint => floatPoint;


        [Tooltip("水壶的移动目标点")] [SerializeField] private Transform floatPoint;


        [Header("默认(浮空)")] [SerializeField] private KettleIdle.IdleProperty idleProperty;
        [Header("移动向玩家")] [SerializeField] private KettleMoveToPlayer.MoveToPlayerProperty moveToPlayerProperty;


        private void Awake()
        {
            Add(new KettleIdle(this, idleProperty));
            Add(new KettleMoveToPlayer(this, moveToPlayerProperty));
        }

        private void Start()
        {
            StateMachine = new StateMachine<KettleState, IKettle>(this);
            ChangedState(KettleState.Idle);
        }


        public float GetPlayerFloatDistance() => Vector3.Distance(FloatPoint.position, CurrentPosition);
    }
}