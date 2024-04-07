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

        protected override void OnStep(Player entity)
        {
            Debug.Log("idle");
        }
    }
}