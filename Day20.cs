using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AdventofCode2023
{
	internal static class Day20
	{
		private interface IModule
		{
			public string id { get; }
			List<string> destinations { get; }
			public void RecievePulse(string from, bool isHigh);

			public void DoRegisterIO();
			public void RegisterIO(string from);
		}

		private class OutputModule : IModule
		{
			public string id { get; private set; }
			public List<string> destinations { get; private set; }
			public OutputModule(string id, string dests)
			{
				destinations = new List<string>();
				this.id = id[1..];
				destinations.AddRange(dests.Split(',').Select(d => d.Trim()));
			}

			public void RecievePulse(string from, bool isHigh)
			{
				if (isHigh) return;
				OutputRecieved = true;
			}

			public void DoRegisterIO()
			{

			}

			public void RegisterIO(string from)
			{

			}
		}

		private class FlipFlop : IModule
		{
			public string id { get; private set; }
			public List<string> destinations { get; private set; }
			private bool isOn = false;
			
			public FlipFlop(string id, string dests)
			{
				destinations = new List<string>();
				this.id = id[1..];
				destinations.AddRange(dests.Split(',').Select(d => d.Trim()));
			}



			public void RecievePulse(string from, bool isHigh)
			{
				if (isHigh) return;

				isOn = !isOn;
				foreach (string dest in destinations)
				{
					if (isOn)
						highPulse++;
					else
						lowPulse++;
					if (isOn)
					{
						//send a high pulse
						outgoingPulses.Enqueue((dest,id,true));
					}
					else
					{
						//send a low pulse
						outgoingPulses.Enqueue((dest, id, false));
					}
				}
			}

			public void DoRegisterIO()
			{
				foreach (string dest in destinations)
				{
					if(allModules.TryGetValue(dest, out IModule m))
						m.RegisterIO(id);
					else
					{
						OutputModule f = new OutputModule(dest, "");
						allModules.Add(f.id, f);
						f.RegisterIO(id);
					}
				}
			}

			public void RegisterIO(string from)
			{

			}
		}

		private class ConjunctionModule : IModule
		{
			public string id { get; private set; }
			public List<string> destinations { get; private set; }
			private Dictionary<string, bool> mostRecentPulseWasHigh = new Dictionary<string, bool>();

			public ConjunctionModule(string id, string dests)
			{
				destinations = new List<string>();
				this.id = id[1..];
				destinations.AddRange(dests.Split(',').Select(d => d.Trim()));
			}

			public void RecievePulse(string from, bool isHigh)
			{
				mostRecentPulseWasHigh[from] = isHigh;
				bool sendPulseHigh = !mostRecentPulseWasHigh.All(kvp => kvp.Value);
				foreach (string dest in destinations)
				{
					if (sendPulseHigh) highPulse++;
					else lowPulse++;
					outgoingPulses.Enqueue((dest, id, sendPulseHigh));
				}

				/****
				 By hand:
				 the four conjunction modules that need to send-high
				 in order for `ll` to send low to `rx`
				 ****/
				if (id == "kv" && sendPulseHigh)
				{
					//cycle length: 4013
					Console.WriteLine($"{id} high at {numPresses}");
				}
				if (id == "vb" && sendPulseHigh)
				{
					//cycle length: 3793
					Console.WriteLine($"{id} high at {numPresses}");
				}
				if (id == "vm" && sendPulseHigh)
				{
					//cycle length: 4051
					Console.WriteLine($"{id} high at {numPresses}");
				}

				if (id == "kl" && sendPulseHigh)
				{
					//cycle length: 3917
					Console.WriteLine($"{id} high at {numPresses}");
				}
			}

			public void DoRegisterIO()
			{
				foreach (string dest in destinations)
				{
					if (allModules.TryGetValue(dest, out IModule m))
						m.RegisterIO(id);
					else
					{
						OutputModule f = new OutputModule("*" + dest, "");
						allModules.Add(f.id, f);
						f.RegisterIO(id);
					}
				}
			}

			public void RegisterIO(string from)
			{
				mostRecentPulseWasHigh[from] = false;
			}
		}

		private class Broadcaster : IModule
		{
			public string id { get; private set; }
			public List<string> destinations { get; private set; }

			public Broadcaster(string id, string dests)
			{
				destinations = new List<string>();
				this.id = id[1..];
				destinations.AddRange(dests.Split(',').Select(d => d.Trim()));
			}

			public void RecievePulse(string from, bool isHigh)
			{
				foreach (string dest in destinations)
				{
					if (isHigh) highPulse++;
					else lowPulse++;
					outgoingPulses.Enqueue((dest, id, isHigh));
				}
			}

			public void PressButton()
			{
				lowPulse++;
				foreach (string dest in destinations)
				{
					lowPulse++;
					outgoingPulses.Enqueue((dest, id, false));
				}
			}
			public void DoRegisterIO()
			{
				foreach (string dest in destinations)
				{
					if (allModules.TryGetValue(dest, out IModule m))
						m.RegisterIO(id);
				}
			}

			public void RegisterIO(string from)
			{

			}
		}

		private static long highPulse = 0L;
		private static long lowPulse = 0L;

		private static Dictionary<string, IModule> allModules = new Dictionary<string, IModule>();
		private static Queue<(string dest, string from, bool isHigh)> outgoingPulses = new Queue<(string dest, string from, bool isHigh)>();

		internal static long Part1(string input)
		{
			string[] lines = input.Split('\n');
			Broadcaster broadcaster = null;
			foreach (string line in lines)
			{
				string[] parts = line.Split(" -> ");
				switch (parts[0][0])
				{
					case '%':
						FlipFlop f = new FlipFlop(parts[0], parts[1]);
						allModules.Add(f.id, f);
						break;
					case '&':
						ConjunctionModule c = new ConjunctionModule(parts[0], parts[1]);
						allModules.Add(c.id, c);
						break;
					case 'b':
						broadcaster = new Broadcaster(parts[0], parts[1]);
						break;
				}
			}

			foreach (IModule module in allModules.Values.ToArray())
			{
				module.DoRegisterIO();
			}

			if (broadcaster == null) return -1;

			for (int i = 0; i < 1000; i++)
			{
				broadcaster.PressButton();
				while (outgoingPulses.Count > 0)
				{
					(string dest, string from, bool isHigh) = outgoingPulses.Dequeue();
					if(!allModules.ContainsKey(dest)) 
						continue;
					allModules[dest].RecievePulse(from, isHigh);
				}
			}
			Console.WriteLine($"{highPulse} * {lowPulse}");
			return highPulse * lowPulse;
		}

		private static long numPresses = 0L;
		private static bool OutputRecieved = false;

		internal static long Part2(string input)
		{
			allModules.Clear();
			outgoingPulses.Clear();
			string[] lines = input.Split('\n');
			Broadcaster broadcaster = null;
			foreach (string line in lines)
			{
				string[] parts = line.Split(" -> ");
				switch (parts[0][0])
				{
					case '%':
						FlipFlop f = new FlipFlop(parts[0], parts[1]);
						allModules.Add(f.id, f);
						break;
					case '&':
						ConjunctionModule c = new ConjunctionModule(parts[0], parts[1]);
						allModules.Add(c.id, c);
						break;
					case 'b':
						broadcaster = new Broadcaster(parts[0], parts[1]);
						break;
				}
			}

			foreach (IModule module in allModules.Values.ToArray())
			{
				module.DoRegisterIO();
			}

			if (broadcaster == null) return -1;

			for (;; numPresses++)
			{
				broadcaster.PressButton();
				while (outgoingPulses.Count > 0)
				{
					(string dest, string from, bool isHigh) = outgoingPulses.Dequeue();
					if (!allModules.ContainsKey(dest))
						continue;
					allModules[dest].RecievePulse(from, isHigh);
				}

				if (OutputRecieved)
				{
					return numPresses;
				}

				if (numPresses % 100000 == 0)
				{
					Console.WriteLine(numPresses);
				}
			}
		}
	}
}
