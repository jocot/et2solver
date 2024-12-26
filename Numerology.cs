/*
 * Created by SharpDevelop.
 * User: Joe
 * Date: 15/07/2010
 * Time: 9:40 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ET2Solver
{
	/// <summary>
	/// Description of Numerology.
	/// </summary>
	public static class Numerology
	{
		// calculate the sum of a list of numbers reduced to mod
		// reduce each number to mod and calculate sum
		public static int reduce_sum(int max, params int[] numbers)
		{
			int[] rlist = new int[numbers.Length];
			for ( int i = 0; i < numbers.Length; i++ )
			{
				rlist[i] = num_reduce(max, numbers[i]);
			}
			return rlist.Sum();
		}
		
		// calculate the sum of a list of numbers reduced using num_reduce
		// reduce each number and calculate sum
		public static int mod_sum(int mod, params int[] numbers)
		{
			int[] rlist = new int[numbers.Length];
			for ( int i = 0; i < numbers.Length; i++ )
			{
				rlist[i] = numbers[i] % mod;
			}
			return rlist.Sum();
		}
		
		// calculate numerology value for a string
		// A=1, B=2, Z=26 etc
		public static int num_value(string input)
		{
			int rv = 0;
			string alphaonly = Regex.Replace(input.ToLower(), "[^a-z0-9]", "");
			for ( int i = 0; i < alphaonly.Length; i++ )
			{
				rv += Convert.ToInt16(alphaonly[i]) - 96;
			}
			return rv;
		}
		
		// convert a list of strings to numerology values
		public static int[] num_list(params string[] items)
		{
			int[] rv = new int[items.Length];
			for ( int i = 0; i < items.Length; i++ )
			{
				rv[i] = num_value(items[i]);
			}
			return rv;
		}
		
		// reduce a number, similar to mod but does not return 0 for any number > 0 
		public static int num_reduce(int max, int number)
		{
		  int rv = number % max;
		  if ( number > 0 && rv == 0 )
		  {
		    rv = max;
		  }
		  return rv;
		}
		
		public static bool IsPrime(Int32 number)
		{
			for ( int i = 2; i <= (number / 2); i++ )
			{
				if ( number % i == 0 )
				{
					return false;
				}
			}
			return true;
		}

	}
}
