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
	// 
	// ===================================================================================
	[System.Serializable]
	public class CurrencyData
	{
		public CurrencyTemplate template;
		public long amount;
		public long maxAmount;
	}

	// ===================================================================================
	// 
	// ===================================================================================
	[System.Serializable]
	public class CurrencyModifier
	{
		public CurrencyTemplate template;
		public float multiplier;
	}

	// ===================================================================================
	// 
	// ===================================================================================
	[System.Serializable]
	public class CurrencyCondition
	{
		public CurrencyTemplate template;
		public bool blockOnMin;
	}

	// ===================================================================================
	
}
