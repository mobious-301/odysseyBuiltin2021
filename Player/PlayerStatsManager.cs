using UnityEngine;
using System.Collections.Generic;

namespace PLAYERTWO.PlatformerProject
{
	[AddComponentMenu("PLAYER TWO/Platformer Project/Player/Player Stats Manager")]
	public class PlayerStatsManager : EntityStatsManager<PlayerStats> {
        // string a=PlayerStatsEnum.AirDivePlayerState.ToString();
        // public string[] states;

		// protected override List<EntityState<Player>> GetStateList()
		// {
		// 	return PlayerState.CreateListFromStringArray(states);
		// }
    }
}
