// =======================================================================================
//
//
// =======================================================================================
using System;
using UnityEngine;
using UnityEngine.Events;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;
using wovencode;

namespace wovencode
{
	
	// ===================================================================================
	// ENUM
	// ===================================================================================
	public enum DataType { TEXT, INTEGER, REAL }
	
	// ===================================================================================
	// DATABASE DATA
	// ===================================================================================
	[System.Serializable]
	public class DatabaseData
	{
		
		public string name = "Database.sqlite";
		public bool initDatabase 	= false;
		public TableData[] tables;
		
		// -------------------------------------------------------------------------------
		// valid
		// -------------------------------------------------------------------------------
		public bool valid
		{
			get {
				return !string.IsNullOrWhiteSpace(name) &&
						tables.Length > 0;
			}
		}
		
	}
	
	// ===================================================================================
	// TABLE DATA
	// ===================================================================================
	[System.Serializable]
	public class TableData
	{
		
		public string name;
		public bool deleteTable = false;
		public ColumnData[] columns;
		
		// -------------------------------------------------------------------------------
		// valid
		// -------------------------------------------------------------------------------
		public bool valid
		{
			get {
				return !string.IsNullOrWhiteSpace(name) &&
						columns.Length > 0;
			}
		}
		
	}
	
	// ===================================================================================
	// COLUMN DATA
	// ===================================================================================
	[System.Serializable]
	public class ColumnData
	{
		
		public string name;
		public DataType dataType;
		public bool primaryKey = false;
		public bool notNull = true;
		
		// -------------------------------------------------------------------------------
		// valid
		// -------------------------------------------------------------------------------
		public bool valid
		{
			get {
				return !string.IsNullOrWhiteSpace(name);
			}
		}
		
	}
	
	// ===================================================================================
		
}
