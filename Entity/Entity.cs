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
			InitializeStateManager();
        }	
        protected virtual void Update()
		{
				HandleStates();
		}
    }
}