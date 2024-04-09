using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
            HandleStates();
            HandleController();
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
        public bool isGrounded { get; protected set; } = true;
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
            Debug.Log(direction);
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
        protected readonly float m_slopingGroundAngle = 20f;
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


    }
}