using System;
using System.Collections.ObjectModel;

namespace Lupus.Chess
{
	public sealed class History : Collection<Move>
	{
		private static readonly Lazy<History> Singleton =
			new Lazy<History>(() => new History());

		public static History Instance { get { return Singleton.Value; } }

		private History()
		{
		}
	}
}
