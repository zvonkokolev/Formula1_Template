﻿using System;
using System.Linq;
using Formula1.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Formula1.CoreTest
{
	[TestClass()]
	public class ImportControllerTests
	{
		/// <summary>
		/// Als erste Übung die Rennen aus der XML-Datei parsen
		/// </summary>
		[TestMethod()]
		public void T01_LoadRacesFromRacesXmlTest()
		{
			var races = ImportController.LoadRacesFromRacesXml().ToList();
			Assert.AreEqual(20, races.Count);
			Assert.AreEqual("Melbourne", races.First().City);
			Assert.AreEqual(1, races.First().Number);
			Assert.AreEqual("Abu Dhabi", races.Last().City);
			Assert.AreEqual(20, races.Last().Number);
		}

		/// <summary>
		/// Alle Results in Collections laden.
		/// </summary>
		[TestMethod()]
		public void T02_LoadResultsFromResultsXmlTest()
		{
			var results = ImportController.LoadResultsFromXmlIntoCollections().ToList();
			Assert.AreEqual(10, results.GroupBy(res => res.Team).Count());
			Assert.AreEqual(24, results.GroupBy(res => res.Driver).Count());
			Assert.AreEqual(300, results.Count);
		}

		/// <summary>
		/// Results von Verstappen
		/// </summary>
		[TestMethod()]
		public void T03_LoadVerstappenResults()
		{
			// Lade Verstappens Platzierungen in ein anonymes Objekt { City, Position }
			// Sortiert nach der Rennnummer
			var results = ImportController.LoadResultsFromXmlIntoCollections().ToArray();
			var endResults = results
				.Where(res => res.Driver.Name.Equals("Verstappen Max"))
				.OrderBy(res => res.Race.Number)
				.ToList();

			Assert.AreEqual(15, endResults.Count);

			Assert.AreEqual(5, endResults[0].Position);
			Assert.AreEqual("Melbourne", endResults[0].Race.City);

			Assert.AreEqual(1, endResults[14].Position);
			Assert.AreEqual("Kuala Lumpur", endResults[14].Race.City);
		}
	}
}