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
	// ITEM TEMPLATE
	// ===================================================================================
	[CreateAssetMenu(fileName = "New Item", menuName = "Templates/New Item", order = 999)]
	public class ItemTemplate : BaseTemplate
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
		static Dictionary<int, ItemTemplate> cache;
		public static Dictionary<int, ItemTemplate> dict
		{
			get
			{
				return cache ?? (cache = Resources.LoadAll<ItemTemplate>("").ToDictionary(
					x => Tools.GetDeterministicHashCode(x.name), x => x)
				);
			}
		}
		
		// -------------------------------------------------------------------------------
	
	}

	// ===================================================================================
	
}
