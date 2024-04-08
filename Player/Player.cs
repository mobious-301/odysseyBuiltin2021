using UnityEngine;
namespace PLAYERTWO.PlatformerProject
{
    public class Player : Entity<Player>
    {
        public PlayerEvents playerEvents;
        public PlayerInputManager inputs { get; protected set; }
        public PlayerStatsManager stats { get; protected set; }
        protected virtual void InitializeInputs() => inputs = this.GetComponent<PlayerInputManager>();
        protected virtual void InitializeStats() => stats = GetComponent<PlayerStatsManager>();
        protected override void Awake()
		{
            Debug.Log(this.GetComponent<PlayerInputManager>());
			base.Awake();
			InitializeInputs();
			InitializeStats();
			// InitializeHealth();
			// InitializeTag();
			// InitializeRespawn();

			// entityEvents.OnGroundEnter.AddListener(() =>
			// {
			// 	ResetJumps();
			// 	ResetAirSpins();
			// 	ResetAirDash();
			// });

			// entityEvents.OnRailsEnter.AddListener(() =>
			// {
			// 	ResetJumps();
			// 	ResetAirSpins();
			// 	ResetAirDash();
			// 	StartGrind();
			// });
		}


        public virtual void Accelerate(Vector3 direction)
        {
            var turningDrag = isGrounded && inputs.GetRun() ? stats.current.runningTurningDrag : stats.current.turningDrag;
            var acceleration = isGrounded && inputs.GetRun() ? stats.current.runningAcceleration : stats.current.acceleration;
            var finalAcceleration = isGrounded ? acceleration : stats.current.airAcceleration;
            var topSpeed = inputs.GetRun() ? stats.current.runningTopSpeed : stats.current.topSpeed;

            Accelerate(direction, turningDrag, finalAcceleration, topSpeed);

            if (inputs.GetRunUp())
            {
                lateralVelocity = Vector3.ClampMagnitude(lateralVelocity, topSpeed);
            }
        }
    }
}