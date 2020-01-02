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
	// CURRENCY MANAGER
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class CurrencyManager : BaseManager
	{
		
		public static CurrencyManager 	instance 		= null;
		
		protected List<CurrencyData> 	currencyData = new List<CurrencyData>();
		
		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
        void Awake() {
            instance = this;
        }
        
        // ================================ FUNCTIONS ====================================
        
  		// -------------------------------------------------------------------------------
		// CheckConditions
		// Checks if a currency can be modified based on the state of other currencies
		// -------------------------------------------------------------------------------
        protected bool CheckConditions(CurrencyData currency)
        {
        	
        	if (currency.template == null || currency.template.increaseConditions == null) return false;
        	
        	foreach (CurrencyCondition condition in currency.template.increaseConditions)
        	{
        	
        		if (condition.template != null && condition.blockOnMin)
        		{
        		
        			CurrencyData data = currencyData.FirstOrDefault(x => x.template == condition.template);
        			
        			if (data.amount <= data.template.minAmount)
        				return false;

        		}
        	
        	}
        	
        	return true;
        	
        }
        
        // -------------------------------------------------------------------------------
		// ResetAmounts
		// Adjusts the currency amounts when the session is reset
		// -------------------------------------------------------------------------------
		protected void ResetAmounts()
		{
      
        	foreach (CurrencyData currency in currencyData)
			{
				
				if (currency.template == null) continue;
				
				currency.maxAmount = Convert.ToInt32(currency.template.maxAmount * currency.template.resetMultiplier);
				
				if (currency.template.maxModifiers == null) continue;
				
				long m = currency.maxAmount;
				
				foreach (CurrencyModifier modifier in currency.template.resetModifiers)
				{
					if (modifier.template == null) continue;
					m += Convert.ToInt32(currencyData.FirstOrDefault(x => x.template == modifier.template).amount * modifier.multiplier);
				}
				
				currency.amount = m;

			}
        
        }
        
        // -------------------------------------------------------------------------------
		// UpdateAmounts
		// Adjusts the currency amounts when new steps are detected
		// -------------------------------------------------------------------------------
		protected void UpdateAmounts()
		{
			
			int s = StepManager.instance.GetSteps();
			
			foreach (CurrencyData currency in currencyData)
			{
				
				if (!CheckConditions(currency)) continue;
				
				long v = Convert.ToInt32(s * currency.template.stepMultiplier);
				long n = currency.amount + v;
				
				currency.amount = n;
				
				if (currency.amount < currency.template.minAmount)
					currency.amount = currency.template.minAmount;
				else if (currency.amount > currency.maxAmount)
					currency.amount = currency.maxAmount;
					
				if (n < currency.template.minAmount && currency.template.underflowTemplate != null)
					currencyData.FirstOrDefault(x => x.template == currency.template.underflowTemplate).amount += (n - currency.template.minAmount);
				else if (n > currency.maxAmount && currency.template.overflowTemplate != null)
					currencyData.FirstOrDefault(x => x.template == currency.template.overflowTemplate).amount += (n - currency.maxAmount);
				
			}
			
			UpdateMaximums();
			
		}
		
		// -------------------------------------------------------------------------------
		// UpdateMaximums
		// Sets the maximum of each currency and adjusts it depending on other currencies
		// -------------------------------------------------------------------------------
		protected void UpdateMaximums()
		{
		
			foreach (CurrencyData currency in currencyData)
			{
				
				if (currency.template == null) continue;
				
				currency.maxAmount = currency.template.maxAmount;
				
				if (currency.template.maxModifiers == null) continue;
				
				long m = currency.maxAmount;
				
				foreach (CurrencyModifier modifier in currency.template.maxModifiers)
				{
					if (modifier.template == null) continue;
					m += Convert.ToInt32(currencyData.FirstOrDefault(x => x.template == modifier.template).amount * modifier.multiplier);
				}
				
				currency.maxAmount = m;
		
			}
			
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
		// OnReset
		// -------------------------------------------------------------------------------
		public override void OnReset() {
			
		}

		// -------------------------------------------------------------------------------
		// OnLoad
		// -------------------------------------------------------------------------------
		public override void OnLoad() {
			
			currencyData = new List<CurrencyData>();
			
			List< List<object> > table = DatabaseManager.instance.ExecuteReader("SELECT name, amount FROM currencies WHERE id=@id", new SqliteParameter("@id", Tools.GetUserId));
			
			if (table.Count > 0) 
			{

				foreach (List<object> row in table)
				{
					string n = (string)row[0];
					CurrencyTemplate template;
					
					if (CurrencyTemplate.dict.TryGetValue(Tools.GetDeterministicHashCode(n), out template))
					{
						CurrencyData currency 	= new CurrencyData();
						currency.template = template;
						currency.amount = Convert.ToInt32((long)row[1]);
						currencyData.Add(currency);
					}

				}
			
			}
			else
			{
			
				foreach (CurrencyTemplate template in CurrencyTemplate.dict.Values)
				{
					CurrencyData currency 	= new CurrencyData();
					currency.template = template;
					currency.amount = template.startAmount;
					currencyData.Add(currency);
				}
			
			}
			
			UpdateMaximums();
			
		}
		
		// -------------------------------------------------------------------------------
		// OnSave
		// -------------------------------------------------------------------------------
		public override void OnSave() {

			DatabaseManager.instance.ExecuteNonQuery("DELETE FROM currencies WHERE id=@id", new SqliteParameter("@id", Tools.GetUserId));
			
			foreach (CurrencyData currency in currencyData)
			{
				DatabaseManager.instance.ExecuteNonQuery("INSERT INTO currencies VALUES (@id, @name, @amount)",
													new SqliteParameter("@id", 			Tools.GetUserId),
													new SqliteParameter("@name", 		currency.template.name),
													new SqliteParameter("@amount", 		currency.amount)
													);
			}
			
		}
		
		// -------------------------------------------------------------------------------
		
	}
}
