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
	// ITEM MANAGER
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class ItemManager : BaseManager
	{
		
		
		public static ItemManager 		instance 		= null;
		
		protected List<ItemData> 		itemData = new List<ItemData>();
		
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
		public bool HasItem()
		{
			return false;
		}
		
        // -------------------------------------------------------------------------------
		// 
		// Adds a new item to the inventory, using its default stats and a variable amount
		// -------------------------------------------------------------------------------
		protected void AddItem()
		{
		
		}
		
		// -------------------------------------------------------------------------------
		// 
		// Uses the selected item, applying effects and modifying its stats
		// -------------------------------------------------------------------------------
		protected void UseItem()
		{
		
		}
        
        // -------------------------------------------------------------------------------
		// 
		// Modifies one or more of the stated items stats
		// -------------------------------------------------------------------------------
		protected void ModifyItem()
		{
		
		}
		
		// -------------------------------------------------------------------------------
		// 
		// Removes the stated amount of an item from the inventory
		// -------------------------------------------------------------------------------
		protected void RemoveItem()
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
			
			itemData = new List<ItemData>();
			
			List< List<object> > table = DatabaseManager.instance.ExecuteReader("SELECT name, amount, charges, durability, level, cooldown FROM items WHERE id=@id", new SqliteParameter("@id", Tools.GetUserId));
			
			if (table.Count > 0) 
			{

				foreach (List<object> row in table)
				{
					string n = (string)row[0];
					ItemTemplate template;
					
					if (ItemTemplate.dict.TryGetValue(Tools.GetDeterministicHashCode(n), out template))
					{
						ItemData item 			= new ItemData();
						item.template 			= template;
						item.amount 			= Convert.ToInt32((long)row[1]);
						item.charges 			= Convert.ToInt32((long)row[2]);
						item.durability 		= Convert.ToInt32((long)row[3]);
						item.level 				= Convert.ToInt32((long)row[4]);
						item.cooldown			= Convert.ToInt32((long)row[5]);
						itemData.Add(item);
					}

				}
			
			}
			else
			{
			
				foreach (ItemTemplate template in ItemTemplate.dict.Values)
				{
					if (template.startAmount == 0) continue;
				
					ItemData item 		= new ItemData();
					item.template 		= template;
					item.amount 		= template.startAmount;
					item.charges 		= template.startCharges;
					item.durability 	= template.startDurability;
					item.level 			= template.startLevel;
					itemData.Add(item);
				}
			
			}
			
		}
		
		// -------------------------------------------------------------------------------
		// OnSave
		// -------------------------------------------------------------------------------
		public override void OnSave() {

			DatabaseManager.instance.ExecuteNonQuery("DELETE FROM items WHERE id=@id", new SqliteParameter("@id", Tools.GetUserId));
			
			foreach (ItemData item in itemData)
			{
				DatabaseManager.instance.ExecuteNonQuery("INSERT INTO items VALUES (@id, @name, @amount, @charges, @durability, @level, @cooldown)",
													new SqliteParameter("@id", 			Tools.GetUserId),
													new SqliteParameter("@name", 		item.template.name),
													new SqliteParameter("@amount", 		item.amount),
													new SqliteParameter("@charges", 	item.charges),
													new SqliteParameter("@durability", 	item.durability),
													new SqliteParameter("@level", 		item.level),
													new SqliteParameter("@cooldown", 		item.cooldown)
													);
			}
			
		}
		
		// -------------------------------------------------------------------------------
		
	}
}
