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
	public class AttributeData
	{
	
		public AttributeTemplate template;
		public long amount;
		public long maxAmount;
		
	}

	// ===================================================================================
	// 
	// ===================================================================================
	[System.Serializable]
	public class AttributeModifier
	{
	
		public AttributeTemplate template;
		public float multiplier;
		
	}



	// ===================================================================================
	
}
