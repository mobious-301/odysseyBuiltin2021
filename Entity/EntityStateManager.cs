using System;
using System.Collections.Generic;
using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	public abstract class EntityStateManager : MonoBehaviour
	{
		// public EntityStateManagerEvents events;
	}

	public abstract class EntityStateManager<T> : EntityStateManager where T : Entity<T>
	{
        protected abstract List<EntityState<T>> GetStateList();
    }
}
