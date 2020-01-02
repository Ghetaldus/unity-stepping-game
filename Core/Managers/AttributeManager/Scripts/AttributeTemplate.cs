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
	// ATTRIBUTE TEMPLATE
	// ===================================================================================
	[CreateAssetMenu(fileName = "New Attribute", menuName = "Templates/New Attribute", order = 999)]
	public class AttributeTemplate : BaseTemplate
	{
	
		
		
		
		// -------------------------------------------------------------------------------
		// Caching
		// -------------------------------------------------------------------------------
		static Dictionary<int, AttributeTemplate> cache;
		public static Dictionary<int, AttributeTemplate> dict
		{
			get
			{
				return cache ?? (cache = Resources.LoadAll<AttributeTemplate>("").ToDictionary(
					x => Tools.GetDeterministicHashCode(x.name), x => x)
				);
			}
		}
		
		// -------------------------------------------------------------------------------
	
	}

	// ===================================================================================
	
}
