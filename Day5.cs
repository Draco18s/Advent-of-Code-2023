using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventofCode2023 {
	internal static class Day5 {
		enum ReadState
		{
			seeds,
			seedtosoilmap,
			soiltofertilizermap,
			fertilizertowatermap,
			watertolightmap,
			lighttotemperaturemap,
			temperaturetohumiditymap,
			humiditytolocationmap
		}
		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;
			Dictionary<(long,long), long> seedToSoil = new Dictionary<(long, long), long>();
			Dictionary<(long, long), long> soilToFertilizer = new Dictionary<(long, long), long>();
			Dictionary<(long, long), long> fertilizerToWater = new Dictionary<(long, long), long>();
			Dictionary<(long, long), long> waterToLight = new Dictionary<(long, long), long>();
			Dictionary<(long, long), long> lightToTemperature = new Dictionary<(long, long), long>();
			Dictionary<(long, long), long> temperatureToHumidity = new Dictionary<(long, long), long>();
			Dictionary<(long, long), long> humidityToLocation = new Dictionary<(long, long), long>();

			Dictionary<ReadState, Dictionary<(long, long), long>> dictMap = new Dictionary<ReadState, Dictionary<(long, long), long>> {
				{ ReadState.seedtosoilmap, seedToSoil },
				{ ReadState.soiltofertilizermap, soilToFertilizer },
				{ ReadState.fertilizertowatermap, fertilizerToWater },
				{ ReadState.watertolightmap, waterToLight },
				{ ReadState.lighttotemperaturemap, lightToTemperature },
				{ ReadState.temperaturetohumiditymap, temperatureToHumidity },
				{ ReadState.humiditytolocationmap, humidityToLocation },

			};


			Dictionary<(long, long), long> activeDict;

			List<long> seeds = new List<long>();

			ReadState state = ReadState.seeds;

			foreach (string line in lines)
			{
				if(string.IsNullOrWhiteSpace(line)) continue;
				string ln = line;
				if (line.StartsWith("seeds:"))
				{
					state = ReadState.seeds;
					ln = ln.Replace("seeds: ", "");
				}
				if (line.Contains("map:"))
				{
					string l = line.Replace("-", "").Replace(" ","").Replace(":", "");
					state = (ReadState)Enum.Parse(typeof(ReadState), l);
					continue;
				}

				string[] values = ln.Split(' ');
				switch (state)
				{
					case ReadState.seeds:
						seeds.AddRange(values.Select(long.Parse));
						break;
					default:
						activeDict = dictMap[state];
						var vals = values.Select(long.Parse).ToArray();
						long destStart = vals[0];
						long sourceStart = vals[1];
						long rangeSize = vals[2];

						activeDict.Add((sourceStart, rangeSize), destStart);

						break;
				}
			}

			result = long.MaxValue;
			foreach (long s in seeds)
			{
				result = Math.Min(GetLocationForSeed(s, dictMap), result);
			}
			return result;
		}

		private static long GetLocationForSeed(long seed, Dictionary<ReadState, Dictionary<(long, long), long>> dictMap)
		{
			long val = -1;
			long key = seed;
			foreach(ReadState map in Enum.GetValues<ReadState>())
			{
				if(map == ReadState.seeds) continue;
				if (dictMap[map].Any(p =>
				    {
					    return p.Key.Item1 <= key && p.Key.Item1 + p.Key.Item2 > key;
				    }))
				{
					KeyValuePair<(long, long), long> range = dictMap[map].FirstOrDefault(p => p.Key.Item1 <= key && p.Key.Item1 + p.Key.Item2 > key);
					val = key - range.Key.Item1 + range.Value;
				}
				else
				{
					val = key;
				}

				key = val;
			}
			return val;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			long result = 0l;
			Dictionary<(long, long), long> seedToSoil = new Dictionary<(long, long), long>();
			Dictionary<(long, long), long> soilToFertilizer = new Dictionary<(long, long), long>();
			Dictionary<(long, long), long> fertilizerToWater = new Dictionary<(long, long), long>();
			Dictionary<(long, long), long> waterToLight = new Dictionary<(long, long), long>();
			Dictionary<(long, long), long> lightToTemperature = new Dictionary<(long, long), long>();
			Dictionary<(long, long), long> temperatureToHumidity = new Dictionary<(long, long), long>();
			Dictionary<(long, long), long> humidityToLocation = new Dictionary<(long, long), long>();

			Dictionary<ReadState, Dictionary<(long, long), long>> dictMap = new Dictionary<ReadState, Dictionary<(long, long), long>> {
				{ ReadState.seedtosoilmap, seedToSoil },
				{ ReadState.soiltofertilizermap, soilToFertilizer },
				{ ReadState.fertilizertowatermap, fertilizerToWater },
				{ ReadState.watertolightmap, waterToLight },
				{ ReadState.lighttotemperaturemap, lightToTemperature },
				{ ReadState.temperaturetohumiditymap, temperatureToHumidity },
				{ ReadState.humiditytolocationmap, humidityToLocation },

			};


			Dictionary<(long, long), long> activeDict;

			List<(long min,long max)> seeds = new List<(long,long)>();

			ReadState state = ReadState.seeds;

			foreach (string line in lines)
			{
				if (string.IsNullOrWhiteSpace(line)) continue;
				string ln = line;
				if (line.StartsWith("seeds:"))
				{
					state = ReadState.seeds;
					ln = ln.Replace("seeds: ", "");
				}
				if (line.Contains("map:"))
				{
					string l = line.Replace("-", "").Replace(" ", "").Replace(":", "");
					state = (ReadState)Enum.Parse(typeof(ReadState), l);
					continue;
				}

				string[] values = ln.Split(' ');
				var vals = values.Select(long.Parse).ToArray();
				switch (state)
				{
					case ReadState.seeds:
						for (int i = 0; i < values.Length; i += 2)
						{
							seeds.Add((vals[i], vals[i]+ vals[i+1]));
						}
						break;
					default:
						activeDict = dictMap[state];
						long destStart = vals[0];
						long sourceStart = vals[1];
						long rangeSize = vals[2];

						activeDict.Add((sourceStart, rangeSize), destStart);

						break;
				}
			}

			result = long.MaxValue;
			foreach ((long,long) s in seeds)
			{
				result = Math.Min(GetLocationForSeed(s, dictMap).Min(p => p.Item1), result);
			}
			return result;
		}

		private static List<(long,long)> GetLocationForSeed((long,long) seed, Dictionary<ReadState, Dictionary<(long, long), long>> dictMap)
		{
			List<(long, long)> inputList = new List<(long, long)> { seed };
			//List<(long, long)> returnList = new List<(long, long)>();
			List<(long, long)> workingList = new List<(long, long)>();

			(long min, long max) val = (-1,-1);
			foreach (ReadState map in Enum.GetValues<ReadState>())
			{

				if (map == ReadState.seeds) continue;
				while (inputList.Count > 0)
				{
					(long min, long max) key = inputList[0];
					inputList.RemoveAt(0);

					if (dictMap[map].Any(p => p.Key.Item1 <= key.min && p.Key.Item1 + p.Key.Item2 > key.min))
					{
						//activeDict.Add((sourceStart, rangeSize), destStart);
						KeyValuePair<(long sourceStart, long rangeSize), long> range = dictMap[map].FirstOrDefault(p => p.Key.Item1 <= key.min && p.Key.Item1 + p.Key.Item2 > key.min);
						if (range.Key.sourceStart + range.Key.rangeSize < key.max)
						{
							val = (key.min - range.Key.sourceStart + range.Value, range.Value + range.Key.rangeSize);
							workingList.Add(val);

							long m = range.Key.rangeSize + range.Key.sourceStart;

							inputList.Add((m, key.max));
						}
						else
						{
							val = (key.min - range.Key.Item1 + range.Value, key.max - range.Key.Item1 + range.Value);
							workingList.Add(val);
						}
					}
					else
					{
						workingList.Add(key);
					}
				}

				inputList.AddRange(workingList);
				workingList.Clear();
			}
			return inputList;
		}
	}
}