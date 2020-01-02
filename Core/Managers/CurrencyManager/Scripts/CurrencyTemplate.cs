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
	// CURRENCY TEMPLATE
	// ===================================================================================
	[CreateAssetMenu(fileName = "New Currency", menuName = "Templates/New Currency", order = 999)]
	public class CurrencyTemplate : BaseTemplate
	{
	
		public long startAmount 		= 0;
		public long minAmount			= 0;
		public long maxAmount 			= 100;
		
		[Header("[INCREASE SETTINGS]")]
		public float 					stepMultiplier 		= 1f;
		public CurrencyCondition[] 		increaseConditions;
		
		[Header("[MAX SETTINGS]")]
		public CurrencyModifier[] 		maxModifiers;
		
		[Header("[RESET SETTINGS]")]
		public float 					resetMultiplier 	= 1f;
		public CurrencyModifier[] 		resetModifiers;
		
		[Header("[FLOW SETTINGS]")]
		public CurrencyTemplate 		overflowTemplate;
		public CurrencyTemplate 		underflowTemplate;
		
		// -------------------------------------------------------------------------------
		// Caching
		// -------------------------------------------------------------------------------
		static Dictionary<int, CurrencyTemplate> cache;
		public static Dictionary<int, CurrencyTemplate> dict
		{
			get
			{
				return cache ?? (cache = Resources.LoadAll<CurrencyTemplate>("").ToDictionary(
					x => Tools.GetDeterministicHashCode(x.name), x => x)
				);
			}
		}
		
		// -------------------------------------------------------------------------------
	
	}

	// ===================================================================================
	
}
