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
	public class ItemData
	{
	
		public ItemTemplate template;
		public int amount;
		public int charges;
		public int durability;
		public int level;
		public int cooldown;
		
	}

	// ===================================================================================
	
}
