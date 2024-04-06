using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public abstract class EntityState<T> where T : Entity<T>
{
    public UnityEvent onEnter;
    public UnityEvent onExit;
    public float timeSinceEntered { get; protected set; }

		public void Enter(T entity)
		{
			timeSinceEntered = 0;
			onEnter?.Invoke();
			OnEnter(entity);
		}
        public void Exit(T entity)
		{
			onExit?.Invoke();
			OnExit(entity);
		}

		public void Step(T entity)
		{
			OnStep(entity);
			timeSinceEntered += Time.deltaTime;
		}
        protected abstract void OnEnter(T entity);
        protected abstract void OnExit(T entity);
        protected abstract void OnStep(T entity);
}
