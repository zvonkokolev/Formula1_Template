using Formula1.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Utils;

namespace Formula1.Core
{
	/// <summary>
	/// Daten sind in XML-Dateien gespeichert und werden per Linq2XML
	/// in die Collections geladen.
	/// </summary>
	public static class ImportController
	{
		public static List<Race> Races { get; private set; }
		public static List<Driver> Drivers { get; private set; }
		public static List<Team> Teams { get; private set; }
		public static List<Result> Results { get; private set; }

		/// <summary>
		/// Daten der Rennen werden aus der
		/// XML-Datei ausgelesen und in die Races-Collection gespeichert.
		/// Grund: Races werden nicht aus den Results geladen, weil sonst die
		/// Rennen in der Zukunft fehlen
		/// </summary>
		public static IEnumerable<Race> LoadRacesFromRacesXml()
		{
			List<Race> races = new List<Race>();
			string racesPath = MyFile.GetFullNameInApplicationTree("Races.xml");
			var xElement = XDocument.Load(racesPath).Root;
			if(xElement != null)
			{
				races =
					xElement.Elements("Race")
						.Select(race => new Race
						{
							Number = (int)race.Attribute("round"),
							Date = (DateTime)race.Element("Date"),
							Country = race.Element("Circuit")
								?.Element("Location")
								?.Element("Country")
								?.Value,
							City = race.Element("Circuit")
								?.Element("Location")
								?.Element("Locality")
								?.Value
						})
						.ToList();
			}
			return races;
		}

		/// <summary>
		/// Aus den Results werden alle Collections, außer Races gefüllt.
		/// Races wird extra behandelt, um auch Rennen ohne Results zu verwalten
		/// </summary>
		public static IEnumerable<Result> LoadResultsFromXmlIntoCollections()
		{
			Races =  LoadRacesFromRacesXml().ToList();
			Drivers = new List<Driver>();
			Teams = new List<Team>();
			Results = new List<Result>();
			string racePath = MyFile.GetFullNameInApplicationTree("Results.xml");
			var xElement = XDocument.Load(racePath).Root;
			if(xElement != null)
			{
				Results = xElement.Elements("Race").Elements("ResultsList").Elements("Result")
					.Select(result => new Result
					{
						Race = GetRace(result),
						Driver = GetDriver(result),
						Team = GetTeam(result),
						Position = (int)result.Attribute("position"),
						Points = (int)result.Attribute("points")
					}).ToList();
			}
			return Results;
		}

		private static Team GetTeam(XElement result)
		{
			//string teamName = (string)result.Element("Constructor").Attribute("constructorId");
			string teamName = (string)result.Element("Constructor").Element("Name").Value;
			Team team = new Team
			{
				Name = teamName
			};
			if (!Teams.Contains(team))
			{
				Teams.Add(team);
			}
			return Teams.Find(t => t.Name.Equals(teamName));
		}

		private static Driver GetDriver(XElement result)
		{
			//string driverName = (string)result.Element("Driver").Attribute("driverId").Value;
			string driverName = (string)(result.Element("Driver").Element("FamilyName").Value + " " +
				result.Element("Driver").Element("GivenName").Value);
			Driver fahrer = new Driver
			{
				Name = driverName
			};
			if (!Drivers.Contains(fahrer))
				Drivers.Add(fahrer);
			return Drivers.Find(d => d.Name.Equals(driverName));
		}

		private static Race GetRace(XElement result)
		{
			int raceNumber = (int)result.Parent?.Parent?.Attribute("round");
			return Races.Single(race => race.Number == raceNumber);
		}
	}
}