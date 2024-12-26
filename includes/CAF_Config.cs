/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 23/10/2010
 * Time: 4:19 AM
 *
 * Configuration file parser
 *
 * ignores comments & blank lines in config files - lines beginning with: # // ;
 * defaults are loaded/saved to config\defaults.txt (allows multiple configuration files to be used)
 * last loaded config file is automatically set to default
 *
 */
using System;
using System.Collections.Generic;

namespace CAF
{
	public class CAF_Config
	{
		private Dictionary<string, string> defaults = new Dictionary<string, string>();
		private List<string> raw_config = new List<string>();
		public string filename = "";
		public Dictionary<string, string> config = new Dictionary<string, string>();
		// on_solve = next_iteration, pause

		public CAF_Config()
		{
		}

		public void setConfiguration(string[] lines)
		{
			// set configuration options loaded in config tab
			this.config.Clear();
			this.raw_config.Clear();
			this.raw_config.AddRange(new List<string>(lines));
			foreach ( string line in lines )
			{
				// ignore blank lines and comments
				if ( line.Trim() != "" && !line.Trim().StartsWith(";") && !line.Trim().StartsWith("#") && !line.Trim().StartsWith("//") )
				{
					string[] parts = line.Trim().Split('=');
					if ( parts.Length == 2 )
					{
						string option = parts[0].Trim();
						string value = parts[1].Trim();
						// override previous value if duplicate entries in config file
						this.setValue(option, value);
					}
				}
			}

		}

		public string getValue(string key)
		{
			if ( this.config.ContainsKey(key) )
			{
				return this.config[key];
			}
			else
			{
				return "";
			}
		}

		public void setValue(string key, string value)
		{
			if ( this.config.ContainsKey(key) )
			{
				this.config[key] = value;
			}
			else
			{
				this.config.Add(key, value);
			}
		}

		public void deleteValue(string key)
		{
			if ( this.config.ContainsKey(key) )
			{
				this.config.Remove(key);
			}
		}

		// attempts to load configuration file in 2 ways
		// 1. from filename as is
		// 2. from config\\filename (strips dirname from filename)
		public void loadConfiguration(string filename)
		{
			System.IO.Path.GetFileName(filename);

			if ( System.IO.File.Exists(filename) )
			{
				string[] lines = System.IO.File.ReadAllLines(filename);
				this.setConfiguration(lines);
				this.filename = filename;
				this.setDefaultValue("config_file", filename);
				this.saveDefaults();
			}
			else
			{
				string filepath = "config\\" + System.IO.Path.GetFileName(filename);
				if ( System.IO.File.Exists(filepath) )
				{
					string[] lines = System.IO.File.ReadAllLines(filepath);
					this.setConfiguration(lines);
				}
				this.filename = filepath;
				this.setDefaultValue("config_file", filepath);
				this.saveDefaults();
			}
		}

		public void saveConfiguration(string filepath)
		{
			System.IO.File.WriteAllLines(filepath, this.raw_config.ToArray());
		}

		public string getAsText()
		{
			return String.Join("\r\n", this.raw_config.ToArray());
		}

		public bool contains(string key)
		{
			return this.config.ContainsKey(key);
		}

		public string getDefaultValue(string key)
		{
			if ( this.defaults.ContainsKey(key) )
			{
				return this.defaults[key];
			}
			else
			{
				return "";
			}
		}

		public void setDefaultValue(string key, string value)
		{
			if ( this.defaults.ContainsKey(key) )
			{
				this.defaults[key] = value;
			}
			else
			{
				this.defaults.Add(key, value);
			}
		}

		public void loadDefaults()
		{
			string filepath = "config\\defaults.txt";
			if ( System.IO.File.Exists(filepath) )
			{
				string[] lines = System.IO.File.ReadAllLines(filepath);
				foreach ( string line in lines )
				{
					// ignore blank lines and comments
					if ( line.Trim() != "" && !line.Trim().StartsWith(";") && !line.Trim().StartsWith("#") && !line.Trim().StartsWith("//") )
					{
						string[] parts = line.Trim().Split('=');
						if ( parts.Length == 2 )
						{
							string option = parts[0].Trim();
							string value = parts[1].Trim();

							switch ( option )
							{
								case "config_file":
									this.loadConfiguration(value);
									break;
							}
						}
					}
				}
			}
		}

		public void saveDefaults()
		{
			string filepath = "config\\defaults.txt";
			System.IO.File.WriteAllText(filepath, this.dict2txt(this.defaults));
		}

		// convert dictionary object to key = value text
		private string dict2txt(Dictionary<string, string> dict)
		{
			List<string> text = new List<string>();
			foreach ( string key in dict.Keys )
			{
				text.Add(String.Format("{0} = {1}", key, dict[key]));
			}
			return String.Join("\r\n", text.ToArray());
		}

        public string imagePath()
        {
            if ( this.contains("imageset") && this.getValue("imageset") == "1" )
            {
                return "images.color";
            }

            return "images";
        }

	}
}
