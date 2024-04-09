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
            // throw new System.NotImplementedException();
        }

        protected override void OnStep(Player player)
        {
            var inputDirection = player.inputs.GetMovementCameraDirection();//获取相机空间水平输入 //控制器输入 操作  世界空间方向  //输入目标水平方向
            // Debug.Log(inputDirection);

			if (inputDirection.sqrMagnitude > 0)
			{
				var dot = Vector3.Dot(inputDirection, player.lateralVelocity);//输入方向在现在速度方向上的投影>-0.8，也就是非现在速度方向的正背面

				if (dot >= player.stats.current.brakeThreshold||dot>-10)//大于默认阈值 //默认实现是反方向加速时 触发刹车。添加dot>-10直接转向
				{
                    // Debug.Log(inputDirection);
					player.Accelerate(inputDirection);//加速 和 方向控制
                    // Debug.Log(player.lateralVelocity);
					player.FaceDirectionSmooth(player.lateralVelocity);//当前方向到目标 水平方向的平滑
				}
				// else
				{
					// player.states.Change<BrakePlayerState>();
				}
			}
            else
			{
				player.Friction();

				if (player.lateralVelocity.sqrMagnitude <= 0)
				{
					player.states.Change<IdlePlayerState>();
				}
                // player.states.Change<IdlePlayerState>();
			}
            // throw new System.NotImplementedException();
        }
    }
}