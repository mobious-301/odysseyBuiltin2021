using System;
using System.Collections.Generic;
using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	public abstract class EntityStateManager : MonoBehaviour
	{
		public EntityStateManagerEvents events;
	}

	public abstract class EntityStateManager<T> : EntityStateManager where T : Entity<T>
	{
        protected List<EntityState<T>> m_list = new List<EntityState<T>>();
        protected Dictionary<Type, EntityState<T>> m_states = new Dictionary<Type, EntityState<T>>();
        protected abstract List<EntityState<T>> GetStateList();
        public EntityState<T> current { get; protected set; }//当前状态
        public EntityState<T> last { get; protected set; }//前一个状态
        public T entity { get; protected set; }
        protected virtual void InitializeEntity() => entity = GetComponent<T>();
        public virtual void Step()
		{
            // Debug.Log(entity);
			if (current != null && Time.timeScale > 0)
			{
				current.Step(entity);
			}
		}
        protected virtual void InitializeStates()
		{
			m_list = GetStateList();

			foreach (var state in m_list)
			{
				var type = state.GetType();

				if (!m_states.ContainsKey(type))
				{
					m_states.Add(type, state);
				}
			}

			if (m_list.Count > 0)
			{
				current = m_list[0];
			}
		}
        protected virtual void Start()
		{
            InitializeEntity();
			InitializeStates();
        }

        public virtual void Change<TState>() where TState : EntityState<T>
		{
			var type = typeof(TState);

			if (m_states.ContainsKey(type))
			{
				Change(m_states[type]);
			}
		}

		/// <summary>
		/// Changes to a given Entity State based on its instance.
		/// </summary>
		/// <param name="to">The instance of the Entity State you want to change to.</param>
		public virtual void Change(EntityState<T> to)
		{
			if (to != null && Time.timeScale > 0)
			{
				if (current != null)
				{
					current.Exit(entity);
					events.onExit.Invoke(current.GetType());
					last = current;
				}

				current = to;
				current.Enter(entity);
				events.onEnter.Invoke(current.GetType());
				events.onChange?.Invoke();
			}
		}

		///for animator
		public int index => m_list.IndexOf(current);
		public int lastIndex => m_list.IndexOf(last);







    }
    
}
