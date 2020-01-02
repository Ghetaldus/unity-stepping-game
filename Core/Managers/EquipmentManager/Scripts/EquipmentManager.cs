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
	// EQUIPMENT MANAGER
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class EquipmentManager : BaseManager
	{
		
		
		public static EquipmentManager 		instance 		= null;
		
		protected List<EquipmentData> 		equipmentData = new List<EquipmentData>();
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
        void Awake() {
            instance = this;
        }
        
        // ================================= FUNCTIONS ===================================
        
        // -------------------------------------------------------------------------------
		// 
		// Checks if the stated item is available in the stated amount
		// -------------------------------------------------------------------------------
		public bool HasEquipment()
		{
			return false;
		}
		
        // -------------------------------------------------------------------------------
		// 
		// Adds a new item to the inventory, using its default stats and a variable amount
		// -------------------------------------------------------------------------------
		protected void AddEquipment()
		{
		
		}
		
		// -------------------------------------------------------------------------------
		// 
		// Uses the selected item, applying effects and modifying its stats
		// -------------------------------------------------------------------------------
		protected void UseEquipment()
		{
		
		}
        
        // -------------------------------------------------------------------------------
		// 
		// Modifies one or more of the stated items stats
		// -------------------------------------------------------------------------------
		protected void ModifyEquipment()
		{
		
		}
		
		// -------------------------------------------------------------------------------
		// 
		// Removes the stated amount of an item from the inventory
		// -------------------------------------------------------------------------------
		protected void RemoveEquipment()
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
			OnSave();
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
			
			equipmentData = new List<EquipmentData>();
			
			List< List<object> > table = DatabaseManager.instance.ExecuteReader("SELECT name, amount, charges, durability, level, cooldown FROM equipment WHERE id=@id", new SqliteParameter("@id", Tools.GetUserId));
			
			if (table.Count > 0) 
			{

				foreach (List<object> row in table)
				{
					string n = (string)row[0];
					EquipmentTemplate template;
					
					if (EquipmentTemplate.dict.TryGetValue(Tools.GetDeterministicHashCode(n), out template))
					{
						EquipmentData equipment 	= new EquipmentData();
						equipment.template 			= template;
						equipment.amount 			= Convert.ToInt32((long)row[1]);
						equipment.charges 			= Convert.ToInt32((long)row[2]);
						equipment.durability 		= Convert.ToInt32((long)row[3]);
						equipment.level 			= Convert.ToInt32((long)row[4]);
						equipment.cooldown			= Convert.ToInt32((long)row[5]);
						equipmentData.Add(equipment);
					}

				}
			
			}
			else
			{
			
				foreach (EquipmentTemplate template in EquipmentTemplate.dict.Values)
				{
					if (template.startAmount == 0) continue;
				
					EquipmentData equipment 	= new EquipmentData();
					equipment.template 			= template;
					equipment.amount 			= template.startAmount;
					equipment.charges 			= template.startCharges;
					equipment.durability 		= template.startDurability;
					equipment.level 			= template.startLevel;
					equipmentData.Add(equipment);
				}
			
			}
			
		}
		
		// -------------------------------------------------------------------------------
		// OnSave
		// -------------------------------------------------------------------------------
		public override void OnSave() {

			DatabaseManager.instance.ExecuteNonQuery("DELETE FROM equipment WHERE id=@id", new SqliteParameter("@id", Tools.GetUserId));
			
			foreach (EquipmentData equipment in equipmentData)
			{
				DatabaseManager.instance.ExecuteNonQuery("INSERT INTO items VALUES (@id, @name, @amount, @charges, @durability, @level, @cooldown)",
													new SqliteParameter("@id", 				Tools.GetUserId),
													new SqliteParameter("@name", 			equipment.template.name),
													new SqliteParameter("@amount", 			equipment.amount),
													new SqliteParameter("@charges", 		equipment.charges),
													new SqliteParameter("@durability", 		equipment.durability),
													new SqliteParameter("@level", 			equipment.level),
													new SqliteParameter("@cooldown", 		equipment.cooldown)
													);
			}
			
		}
		
		// -------------------------------------------------------------------------------
		
	}
}
