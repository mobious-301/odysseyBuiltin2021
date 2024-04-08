using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
    [AddComponentMenu("PLAYER TWO/Platformer Project/Player/States/Walk Player State")]
    public class WalkPlayerState : PlayerState
    {
        protected override void OnEnter(Player entity)
        {
            // throw new System.NotImplementedException();
        }

        protected override void OnExit(Player entity)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnStep(Player player)
        {
            var inputDirection = player.inputs.GetMovementCameraDirection();
            // Debug.Log(inputDirection);

			if (inputDirection.sqrMagnitude > 0)
			{
				var dot = Vector3.Dot(inputDirection, player.lateralVelocity);

				if (dot >= player.stats.current.brakeThreshold)//大于默认阈值
				{
                    // Debug.Log(inputDirection);
					player.Accelerate(inputDirection);//加速
					// player.FaceDirectionSmooth(player.lateralVelocity);
				}
				// else
				{
					// player.states.Change<BrakePlayerState>();
				}
			}
            // throw new System.NotImplementedException();
        }
    }
}