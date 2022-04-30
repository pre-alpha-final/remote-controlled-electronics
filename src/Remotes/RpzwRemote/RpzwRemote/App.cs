using System;
using System.Threading.Tasks;

namespace RpzwRemote
{
	internal class App
	{
		public async Task Run()
		{
			while (true)
			{
				Console.WriteLine("Menu: [L]ogIn L[i]st [R]un [Q]uit");
				var key = Console.ReadKey(true);
				switch (key.KeyChar)
				{
					case 'l':
					case 'L':

						break;
					case 'i':
					case 'I':
						break;
					case 'r':
					case 'R':
						break;
					case 'q':
					case 'Q':
						return;
					default:
						break;
				}
			}
		}
	}
}
