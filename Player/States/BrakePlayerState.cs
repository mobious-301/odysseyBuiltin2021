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
            player.Decelerate();
            if (player.lateralVelocity.sqrMagnitude == 0)
				{
					player.states.Change<IdlePlayerState>();
				}
            // throw new System.NotImplementedException();
        }
    }
}