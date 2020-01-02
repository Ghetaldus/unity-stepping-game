
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using wovencode;
using PedometerU;

namespace wovencode
{
	
	// ===================================================================================
	// TOOLS
	// ===================================================================================
	public partial class Tools
	{
	
		// -------------------------------------------------------------------------------
		// ConvertToUnixTimestamp
		// -------------------------------------------------------------------------------
		public static double ConvertToUnixTimestamp(DateTime date)
		{
			DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			TimeSpan diff = date.ToUniversalTime() - origin;
			return Math.Floor(diff.TotalSeconds);
		}

		// -------------------------------------------------------------------------------
		// GetUserId
		// -------------------------------------------------------------------------------
		public static string GetUserId
		{
			get
			{
				return SystemInfo.deviceUniqueIdentifier.ToString();
			}
		}
		
		// -------------------------------------------------------------------------------
		// GetDeterministicHashCode
		// -------------------------------------------------------------------------------
		public static int GetDeterministicHashCode(string str)
		{
			int hash1 = (5381 << 16) + 5381;
			int hash2 = hash1;

			for (int i = 0; i < str.Length; i += 2)
			{
				hash1 = ((hash1 << 5) + hash1) ^ str[i];
				if (i == str.Length - 1)
					break;
				hash2 = ((hash2 << 5) + hash2) ^ str[i + 1];
			}

			return hash1 + (hash2 * 1566083941);
		}

        // -------------------------------------------------------------------------------
		// ConvertToCalories
		// steps
		// -------------------------------------------------------------------------------       
        public static double ConvertToCalories(int nValue, int nSteps, float fCalories) {
        	return (nValue/nSteps) * fCalories;
        }
        
        // -------------------------------------------------------------------------------
		// ConvertToFeet
		// distance
		// -------------------------------------------------------------------------------       
        public static double ConvertToFeet(double dValue) {
        	return dValue * 3.28084;
        }
        
        // -------------------------------------------------------------------------------
		// ConvertToMeters
		// distance
		// -------------------------------------------------------------------------------       
        public static double ConvertToMeters(double dValue) {
        	return ConvertToFeet(dValue) * 0.3048;
        }
        
        // -------------------------------------------------------------------------------
		// ConvertToMiles
		// distance
		// -------------------------------------------------------------------------------       
        public static double ConvertToMiles(double dValue) {
        	return ConvertToFeet(dValue) * 0.000189394;
        }

        // -------------------------------------------------------------------------------
		// ConvertToKilometers
		// distance
		// -------------------------------------------------------------------------------       
        public static double ConvertToKilometers(double dValue) {
        	return ConvertToFeet(dValue) * 0.0003048;
        }

        // -------------------------------------------------------------------------------
    
	}
	
	// ===================================================================================
	
}
