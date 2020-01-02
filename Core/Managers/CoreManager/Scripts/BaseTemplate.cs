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
	// BASE TEMPLATE
	// ===================================================================================
	public abstract class BaseTemplate : ScriptableObject
	{
		
		[Header("[BASE TEMPLATE]")]
		public string title;
		public string description;
		public Sprite image;
		
		// -------------------------------------------------------------------------------
	
	}

	// ===================================================================================
	
}
