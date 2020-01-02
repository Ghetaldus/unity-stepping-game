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
	// TRAIT TEMPLATE
	// ===================================================================================
	[CreateAssetMenu(fileName = "New Trait", menuName = "Templates/New Trait", order = 999)]
	public class TraitTemplate : BaseTemplate
	{
		
		
		// -------------------------------------------------------------------------------
		// Caching
		// -------------------------------------------------------------------------------
		static Dictionary<int, TraitTemplate> cache;
		public static Dictionary<int, TraitTemplate> dict
		{
			get
			{
				return cache ?? (cache = Resources.LoadAll<TraitTemplate>("").ToDictionary(
					x => Tools.GetDeterministicHashCode(x.name), x => x)
				);
			}
		}
		
		// -------------------------------------------------------------------------------
	
	}

	// ===================================================================================
	
}
