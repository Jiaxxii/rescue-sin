using UnityEngine;

namespace Workspace.FiniteStateMachine.ExpandInterface
{
    public interface IPlayerPosition
    {
        Vector3 CurrentPosition { get; }
        Vector3 PlayerOffsetPosition { get; }

        float Distance(Vector3 position);

        Vector3 InPlayerDirection(Vector3 position);

        bool InRangeX(Vector2 range);
        
        bool InRangeY(Vector2 range);

        bool InPlayerLeft(Vector3 position);
        bool InPlayerRight(Vector3 position);
        bool InPlayerUp(Vector3 position);
        bool InPlayerDown(Vector3 position);
    }
}