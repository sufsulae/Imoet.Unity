using System;
using System.Collections.Generic;
using Vec3 = UnityEngine.Vector3;
using Vec2 = UnityEngine.Vector2;

//Random Object Generator based on Probability Score
//Author : Yusuf Sulaeman (sufsulae@gmail.com)

namespace Imoet.Unity.Utility {
	/// <summary>
	/// Utillity to Generate Random thing and sh*t
	/// </summary>
	public static class RandomX
	{
		//--- Private Member ---
		private static Random m_random;

		//--- Static Class Contructor ---
		static RandomX() {
			m_random = new Random();
		}

		//--- Public Static Method ---
		/// <summary>
		/// Get Random Object based on probability map
		/// </summary>
		/// <typeparam name="T">The Object</typeparam>
		/// <param name="minProbScore">Minimum expected probability value</param>
		/// <param name="maxProbScore">Minimum expected probability value</param>
		/// <param name="probabilityMap">The Probability Map</param>
		/// <returns>Of Course The Object</returns>
		public static T GetRandomBetween<T>(float minProbScore, float maxProbScore, ProbItem<T>[] probabilityMap)
		{
			var list = new List<ProbItem<T>>(probabilityMap);
			list.Sort((a, b) => { return a.score.CompareTo(b.score); });
			return _getRndObj2<T>(minProbScore, maxProbScore, list);
		}
		/// <summary>
		/// Get Random Object based on probability map
		/// </summary>
		/// <typeparam name="T">The Object</typeparam>
		/// <param name="probabilityMap">The Probability Map</param>
		/// <returns>The Object</returns>
		public static T GetRandomBetween<T>(ProbItem<T>[] probabilityMap) {
			return GetRandomBetween(0, GetTotalProbabilityScore(probabilityMap), probabilityMap);
		}
		/// <summary>
		/// Generate Random Number between expected values. 
		/// Just In case if you need it, but dont delete it... AING BUTUH...
		/// </summary>
		public static float GetRandomBetween(float min, float max)
		{
			return min + (max - min) * (float)m_random.NextDouble();
		}
		/// <summary>
		/// Generate Random Number between expected values. 
		/// Just In case if you need it, but dont delete it... AING BUTUH...
		/// </summary>
		public static int GetRandomBetween(int min, int max)
		{
			return (int)(min + (max - min) * (float)m_random.NextDouble());
		}
		/// <summary>
		/// Generate Random Number between expected values. 
		/// Just In case if you need it, but dont delete it... AING BUTUH...
		/// </summary>
		public static double GetRandomBetween(double min, double max)
		{
			return min + (max - min) * m_random.NextDouble();
		}

		public static Vec3 GetRandomBetween(Vec3 min, Vec3 max) {
			return new Vec3(
				GetRandomBetween(min.x, max.x),
				GetRandomBetween(min.y, max.y),
				GetRandomBetween(min.z, max.z)
			);
		}

		public static Vec2 GetRandomBetween(Vec2 min, Vec2 max)
		{
			return new Vec2(
				GetRandomBetween(min.x, max.x),
				GetRandomBetween(min.y, max.y)
			);
		}
		/// <summary>
		/// Get Sum of all probability Score
		/// </summary>
		/// <typeparam name="T">The ObJect</typeparam>
		/// <param name="probMap">Probability Map</param>
		public static float GetTotalProbabilityScore<T>(IEnumerable<ProbItem<T>> probMap)
		{
			var res = 0.0f;
			foreach (var i in probMap)
				res += i.score;
			return res;
		}

		/// <summary>
		/// Get Min Probability Score
		/// </summary>
		/// <typeparam name="T">The ObJect</typeparam>
		/// <param name="probMap">Probability Map</param>
		public static float GetMinProbabilityScore<T>(IEnumerable<ProbItem<T>> probMap) {
			var res = 0.0f;
			foreach (var i in probMap) {
				if (res == 0.0f || i.score <= res)
					res = i.score;
			}
			return res;
		}

		/// <summary>
		/// Get Max Probability Score
		/// </summary>
		/// <typeparam name="T">The ObJect</typeparam>
		/// <param name="probMap">Probability Map</param>
		public static float GetMaxProbabilityScore<T>(IEnumerable<ProbItem<T>> probMap) {
			var res = 0.0f;
			foreach (var i in probMap)
			{
				if (res == 0.0f || i.score >= res)
					res = i.score;
			}
			return res;
		}


		//--- Private Static Method ---
		//THE MACHINE !!!! ....
		private static T _getRndObj2<T>(float minR, float maxR, List<ProbItem<T>> sortedProbMap)
		{
			if (sortedProbMap == null || sortedProbMap.Count == 0)
				throw new NullReferenceException("probabilityMap is Empty");

			//Pastikan bahwa parameter "minR" dan "maxR" diatas  
			//dapat mencakup semua angka percentase di dalam "probabilityMap".
			//Karena jika tidak, maka proses didalam fungsi ini tidak akan pernah berhenti.
			var probabilitySum = GetTotalProbabilityScore(sortedProbMap);

			//Check apakah besar angka minR atau maxR lebih kecil dari total probability score
			if ((decimal)minR > (decimal)probabilitySum || (decimal)maxR > (decimal)probabilitySum) {
				maxR = probabilitySum;
				//throw new ArgumentException("Minimum or Maximum value range doesn't fall between expected values (min (" + minR + ") | max (" + maxR + ") > " + probabilitySum + ")");
			}
				

			var probMapLen = sortedProbMap.Count;

			//EXECUTE IT
			while (true)
			{
				var num = 0.0f;
				var prob = GetRandomBetween(minR, maxR);
				for (int i = 0; i < probMapLen; i++)
				{
					if (prob >= num && prob <= num + sortedProbMap[i].score && prob <= maxR) {
						//if(sortedProbMap[i].obj is PowerUpKind)
						//    UnityEngine.Debug.Log("Power Up Probability: " + prob + "\nObj: " + sortedProbMap[i].obj);
						//if (sortedProbMap[i].obj is PlatformKind)
						//    UnityEngine.Debug.Log("Platform Probabilty: " + prob + "\nObj: " + sortedProbMap[i].obj);
						return sortedProbMap[i].obj;
					}
					num += sortedProbMap[i].score;
				}
			}
		}

		/// <summary>
		/// Class Buat nyimpen Data Object beserta angka probability
		/// </summary>
		/// <typeparam name="T">The Object, yes you can assign everything to it</typeparam>
		[Serializable]
		public class ProbItem<T>
		{
			/// <summary>
			/// Object Value
			/// </summary>
			public T obj { get; set; }
			/// <summary>
			/// Probability Score
			/// </summary>
			public float score { get; set; }

			/// <summary>
			/// Create a new Item with object and probability value
			/// </summary>
			/// <param name="objValue">Object atau Data yang mau Disimpen</param>
			/// <param name="probScore">Angka Probability nya</param>
			public ProbItem(T objValue, float probScore)
			{
				this.obj = objValue;
				this.score = probScore;
			}
		}
	}
}
