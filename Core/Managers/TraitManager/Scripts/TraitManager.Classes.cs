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
	public class TraitData
	{
	
		public TraitTemplate template;
		public long amount;
		public long maxAmount;
		
	}

	// ===================================================================================
	// 
	// ===================================================================================
	[System.Serializable]
	public class TraitModifier
	{
	
		public TraitTemplate template;
		public float multiplier;
		
	}



	// ===================================================================================
	
}
