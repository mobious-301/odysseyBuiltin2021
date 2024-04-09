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





















    }
}