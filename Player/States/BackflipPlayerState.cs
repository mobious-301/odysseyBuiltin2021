using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
    [AddComponentMenu("PLAYER TWO/Platformer Project/Player/States/Backflip Player State")]
    public class BackflipPlayerState : PlayerState
    {
        protected override void OnEnter(Player player)
        {
            // throw new System.NotImplementedException();
            if (player.stats.current.backflipLockMovement)
			{
				player.inputs.LockMovementDirection();
			}
        }

        protected override void OnExit(Player entity)
        {
            // throw new System.NotImplementedException();
        }

        protected override void OnStep(Player player)
        {
            player.Gravity(player.stats.current.backflipGravity);
			// player.BackflipAcceleration();
            if (player.isGrounded)
			{
				player.lateralVelocity = Vector3.zero;
				player.states.Change<IdlePlayerState>();
			}
            // else if (player.verticalVelocity.y < 0)
			// {
			// 	player.Spin();
			// 	player.AirDive();
			// 	player.StompAttack();
			// 	player.Glide();
			// }
            // throw new System.NotImplementedException();
        }
    }
}