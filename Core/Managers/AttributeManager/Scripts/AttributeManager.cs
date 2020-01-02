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
	// ATTRIBUTE MANAGER
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class AttributeManager : BaseManager
	{
		
		public static AttributeManager 	instance 		= null;
		
		protected List<AttributeData> 	attributeData = new List<AttributeData>();
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
        void Awake() {
            instance = this;
        }
        
        // -------------------------------------------------------------------------------
		// UpdateAmounts
		// -------------------------------------------------------------------------------
		protected void UpdateAmounts()
		{
		}
		
		// -------------------------------------------------------------------------------
		// UpdateMaximums
		// -------------------------------------------------------------------------------
		protected void UpdateMaximums()
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
		// OnReset
		// -------------------------------------------------------------------------------
		public override void OnReset() {
		}
				
		// -------------------------------------------------------------------------------
		// OnLoad
		// -------------------------------------------------------------------------------
		public override void OnLoad() {
			
			attributeData = new List<AttributeData>();
			
			List< List<object> > table = DatabaseManager.instance.ExecuteReader("SELECT name, amount FROM attributes WHERE id=@id", new SqliteParameter("@id", Tools.GetUserId));
			
			if (table.Count > 0) 
			{

				foreach (List<object> row in table)
				{
					string n = (string)row[0];
					AttributeTemplate template;
					
					if (AttributeTemplate.dict.TryGetValue(Tools.GetDeterministicHashCode(n), out template))
					{
						AttributeData attribute 	= new AttributeData();
						attribute.template 			= template;
						attribute.amount 			= Convert.ToInt32((long)row[1]);
						attributeData.Add(attribute);
					}

				}
			
			}
			else
			{
			
				foreach (AttributeTemplate template in AttributeTemplate.dict.Values)
				{
					AttributeData attribute 		= new AttributeData();
					attribute.template 				= template;
					//attribute.amount 				= template.startAmount;
					attributeData.Add(attribute);
				}
			
			}
			
			UpdateMaximums();
			
		}
		
		// -------------------------------------------------------------------------------
		// OnSave
		// -------------------------------------------------------------------------------
		public override void OnSave() {

			DatabaseManager.instance.ExecuteNonQuery("DELETE FROM attributes WHERE id=@id", new SqliteParameter("@id", Tools.GetUserId));
			
			foreach (AttributeData attribute in attributeData)
			{
				DatabaseManager.instance.ExecuteNonQuery("INSERT INTO attributes VALUES (@id, @name, @amount)",
													new SqliteParameter("@id", 			Tools.GetUserId),
													new SqliteParameter("@name", 		attribute.template.name),
													new SqliteParameter("@amount", 		attribute.amount)
													);
			}
			
		}
		
		// -------------------------------------------------------------------------------
		
	}
}
