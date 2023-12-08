using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Microsoft.VisualBasic.CompilerServices;

namespace AdventofCode2023 {
	internal static class Day7
	{
		private static char[] highCard = { 'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2' };
		private static char[] highCard2 = { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' };

		private enum RankEnum
		{
			Five,
			Four,
			House,
			Three,
			TwoPair,
			OnePair,
			HighCard
		}


		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			long result = 0l;

			List<(char[] cards, RankEnum rank, long bet)> orderedHands = new List<(char[], RankEnum, long)>();

			foreach (string line in lines)
			{
				string[] handBet = line.Split(' ');
				char[] chars = handBet[0].ToCharArray();
				char[] hand = chars.OrderByDescending(c => Array.IndexOf(highCard, c)).ToArray();
				RankEnum rank = GetHandRank(hand);
				long bet = long.Parse(handBet[1]);
				orderedHands.Add((chars, rank, bet));
			}

			orderedHands = orderedHands.OrderByDescending(h => h.Item2)
				.ThenByDescending(h => Array.IndexOf(highCard, h.cards[0]))
				.ThenByDescending(h => Array.IndexOf(highCard, h.cards[1]))
				.ThenByDescending(h => Array.IndexOf(highCard, h.cards[2]))
				.ThenByDescending(h => Array.IndexOf(highCard, h.cards[3]))
				.ThenByDescending(h => Array.IndexOf(highCard, h.cards[4]))
				.ToList();
			for (int i = 0; i < orderedHands.Count; i++)
			{
				result += (i + 1) * orderedHands[i].bet;
			}
			return result;
		}

		private static RankEnum GetHandRank(char[] hand)
		{
			List<IGrouping<char, char>> groups = hand.GroupBy(c => c).OrderBy(g => g.Count()).ToList();

			foreach (var g in groups)
			{
				if (g.Count() == 5)
					return RankEnum.Five;

				if (g.Count() == 4)
					return RankEnum.Four;

				if (g.Count() == 3)
				{
					if (groups.Count == 2)
					{
						return RankEnum.House;
					}
					else
					{
						return RankEnum.Three;
					}
				}

				if (g.Count() == 2)
				{
					if (groups.Count == 4)
					{
						return RankEnum.OnePair;
					}
					if (groups.Count == 3)
					{
						return RankEnum.TwoPair;
					}
				}
			}

			return RankEnum.HighCard;
		}

		internal static long Part2(string input) {
			string[] lines = input.Split('\n');
			long result = 0l;

			List<(char[] cards, RankEnum rank, long bet)> orderedHands = new List<(char[], RankEnum, long)>();

			foreach (string line in lines)
			{
				string[] handBet = line.Split(' ');
				char[] chars = handBet[0].ToCharArray();
				char[] hand = chars.OrderByDescending(c => Array.IndexOf(highCard2, c)).ToArray();
				RankEnum rank = GetHandRank2(hand);
				long bet = long.Parse(handBet[1]);
				orderedHands.Add((chars, rank, bet));
			}

			orderedHands = orderedHands.OrderByDescending(h => h.Item2)
				.ThenByDescending(h => Array.IndexOf(highCard2, h.cards[0]))
				.ThenByDescending(h => Array.IndexOf(highCard2, h.cards[1]))
				.ThenByDescending(h => Array.IndexOf(highCard2, h.cards[2]))
				.ThenByDescending(h => Array.IndexOf(highCard2, h.cards[3]))
				.ThenByDescending(h => Array.IndexOf(highCard2, h.cards[4]))
				.ToList();

			for (int i = 0; i < orderedHands.Count; i++)
			{
				result += (i + 1) * orderedHands[i].bet;
			}
			return result;
		}

		private static RankEnum GetHandRank2(char[] hand)
		{
			List<IGrouping<char, char>> groups = hand.GroupBy(c => c).OrderBy(g => g.Count()).ToList();

			RankEnum naiveRank = GetHandRank(hand);

			if (groups.Exists(m => m.Key == 'J'))
			{
				var g = groups.First(g => g.Key == 'J');
				var best = groups.OrderByDescending(j => j.Count())
					.ThenBy(j => Array.IndexOf(highCard2, j.Key)).First();
				if (best.Key == 'J')
				{
					var newBest = groups.Where(q => q.Key != 'J').OrderByDescending(j => j.Count())
						.ThenBy(j => Array.IndexOf(highCard2, j.Key)).ToList();
					if (newBest.Count == 0)
					{
						return naiveRank;
					}
					else
					{
						best = newBest.First();
					}
				}
				string fake = string.Join("", hand).Replace('J', best.Key);
				return GetHandRank(fake.ToCharArray().OrderByDescending(c => Array.IndexOf(highCard2, c)).ToArray());
			}
			else
			{
				return naiveRank;
			}
		}
	}
}