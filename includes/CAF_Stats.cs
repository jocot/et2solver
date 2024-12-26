/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 23/10/2010
 * Time: 2:26 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CAF
{
	public class CAF_Stats
	{
		private Dictionary<string, Int64> stats_data = new Dictionary<string, Int64>();
		
		public CAF_Stats()
		{
		}
		
		public void reset()
		{
			this.stats_data.Clear();
		}
		
		public void inc(string key)
		{
			if ( this.stats_data.ContainsKey(key) )
			{
				this.stats_data[key]++;
			}
			else
			{
				this.stats_data.Add(key, 1);
			}
		}
		
		public void dec(string key)
		{
			if ( this.stats_data.ContainsKey(key) )
			{
				this.stats_data[key]--;
			}
			else
			{
				this.stats_data.Add(key, -1);
			}
		}
		
		public Int64 get(string key)
		{
			if ( this.stats_data.ContainsKey(key) )
			{
				return this.stats_data[key];
			}
			else
			{
				return -1;
			}
		}
		
		public void set(string key, Int64 value)
		{
			if ( this.stats_data.ContainsKey(key) )
			{
				this.stats_data[key] = value;
			}
			else
			{
				this.stats_data.Add(key, value);
			}
		}
		
		public string getAsText()
		{
			List<string> text = new List<string>();
			text.Add("Statistics");
			foreach ( string key in this.stats_data.Keys )
			{
				text.Add(String.Format("{0}: {1}", key, this.stats_data[key]));
			}
			return String.Join("\r\n", text.ToArray());
		}
		
		// delete stats that match regex pattern
		public void del(string pattern)
		{
			List<string> keysToRemove = new List<string>();
			foreach ( string key in this.stats_data.Keys )
			{
				if ( Regex.IsMatch(key, pattern) && !keysToRemove.Contains(key) )
				{
					keysToRemove.Add(key);
				}
			}
			foreach ( string key in keysToRemove )
			{
				this.stats_data.Remove(key);
			}
		}
		
		
	}
}
