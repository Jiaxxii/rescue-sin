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


        /// <summary>
        /// 返回与玩家之间的距离
        /// </summary>
        /// <returns></returns>
        public float Distance();

        /// <summary>
        /// 返回玩家是否在指定范围内
        /// </summary>
        /// <param name="rangeX">水平坐标的绝对范围 (为空时忽略)</param>
        /// <param name="rangeY">垂直坐标的绝对范围 (为空时忽略)</param>
        /// <returns>如果为 true 则表示玩家在此范围内</returns>
        public bool InRangeAs(Range? rangeX, Range? rangeY);

        /// <summary>
        /// 返回玩家是否在以当前位置的x进行偏移的指定范围内
        /// </summary>
        /// <param name="offsetX">水平坐标的绝对范围 (为空时忽略)</param>
        /// <param name="offsetY">垂直坐标的绝对范围 (为空时忽略)</param>
        /// <returns>如果为 true 则表示玩家在此范围内</returns>
        /// <returns></returns>
        public bool InRangeOffset(Range? offsetX, Range? offsetY);


        /// <summary>
        /// 获取面向玩家的方向
        /// </summary>
        /// <returns></returns>
        public Vector2 GetPlayerDirection();

        /// <summary>
        /// 玩家在右边返回 Vector2.right，在左边返回 Vector2.left 当玩家在一定的范围内中返回 Vector2.zero
        /// </summary>
        /// <param name="tolerance">容差</param>
        /// <returns></returns>
        public Vector2 GetPlayerHorizontalDirection(float tolerance = 0);


        /// <summary>
        /// 玩家在上边返回 Vector2.up，在下边返回 Vector2.down 当玩家在一定的范围内中返回 Vector2.zero
        /// </summary>
        /// <param name="tolerance">容差</param>
        /// <returns></returns>
        public Vector2 GetPlayerVerticalDirection(float tolerance = 0);
    }
}