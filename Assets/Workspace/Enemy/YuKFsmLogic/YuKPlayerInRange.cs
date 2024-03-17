using Workspace.FiniteStateMachine;

namespace Workspace.Enemy.YuKFsmLogic
{
    public class YuKPlayerInRange : BaseState<YuKState,IYuK,YuKPlayerInRange.PlayerInRangeProperty>
    {
        [System.Serializable]
        public class PlayerInRangeProperty
        {
            
        }

        public YuKPlayerInRange(IYuK resources, PlayerInRangeProperty privateRes) : base(resources, privateRes)
        {
        }

        public override YuKState State => YuKState.PlayerInRange;
        
        public override void OnEnter()
        {
            throw new System.NotImplementedException();
        }

        public override void OnUnityUpdate()
        {
            throw new System.NotImplementedException();
        }

        public override void OnExit()
        {
            throw new System.NotImplementedException();
        }
    }
}