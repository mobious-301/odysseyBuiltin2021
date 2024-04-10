using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Splines;
namespace PLAYERTWO.PlatformerProject
{
    public abstract class Entity : MonoBehaviour
    {

    }
    public abstract class Entity<T> : Entity where T : Entity<T>
    {
        public EntityStateManager<T> states { get; protected set; }
        protected virtual void HandleStates() => states.Step();
        protected virtual void InitializeStateManager() => states = GetComponent<EntityStateManager<T>>();
        protected virtual void Awake()
        {
            InitializeController();
            // InitializePenetratorCollider();
            InitializeStateManager();
        }
        protected virtual void Update()
        {
            if (controller.enabled)
            {
                HandleStates();
                HandleController();
                // HandleSpline();
                HandleGround();
                // HandleContacts();
                // OnUpdate();
            }
        }
        protected readonly float m_groundOffset = 0.1f;
        protected readonly float m_penetrationOffset = -0.1f;
        protected readonly float m_slopingGroundAngle = 20f;
        public Vector3 center => controller.center;
        public Vector3 position => transform.position + center;
        public float radius => controller.radius;
        public virtual bool SphereCast(Vector3 direction, float distance,
            out RaycastHit hit, int layer = Physics.DefaultRaycastLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            var castDistance = Mathf.Abs(distance - radius);
            return Physics.SphereCast(position, radius, direction,
                out hit, castDistance, layer, queryTriggerInteraction);
        }

        public virtual bool SphereCast(Vector3 direction, float distance, int layer = Physics.DefaultRaycastLayers,
            QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.Ignore)
        {
            return SphereCast(direction, distance, out _, layer, queryTriggerInteraction);
        }
        public virtual bool canStandUp => !SphereCast(Vector3.up, originalHeight);



        public Vector3 stepPosition => position - transform.up * (height * 0.5f - controller.stepOffset);
        public virtual bool IsPointUnderStep(Vector3 point) => stepPosition.y > point.y;
        protected virtual bool EvaluateLanding(RaycastHit hit)
        {
            return IsPointUnderStep(hit.point) && Vector3.Angle(hit.normal, Vector3.up) < controller.slopeLimit;
        }

        public float lastGroundTime { get; protected set; }
        // public EntityEvents entityEvents;
        public bool isGrounded { get; protected set; }
        protected virtual void ExitGround()
        {
            if (isGrounded)
            {
                isGrounded = false;
                transform.parent = null;
                lastGroundTime = Time.time;
                verticalVelocity = Vector3.Max(verticalVelocity, Vector3.zero);
                // entityEvents.OnGroundExit?.Invoke();
            }
        }
        public RaycastHit groundHit;
        protected virtual void EnterGround(RaycastHit hit)
        {
            if (!isGrounded)
            {
                groundHit = hit;
                isGrounded = true;
                // entityEvents.OnGroundEnter?.Invoke();
            }
        }
        protected virtual void HandleHighLedge(RaycastHit hit) { }
        
		public Vector3 groundNormal { get; protected set; }
        		public Vector3 localSlopeDirection { get; protected set; }
        		protected virtual void UpdateGround(RaycastHit hit)
		{
			if (isGrounded)
			{
				groundHit = hit;
				groundNormal = groundHit.normal;
				groundAngle = Vector3.Angle(Vector3.up, groundHit.normal);
				localSlopeDirection = new Vector3(groundNormal.x, 0, groundNormal.z).normalized;
				// transform.parent = hit.collider.CompareTag(GameTags.Platform) ? hit.transform : null;
			}
		}
        protected virtual void HandleGround()
        {
            if (onRails) return;

            var distance = (height * 0.5f) + m_groundOffset;

            if (SphereCast(Vector3.down, distance, out var hit) && verticalVelocity.y <= 0)
            {
                if (!isGrounded)
                {
                    if (EvaluateLanding(hit))
                    {
                        EnterGround(hit);
                    }
                    else
                    {
                        HandleHighLedge(hit);
                    }
                }
                else if (IsPointUnderStep(hit.point))
                {
                    UpdateGround(hit);

                    if (Vector3.Angle(hit.normal, Vector3.up) >= controller.slopeLimit)
                    {
                        // HandleSlopeLimit(hit);
                    }
                }
                else
                {
                    HandleHighLedge(hit);
                }
            }
            else
            {
                ExitGround();
            }
        }
        public CharacterController controller { get; protected set; }
        public float originalHeight { get; protected set; }
        protected virtual void InitializeController()
        {
            controller = GetComponent<CharacterController>();

            if (!controller)
            {
                controller = gameObject.AddComponent<CharacterController>();
            }

            controller.skinWidth = 0.005f;
            controller.minMoveDistance = 0;
            originalHeight = controller.height;
        }
        protected virtual void HandleController()
        {
            if (controller.enabled)
            {
                controller.Move(velocity * Time.deltaTime);
                return;
            }

            transform.position += velocity * Time.deltaTime;
        }
        public Vector3 velocity { get; set; }

