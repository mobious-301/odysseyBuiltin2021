using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
    [AddComponentMenu("PLAYER TWO/Platformer Project/Player/States/Idle Player State")]
    public class IdlePlayerState : PlayerState
    {
        protected override void OnEnter(Player entity)
        {

        }

        protected override void OnExit(Player entity)
        {
        }
        //Entity 的 Update()对状态进行调用
        protected override void OnStep(Player player)
        {
            // Debug.Log(player);
            var inputDirection = player.inputs.GetMovementDirection();
            if (inputDirection.sqrMagnitude > 0 || player.lateralVelocity.sqrMagnitude > 0)
			{
				player.states.Change<WalkPlayerState>();
			}
			// else if (player.inputs.GetCrouchAndCraw())
			// {
			// 	player.states.Change<CrouchPlayerState>();
			// }
        }
    }
}