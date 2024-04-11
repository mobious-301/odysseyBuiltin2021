using System;
using UnityEngine;
namespace PLAYERTWO.PlatformerProject
{
    public class Player : Entity<Player>
    {public PlayerEvents playerEvents;

		public Transform pickableSlot;
		public Transform skin;

		protected Vector3 m_respawnPosition;
		protected Quaternion m_respawnRotation;

		protected Vector3 m_skinInitialPosition;
		protected Quaternion m_skinInitialRotation;

		/// <summary>
		/// Returns the Player Input Manager instance.
		/// </summary>
		public PlayerInputManager inputs { get; protected set; }

		/// <summary>
		/// Returns the Player Stats Manager instance.
		/// </summary>
		public PlayerStatsManager stats { get; protected set; }

		/// <summary>
		/// Returns the Health instance.
		/// </summary>
		// public Health health { get; protected set; }

		/// <summary>
		/// Returns true if the Player is on water.
		/// </summary>
		public bool onWater { get; protected set; }

		/// <summary>
		/// Returns true if the Player is holding an object.
		/// </summary>
		public bool holding { get; protected set; }

		/// <summary>
		/// Returns how many times the Player jumped.
		/// </summary>
		public int jumpCounter { get; protected set; }

		/// <summary>
		/// Returns how many times the Player performed an air spin.
		/// </summary>
		public int airSpinCounter { get; protected set; }

		/// <summary>
		/// Returns how many times the Player performed a Dash.
		/// </summary>
		/// <value></value>
		public int airDashCounter { get; protected set; }

		/// <summary>
		/// The last time the Player performed an dash.
		/// </summary>
		/// <value></value>
		public float lastDashTime { get; protected set; }

		/// <summary>
		/// Returns the normal of the last wall the Player touched.
		/// </summary>
		public Vector3 lastWallNormal { get; protected set; }

		/// <summary>
		/// Returns the Pole instance in which the Player is colliding with.
		/// </summary>
		// public Pole pole { get; protected set; }

		/// <summary>
		/// Returns the Collider of the water the Player is swimming.
		/// </summary>
		public Collider water { get; protected set; }

		/// <summary>
		/// Return the Pickable instance which the Player is holding.
		/// </summary>
		// public Pickable pickable { get; protected set; }

		/// <summary>
		/// Returns true if the Player health is not empty.
		/// </summary>
		// public virtual bool isAlive => !health.isEmpty;
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

        		public virtual void BackflipAcceleration()
		{
			var direction = inputs.GetMovementCameraDirection();
			Accelerate(direction, stats.current.backflipTurningDrag, stats.current.backflipAirAcceleration, stats.current.backflipTopSpeed);
		}




        public virtual void Decelerate() => Decelerate(stats.current.deceleration);
        // 角色或物体的横向速度（lateralVelocity）正在被平滑地减少到零，这个过程是通过使用其摩擦属性（friction stats）来实现的。
        public virtual void Friction()
        {
            if (OnSlopingGround())
                Decelerate(stats.current.slopeFriction);
            else
                Decelerate(stats.current.friction);
        }
        /// Rotate the Player forward to a given direction.
        public virtual void FaceDirectionSmooth(Vector3 direction) => FaceDirection(direction, stats.current.rotationSpeed);


        public virtual void Backflip(float force)
		{
			if (stats.current.canBackflip && !holding)
			{
				verticalVelocity = Vector3.up * stats.current.backflipJumpHeight;
				lateralVelocity = -transform.forward * force;
				states.Change<BackflipPlayerState>();
				playerEvents.OnBackflip.Invoke();
			}
		}

        internal void Gravity()//重力赋值
        {
            if(!isGrounded&&verticalVelocity.y>-stats.current.gravityTopSpeed){
                var speed = verticalVelocity.y;
                var force = verticalVelocity.y > 0 ? stats.current.gravity : stats.current.fallGravity;//重力方向 跳跃保持
                speed -= force * gravityMultiplier * Time.deltaTime;
				speed = Mathf.Max(speed, -stats.current.gravityTopSpeed);//重力最大值
				verticalVelocity = new Vector3(0, speed, 0);
            }
        }
        public virtual void SnapToGround() => SnapToGround(stats.current.snapForce);


        public virtual void Jump(float height)
		{
			jumpCounter++;
			verticalVelocity = Vector3.up * height;//垂直速度向上
			states.Change<FallPlayerState>();
			playerEvents.OnJump?.Invoke();
		}

        public virtual void Jump()//跳跃功能
		{
			var canMultiJump = (jumpCounter > 0) && (jumpCounter < stats.current.multiJumps);//多段跳
			var canCoyoteJump = (jumpCounter == 0) && (Time.time < lastGroundTime + stats.current.coyoteJumpThreshold);
			var holdJump = !holding || stats.current.canJumpWhileHolding;

			if ((isGrounded || onRails || canMultiJump || canCoyoteJump) && holdJump)
			{
				if (inputs.GetJumpDown())
				{
					Jump(stats.current.maxJumpHeight);
				}
			}

			if (inputs.GetJumpUp() && (jumpCounter > 0) && (verticalVelocity.y > stats.current.minJumpHeight))
			{
				verticalVelocity = Vector3.up * stats.current.minJumpHeight;
			}
		}

    }
}