        public Vector3 lateralVelocity
        {
            get { return new Vector3(velocity.x, 0, velocity.z); }
            set { velocity = new Vector3(value.x, velocity.y, value.z); }
        }
        public bool nisGrounded { get; protected set; } = true;
        public bool onRails { get; set; }

        public float accelerationMultiplier { get; set; } = 1f;

        public float gravityMultiplier { get; set; } = 1f;

        public float topSpeedMultiplier { get; set; } = 1f;

        public float turningDragMultiplier { get; set; } = 1f;

        public float decelerationMultiplier { get; set; } = 1f;
        /// <summary>
        /// 移动的实现 Accelerate 方法看起来是一个用于加速物体运动的函数，其中参数包括：

        // direction：表示物体加速的方向。
        // turningDrag：表示物体在转向时的阻力或减速度。
        // finalAcceleration：表示物体最终的加速度。
        // topSpeed：表示物体的最大速度限制。
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="turningDrag"></param>
        /// <param name="acceleration"></param>
        /// <param name="topSpeed"></param>

        public virtual void Accelerate(Vector3 direction, float turningDrag, float acceleration, float topSpeed)
        {
            // Debug.Log(direction);
            if (direction.sqrMagnitude > 0)
            {
                var speed = Vector3.Dot(direction, lateralVelocity);
                var velocity = direction * speed;
                var turningVelocity = lateralVelocity - velocity;
                var turningDelta = turningDrag * turningDragMultiplier * Time.deltaTime;
                var targetTopSpeed = topSpeed * topSpeedMultiplier;

                if (lateralVelocity.magnitude < targetTopSpeed || speed < 0)
                {
                    speed += acceleration * accelerationMultiplier * Time.deltaTime;
                    speed = Mathf.Clamp(speed, -targetTopSpeed, targetTopSpeed);
                }

                velocity = direction * speed;
                turningVelocity = Vector3.MoveTowards(turningVelocity, Vector3.zero, turningDelta);
                lateralVelocity = velocity + turningVelocity;
            }
        }

        public float groundAngle { get; protected set; }
        // protected readonly float m_slopingGroundAngle = 20f;
        public float height => controller.height;
        public virtual bool OnSlopingGround()
        {
            if (isGrounded && groundAngle > m_slopingGroundAngle)
            {
                if (Physics.Raycast(transform.position, -transform.up, out var hit, height * 2f,
                    Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                    return Vector3.Angle(hit.normal, Vector3.up) > m_slopingGroundAngle;
                else
                    return true;
            }

            return false;
        }


        /// Smoothly moves Lateral Velocity to zero. <summary>
        /// Smoothly moves Lateral Velocity to zero. 角色减速
        /// </summary>
        /// <param name="deceleration"></param>        
        public virtual void Decelerate(float deceleration)
        {
            var delta = deceleration * decelerationMultiplier * Time.deltaTime;
            lateralVelocity = Vector3.MoveTowards(lateralVelocity, Vector3.zero, delta);
        }

        /// Rotate the Player towards to a given direction. <summary>
        /// Rotate the Player towards to a given direction.direction：表示物体应该朝向的方向的向量。
        // degreesPerSecond：物体旋转以面向方向的速度，以每秒度数为单位。
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="degreesPerSecond"></param>        

        public virtual void FaceDirection(Vector3 direction, float degreesPerSecond)
        {
            if (direction != Vector3.zero)
            {
                var rotation = transform.rotation;
                var rotationDelta = degreesPerSecond * Time.deltaTime;
                var target = Quaternion.LookRotation(direction, Vector3.up);
                // Debug.Log(target);
                transform.rotation = Quaternion.RotateTowards(rotation, target, rotationDelta);
            }
        }

        public Vector3 verticalVelocity//垂直速度
        {
            get { return new Vector3(0, velocity.y, 0); }
            set { velocity = new Vector3(velocity.x, value.y, velocity.z); }
        }

        public virtual void Gravity(float gravity)
        {
            if (!isGrounded)
            {
                verticalVelocity += Vector3.down * gravity * gravityMultiplier * Time.deltaTime;
            }
        }

        public virtual void SnapToGround(float force)
        {
            if (isGrounded && (verticalVelocity.y <= 0))
            {
                verticalVelocity = Vector3.down * force;
            }
        }


    }
}