using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
    [AddComponentMenu("PLAYER TWO/Platformer Project/Player/States/Brake Player State")]
    public class BrakePlayerState : PlayerState
    {
        protected override void OnEnter(Player player)
        {
            // throw new System.NotImplementedException();
        }

        protected override void OnExit(Player player)
        {
            // throw new System.NotImplementedException();
        }

        protected override void OnStep(Player player)
        {
            var inputDirection = player.inputs.GetMovementCameraDirection();
			if (player.stats.current.canBackflip &&
				Vector3.Dot(inputDirection, player.transform.forward) < 0 
				&& player.inputs.GetJumpDown()
                )
			{
                Debug.Log(player.inputs.GetJumpDown());
				player.Backflip(player.stats.current.backflipBackwardTurnForce);//后空翻，力添加
			}
			else
			{
                
				// player.SnapToGround();
				// player.Jump();
				// player.Fall();
				player.Decelerate();

				if (player.lateralVelocity.sqrMagnitude == 0)
				{
					player.states.Change<IdlePlayerState>();
				}
			}
        }
    }
}