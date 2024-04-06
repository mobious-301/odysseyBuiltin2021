using UnityEngine;

namespace PLAYERTWO.PlatformerProject
{
	public abstract class EntityStatsManager<T> : MonoBehaviour where T : EntityStats<T>
	{
		public T[] stats;
    }
}