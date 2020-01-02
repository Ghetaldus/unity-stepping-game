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
	// DATABASE MANAGER
	// ===================================================================================
	[DisallowMultipleComponent]
	public partial class DatabaseManager : BaseManager
	{
		
		[Header("[OPTIONS]")]
		[Range(1,99999)]
		public float updateInterval	= 5f;
		public bool threadMode 		= false;
		
		[Header("[DATABASE]")]
		public DatabaseData database;
		
		public UnityEvent					loadEvent;
		public UnityEvent					saveEvent;
		public UnityEvent					updateEvent;
		
		public static DatabaseManager 		instance 		= null;
		
		protected static string 			_dbPath 		= "";
		protected SqliteConnection 			_connection 	= null;
		protected SqliteCommand 			_command 		= null;
		protected SqliteDataReader 			_reader 		= null;
		protected string 					_sqlString		= "";

		
		/// <summary>
		/// Awake will initialize the connection.  
		/// RunAsyncInit is just for show.  You can do the normal SQLiteInit to ensure that it is
		/// initialized during the Awake() phase and everything is ready during the Start() phase
		/// </summary>

		// -------------------------------------------------------------------------------
		// Awake
		// -------------------------------------------------------------------------------
		protected void Awake()
		{
			instance = this;
			SetPath();
			InitDatabase();
		}
		
		// -------------------------------------------------------------------------------
		// SetPath
		// -------------------------------------------------------------------------------
		protected void SetPath() {
#if UNITY_EDITOR
        	_dbPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, database.name);
#elif UNITY_ANDROID
        	_dbPath = Path.Combine(Application.persistentDataPath, database.name);
#elif UNITY_IOS
        	_dbPath = Path.Combine(Application.persistentDataPath, database.name);
#else
        	_dbPath = Path.Combine(Application.dataPath, database.name);
#endif
			DebugLog(_dbPath);
			
		}

		// -------------------------------------------------------------------------------
		// OnDestroy
		// -------------------------------------------------------------------------------
		protected void OnDestroy()
		{
			saveEvent.Invoke();
			CloseDatabase();
		}

		// -------------------------------------------------------------------------------
		// 
		// -------------------------------------------------------------------------------
		/// <summary>
		/// Example using the Loom to run an asynchronous method on another thread so SQLite lookups
		/// do not block the main Unity thread
		/// </summary>
		public void RunAsyncInit()
		{
			LoomManager.Loom.QueueOnMainThread(() =>
			{
				InitDatabase();
			});
		}
		
		// ================================= BASIC =======================================
		
		// -------------------------------------------------------------------------------
		// ExecuteNonQuery
		// Execute a query that does not return anything
		// -------------------------------------------------------------------------------
		public void ExecuteNonQuery(string sql, params SqliteParameter[] args)
		{
			using (SqliteCommand command = new SqliteCommand(sql, _connection))
			{
				foreach (SqliteParameter param in args)
					command.Parameters.Add(param);
				command.ExecuteNonQuery();
			}
		}
		
		// -------------------------------------------------------------------------------
		// ExecuteScalar
		// Executes a query that returns a single value
		// -------------------------------------------------------------------------------
		public object ExecuteScalar(string sql, params SqliteParameter[] args)
		{
			using (SqliteCommand command = new SqliteCommand(sql, _connection))
			{
				foreach (SqliteParameter param in args)
					command.Parameters.Add(param);
				return command.ExecuteScalar();
			}
		}

		// -------------------------------------------------------------------------------
		// ExecuteReader
		// Return multiple values from the database
		// -------------------------------------------------------------------------------
		public List< List<object> > ExecuteReader(string sql, params SqliteParameter[] args)
		{
			List< List<object> > result = new List< List<object> >();

			using (SqliteCommand command = new SqliteCommand(sql, _connection))
			{
				foreach (SqliteParameter param in args)
					command.Parameters.Add(param);

				using (SqliteDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						object[] buffer = new object[reader.FieldCount];
						reader.GetValues(buffer);
						result.Add(buffer.ToList());
					}
				}
			}

			return result;
		}
		
		// ================================== INIT =======================================

		// -------------------------------------------------------------------------------
		// InitDatabase
		// -------------------------------------------------------------------------------
		protected void InitDatabase()
		{
			
			if (!database.valid)
			{
				DebugLogError("Database Settings invalid!");
				return;
			}
			
			if (File.Exists(_dbPath) && database.initDatabase) 
			{
				DebugLog("Deleting Database");
				File.Delete(_dbPath);
			}
        	else if (!File.Exists(_dbPath))
        	{
        		DebugLog("Creating Database");
            	SqliteConnection.CreateFile(_dbPath);
			}
			
			DebugLog("Opening SQLite Connection");
			
			_sqlString = "URI=file:" + _dbPath;
			
			_connection = new SqliteConnection(_sqlString);
			_command = _connection.CreateCommand();
			
			_connection.Open();

			// WAL = write ahead logging, very huge speed increase
			_command.CommandText = "PRAGMA journal_mode = WAL;";
			_command.ExecuteNonQuery();

			// journal mode = look it up on google, I don't remember
			_command.CommandText = "PRAGMA journal_mode";
			_reader = _command.ExecuteReader();
			if (_reader.Read())
				DebugLog("WAL value is: " + _reader.GetString(0));
			_reader.Close();

			// more speed increases
			_command.CommandText = "PRAGMA synchronous = OFF";
			_command.ExecuteNonQuery();

			// and some more
			_command.CommandText = "PRAGMA synchronous";
			_reader = _command.ExecuteReader();
			if (_reader.Read())
				DebugLog("synchronous value is: " + _reader.GetInt32(0));
			_reader.Close();
			
			_sqlString = "";
			
			InitTables();

			Invoke("LoadDatabase", 2f);
			
			InvokeRepeating("UpdateDatabase", updateInterval, updateInterval);
			
		}

		// -------------------------------------------------------------------------------
		// InitTables
		// Here we check if each table in the database already exists or not. If not, we
		// create it. We might also delete and re-create one or more tables if required.
		// -------------------------------------------------------------------------------
		protected void InitTables()
		{
		
			foreach (TableData table in database.tables)
			{
			
				if (!table.valid)
				{
					DebugLog("Table settings invalid!");
					continue;
				}
				
				bool deleteTable 	= false;
				bool hasPrimary 	= false;
				
				_command.CommandText = "SELECT name FROM sqlite_master WHERE name='" + table.name + "'";
				_reader = _command.ExecuteReader();
				
				if (!_reader.Read())
				{
					DebugLog("Could not find SQLite table " + table.name);
					deleteTable = true;
				}
				_reader.Close();

				
				if (deleteTable || table.deleteTable)
				{
					
					DeleteTable(table.name);
					
					DebugLog("Creating table: " + table.name);
					
					_sqlString = "CREATE TABLE IF NOT EXISTS " + table.name + " (";
					
					foreach (ColumnData column in table.columns)
					{
						if (!column.valid)
						{
							DebugLog("Column settings invalid!");
							continue;
						}
						
						_sqlString += column.name + " " + column.dataType.ToString();
						
						if (column.notNull)
							_sqlString += " NOT NULL";
						
						if (column.primaryKey && !hasPrimary)
						{
							_sqlString += " PRIMARY KEY";
							hasPrimary = true;
						}
						
						if (!column.Equals(table.columns[table.columns.GetUpperBound(0)] ))
							_sqlString += ", ";
						
					}
					
					_sqlString += ")";
					
					_command.CommandText = _sqlString;
					_command.ExecuteNonQuery();
					
				}
				else
				{
					DebugLog("Found table: " + table.name);
				}
				
			}
			
		}
		
		// -------------------------------------------------------------------------------
		// UpdateDatabase
		// -------------------------------------------------------------------------------
		protected void UpdateDatabase()
		{
			updateEvent.Invoke();
		}

		// -------------------------------------------------------------------------------
		// LoadDatabase
		// -------------------------------------------------------------------------------
		protected void LoadDatabase()
		{
			loadEvent.Invoke();
		}
		
		// ================================ DELETE =======================================
		
		// -------------------------------------------------------------------------------
		// DeleteTable
		// -------------------------------------------------------------------------------
		public void DeleteTable(string sName)
		{
			DebugLog("Dropping table: " + sName);
			_command.CommandText = "DROP TABLE IF EXISTS " + sName;
			_command.ExecuteNonQuery();
		}
		
		// ================================= OTHER =======================================
		
		// -------------------------------------------------------------------------------
		// CloseDatabase
		// -------------------------------------------------------------------------------
		protected void CloseDatabase()
		{
			
			
			
			if (_reader != null && !_reader.IsClosed)
				_reader.Close();
			_reader = null;

			if (_command != null)
				_command.Dispose();
			_command = null;

			if (_connection != null && _connection.State != ConnectionState.Closed)
				_connection.Close();
			_connection = null;
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
		}
		
		// -------------------------------------------------------------------------------
		// OnSave
		// -------------------------------------------------------------------------------
		public override void OnSave() {
		}
		
	}
	
	// ===================================================================================
	
}
