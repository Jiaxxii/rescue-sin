using UnityEngine;

namespace Workspace.FiniteStateMachine.ExpandInterface
{
    public interface IPlayerPosition
    {
        /// <summary>
        /// 继承该接口的游戏对象游戏坐标 （允许对坐标进行一定方向的偏移）
        /// </summary>
        Vector3 CurrentPosition { get; }

        /// <summary>
        /// 玩家的坐标 （允许对坐标进行一定方向的偏移）
        /// </summary>
        Vector3 PlayerOffsetPosition { get; }


        Vector3 PlayerLocalScale { get; }
        Vector3 CurrentLocalScale { get; }


        /// <summary>
        /// 返回与玩家之间的距离
        /// </summary>
        /// <returns></returns>
        public float Distance();
        
        
        public Vector2 DistanceVector2();


        /// <summary>
        /// 返回玩家是否在指定范围中
        /// </summary>
        /// <param name="forwardX">面向前方的坐标</param>
        /// <param name="rearX">身后的坐标</param>
        /// <param name="upY">上方坐标</param>
        /// <param name="downY">下方坐标</param>
        /// <returns></returns>
        bool InRangeAs(float? forwardX, float? rearX, float? upY, float? downY);

        /// <summary>
        /// 返回玩家指定范围中 (受 Transform.localScale.x) 影响
        /// </summary>
        /// <param name="offsetForwardX"></param>
        /// <param name="offsetRearX"></param>
        /// <param name="offsetUpY"></param>
        /// <param name="offsetDownY"></param>
        /// <returns></returns>
        bool InRangeOffset(float? offsetForwardX, float? offsetRearX, float? offsetUpY, float? offsetDownY);

        /// <summary>
        /// 获取面向玩家的方向
        /// </summary>
        /// <returns></returns>
        public Vector3 GetPlayerDirection();

        /// <summary>
        /// 玩家在右边返回 Vector2.right，在左边返回 Vector2.left 当玩家在一定的范围内中返回 Vector2.zero
        /// </summary>
        /// <param name="tolerance">容差</param>
        /// <returns></returns>
        public Vector3 GetPlayerHorizontalDirection(float tolerance = 0);


        /// <summary>
        /// 玩家在上边返回 Vector2.up，在下边返回 Vector2.down 当玩家在一定的范围内中返回 Vector2.zero
        /// </summary>
        /// <param name="tolerance">容差</param>
        /// <returns></returns>
        public Vector3 GetPlayerVerticalDirection(float tolerance = 0);
    }
}