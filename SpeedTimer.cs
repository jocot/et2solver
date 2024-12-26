/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 14/06/2010
 * Time: 9:31 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;

namespace ET2Solver
{
	/// <summary>
	/// Description of SpeedTimer.
	/// </summary>
	public class SpeedTimer_Data
	{
		// timer variables
		public string id = "";
		public bool running = false;
		public int numIterations = 0;

		public int duration = 1;
		public int target_iterations = 0;
		public double target_time = 0;

		public double start_time = 0;
		public double end_time = 0;
		public double elapsed = 0;
		public double speed = 0.0;

		public SpeedTimer_Data()
		{
		}
	}
	
	public class SpeedTimer
	{
		public Dictionary<string, SpeedTimer_Data> timers = new Dictionary<string, SpeedTimer_Data>();

		public SpeedTimer()
		{
		}

		public void start(string id)
		{
			if ( this.timers.ContainsKey(id) )
			{
				this.timers[id].start_time = DateTime.Now.Ticks;
				this.timers[id].running = true;
				this.timers[id].target_time = this.timers[id].start_time + (this.timers[id].duration * 10000000.0);
			}
			else
			{
				SpeedTimer_Data timer = new SpeedTimer_Data();
				timer.id = id;
				timer.start_time = DateTime.Now.Ticks;
				timer.target_time = timer.start_time + (timer.duration * 10000000.0);
				timer.end_time = 0;
				timer.elapsed = 0;
				timer.speed = 0.0;
				timer.numIterations = 0;
				timer.running = true;
				this.timers.Add(id, timer);
			}
		}
		
		public void stop(string id)
		{
			if ( this.timers[id].running )
			{
				this.timers[id].numIterations++;
				this.timers[id].end_time = DateTime.Now.Ticks;
				this.timers[id].elapsed += (this.timers[id].end_time - this.timers[id].start_time);
				this.timers[id].running = false;
			}
		}
		
		public void iterate(string id)
		{
			this.timers[id].numIterations++;
		}
		
		public double timeElapsed(string id)
		{
			this.stop(id);
			return this.timers[id].elapsed / 10000000.0;
		}
		
		public bool durationReached(string id)
		{
			return DateTime.Now.Ticks >= this.timers[id].target_time;
		}
		
		public bool iterationsReached(string id)
		{
			return this.timers[id].numIterations >= this.timers[id].target_iterations;
		}
		
		public string calcSpeed(string id)
		{
			this.timers[id].speed = this.timers[id].numIterations / this.timeElapsed(id);
			return String.Format("{0} - {1:#,#} iterations in {2:0.000} second(s) @ {3:#,#.0} it/sec", id, this.timers[id].numIterations, this.timeElapsed(id), this.timers[id].speed);
		}
		
		public string getStats()
		{
			List<string> stats = new List<string>();
			foreach ( string id in this.timers.Keys )
			{
				stats.Add(this.calcSpeed(id));
			}
			return String.Join("\r\n", stats.ToArray());
		}

	}
}
