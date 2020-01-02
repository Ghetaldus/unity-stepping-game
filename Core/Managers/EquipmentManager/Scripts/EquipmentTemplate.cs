// =======================================================================================
//
//
// =======================================================================================

using System;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using Mono.Data.Sqlite;
using wovencode;

namespace wovencode
{

  	// ===================================================================================
	// EQUIPMENT TEMPLATE
	// ===================================================================================
	[CreateAssetMenu(fileName = "New Equipment", menuName = "Templates/New Equipment", order = 999)]
	public class EquipmentTemplate : BaseTemplate
	{
		
		public int startAmount;
		public int startCharges;
		public int startDurability;
		public int startLevel;
		
		public int defaultAmount;
		public int defaultCharges;
		public int defaultDurability;
		public int defaultLevel;
		
		// -------------------------------------------------------------------------------
		// Caching
		// -------------------------------------------------------------------------------
		static Dictionary<int, EquipmentTemplate> cache;
		public static Dictionary<int, EquipmentTemplate> dict
		{
			get
			{
				return cache ?? (cache = Resources.LoadAll<EquipmentTemplate>("").ToDictionary(
					x => Tools.GetDeterministicHashCode(x.name), x => x)
				);
			}
		}
		
		// -------------------------------------------------------------------------------
	
	}

	// ===================================================================================
	
}
