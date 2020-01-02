// =======================================================================================
//
//
// =======================================================================================

using System;
using UnityEngine;
using UnityEngine.Events;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using wovencode;

namespace wovencode
{
	
	// ===================================================================================
	// TRAIT MANAGER
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class TraitManager : BaseManager
	{
		
		public static TraitManager 	instance 		= null;
		
		protected List<TraitData> 	traitData = new List<TraitData>();
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
        void Awake() {
            instance = this;
        }
        
  		// ================================= FUNCTIONS ===================================
        
        // -------------------------------------------------------------------------------
		// 
		// 
		// -------------------------------------------------------------------------------
		public bool HasTrait()
		{
			return false;
		}
		
        // -------------------------------------------------------------------------------
		// 
		// 
		// -------------------------------------------------------------------------------
		protected void AddTrait()
		{
		
		}
		
		// -------------------------------------------------------------------------------
		// 
		// 
		// -------------------------------------------------------------------------------
		protected void RemoveTrait()
		{
		
		}
		
		// =================================== EVENTS ====================================
		
		// -------------------------------------------------------------------------------
		// OnChanged
		// -------------------------------------------------------------------------------
		public override void OnChanged() {
		}
		
		// -------------------------------------------------------------------------------
		// OnUpdate
		// -------------------------------------------------------------------------------
		public override void OnUpdate() {
		}
		
		// -------------------------------------------------------------------------------
		// OnRefresh
		// -------------------------------------------------------------------------------
		public override void OnRefresh() {
		}
		
		// -------------------------------------------------------------------------------
		// OnReset
		// -------------------------------------------------------------------------------
		public override void OnReset() {
		}
				
		// -------------------------------------------------------------------------------
		// OnLoad
		// -------------------------------------------------------------------------------
		public override void OnLoad() {
			
			traitData = new List<TraitData>();
			
			List< List<object> > table = DatabaseManager.instance.ExecuteReader("SELECT name FROM traits WHERE id=@id", new SqliteParameter("@id", Tools.GetUserId));
			
			if (table.Count > 0) 
			{

				foreach (List<object> row in table)
				{
					string n = (string)row[0];
					TraitTemplate template;
					
					if (TraitTemplate.dict.TryGetValue(Tools.GetDeterministicHashCode(n), out template))
					{
						TraitData trait 		= new TraitData();
						trait.template 			= template;
						traitData.Add(trait);
					}

				}
			
			}
			else
			{
			
				foreach (TraitTemplate template in TraitTemplate.dict.Values)
				{
					TraitData trait 	= new TraitData();
					trait.template 		= template;
					traitData.Add(trait);
				}
			
			}
			

			
		}
		
		// -------------------------------------------------------------------------------
		// OnSave
		// -------------------------------------------------------------------------------
		public override void OnSave() {

			DatabaseManager.instance.ExecuteNonQuery("DELETE FROM traits WHERE id=@id", new SqliteParameter("@id", Tools.GetUserId));
			
			foreach (TraitData trait in traitData)
			{
				DatabaseManager.instance.ExecuteNonQuery("INSERT INTO traits VALUES (@id, @name)",
													new SqliteParameter("@id", 			Tools.GetUserId),
													new SqliteParameter("@name", 		trait.template.name)
													);
			}
			
		}
		
		// -------------------------------------------------------------------------------
		
	}
}
