using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
    [AddComponentMenu("PLAYER TWO/Platformer Project/Player/States/Fall Player State")]
    public class FallPlayerState : PlayerState
    {
        protected override void OnEnter(Player entity)
        {
            // throw new System.NotImplementedException();
        }

        protected override void OnExit(Player entity)
        {
            // throw new System.NotImplementedException();
        }

        protected override void OnStep(Player player)
        {
           	player.Gravity();
			player.SnapToGround();
			player.FaceDirectionSmooth(player.lateralVelocity);
			// player.AccelerateToInputDirection();
			// player.Jump();
			// player.Spin();
			// player.PickAndThrow();
			// player.AirDive();
			// player.StompAttack();
			// player.LedgeGrab();
			// player.Dash();
			// player.Glide();

			if (player.isGrounded)
			{
				player.states.Change<IdlePlayerState>();
			}
        }
    }
}