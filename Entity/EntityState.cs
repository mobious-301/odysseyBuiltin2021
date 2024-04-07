using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace PLAYERTWO.PlatformerProject
{
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
        public static EntityState<T> CreateFromString(string typeName)
		{
			return (EntityState<T>)System.Activator
				.CreateInstance(System.Type.GetType(typeName));
		}


        public static List<EntityState<T>> CreateListFromStringArray(string[] array)
		{
			var list = new List<EntityState<T>>();

			foreach (var typeName in array)
			{
                // Debug.Log(System.Type.GetType("PLAYERTWO.PlatformerProject."+typeName));
                if(System.Type.GetType("PLAYERTWO.PlatformerProject."+typeName)!=null)
            {
				list.Add(CreateFromString("PLAYERTWO.PlatformerProject."+typeName));}
			}

			return list;
		}
}
}