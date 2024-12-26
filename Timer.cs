/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 24/11/2009
 * Time: 1:30 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections;
using System.Collections.Generic;

namespace ET2Solver
{
	/// <summary>
	/// Description of Class1.
	/// </summary>
	
	public class TimerChild
	{
		public string id = "";
		public DateTime start_time;
		public DateTime end_time;
		// 10,000,000 ticks per second
		public double elapsed_ticks = 0;
		public double elapsed = 0;
		
		public TimerChild(string id)
		{
			this.id = id;
		}
		
		public void start()
		{
//			this.start_time = DateTime.Now;
		}
		
		public void stop()
		{
//			this.end_time = DateTime.Now;
//			this.elapsed_ticks = this.end_time.Ticks - this.start_time.Ticks;
//			this.elapsed = this.elapsed_ticks / 10000000.0;
		}
		
		public void update()
		{
//			this.elapsed_ticks = DateTime.Now.Ticks - this.start_time.Ticks;
//			this.elapsed = (DateTime.Now.Ticks - this.start_time.Ticks) / 10000000.0;
		}

	}
	
	public class TimerStats
	{
		public string id;
		public Int64 num_iterations;
		public double total_ticks;
		public double total_time;
		
		public TimerStats(string id, int num_iterations)
		{
			this.id = id;
			this.num_iterations = num_iterations;
		}
	}

	public class Timer
	{
		// if enabled will calculate num iterations, total time, avg time for each timer
		public bool calc_stats = false;
		
		public Dictionary<string, TimerChild> timers = new Dictionary<string, TimerChild>();
		public Dictionary<string, TimerStats> stats = new Dictionary<string, TimerStats>();

		public Timer()
		{
		}
		
		public void clear()
		{
			this.timers = new Dictionary<string, TimerChild>();
			this.stats = new Dictionary<string, TimerStats>();
		}

		public void start(string id)
		{
			if ( this.timers.ContainsKey(id) )
			{
				this.timers[id].start();
			}
			else
			{
				TimerChild ctimer = new TimerChild(id);
				ctimer.start();
				this.timers.Add(id, ctimer);
			}
		}
		
		public void stop(string id)
		{
			if ( this.timers.ContainsKey(id) )
			{
				this.timers[id].stop();
				if ( this.calc_stats )
				{
					this.updateStats(id);
				}
			}
		}
		
		public double elapsed(string id)
		{
			if ( this.timers.ContainsKey(id) )
			{
				TimerChild ctimer = this.timers[id];
				if ( ctimer.end_time < ctimer.start_time )
				{
					ctimer.update();
				}
				return ctimer.elapsed;
			}
			else
			{
				return 0d;
			}
		}
		
		public string results(string id)
		{
			string results = "";
			if ( this.timers.ContainsKey(id) )
			{
				TimerChild ctimer = this.timers[id];
				if ( ctimer.end_time < ctimer.start_time )
				{
					ctimer.update();
					results = "timer " + id + ", started @ " + ctimer.start_time.ToString() + ", running for: " + String.Format("{0:0.000}", ctimer.elapsed);
				}
				else
				{
					results = "timer " + id + ", started @ " + ctimer.start_time.ToString() + ", ended @ " + ctimer.end_time.ToString() + ", elapsed: " + String.Format("{0:0.000}", ctimer.elapsed);
				}
			}
			return results;
		}
		
		public void updateStats(string id)
		{
			if ( !this.stats.ContainsKey(id) )
			{
				TimerStats ts = new TimerStats(id, 1);
				this.stats.Add(id, ts);
				this.stats[id].total_ticks = this.timers[id].elapsed_ticks;
				this.stats[id].total_time = this.stats[id].total_ticks / 10000000.0;
			}
			else
			{
				this.stats[id].num_iterations++;
				this.stats[id].total_ticks += this.timers[id].elapsed_ticks;
				this.stats[id].total_time = this.stats[id].total_ticks / 10000000.0;
			}
		}
		
		public string getStats()
		{
			List<string> output = new List<string>();
			foreach ( TimerStats timerstat in this.stats.Values )
			{
				string avg_time = String.Format("{0:0.000000}", timerstat.total_time / timerstat.num_iterations);
				string total_time = String.Format("{0:0.000000}", timerstat.total_time);
				string speed = String.Format("{0:#,#.0}", timerstat.num_iterations / timerstat.total_time );
				output.Add("Timer: " + timerstat.id + ", Num Iterations: " + String.Format("{0:#,#}", timerstat.num_iterations) + ", Total Time: " + total_time + ", Avg Time: " + avg_time.ToString() + ", Speed: " + speed + "/sec");
			}
			return String.Join("\r\n", output.ToArray());
		}
	
	}
}
