using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Formula1.Core.Entities;

namespace Formula1.Core
{
	public class ResultCalculator
	{
		/// <summary>
		/// Berechnet aus den Ergebnissen den aktuellen WM-Stand für die Fahrer
		/// </summary>
		/// <returns>DTO für die Fahrerergebnisse</returns>
		public static IEnumerable<TotalResultDto<Driver>> GetDriverWmTable()
		{
			IEnumerable<TotalResultDto<Driver>> totalResultDtosDrivers;
			var cacheList = ImportController.LoadResultsFromXmlIntoCollections().ToArray();
			totalResultDtosDrivers = cacheList
				.GroupBy(res => res.Driver)
				.OrderByDescending(res => res.Sum(res => res.Points))
				.Select((res, counter) => new TotalResultDto<Driver>()
				{
					Position = ++counter,
					Competitor = res.Key,
					Points = res.Sum(res => res.Points)
				})
				.ToArray();
			return totalResultDtosDrivers;
		}

		/// <summary>
		/// Berechnet aus den Ergebnissen den aktuellen WM-Stand für die Teams
		/// </summary>
		/// <returns>DTO für die Teamergebnisse</returns>
		public static IEnumerable<TotalResultDto<Team>> GetTeamWmTable()
		{
			IEnumerable<TotalResultDto<Team>> totalResultDtosTeams;
			var cacheList = ImportController.LoadResultsFromXmlIntoCollections().ToArray();
			totalResultDtosTeams = cacheList
				.GroupBy(res => res.Team)
				.OrderByDescending(res => res.Sum(res => res.Points))
				.Select((res, counter) => new TotalResultDto<Team>()
				{
					Position = ++counter,
					Competitor = res.Key,
					Points = res.Sum(res => res.Points)
				})
				.ToArray();
			return totalResultDtosTeams;
		}
	}
}



