using System;
using System.Collections.ObjectModel;

namespace Lupus.Chess
{
	public class History : Collection<Move>, ICloneable
	{
		private static readonly Lazy<History> Singleton =
			new Lazy<History>(() => new History());

		public static History Instance { get { return Singleton.Value; } }

		public object Clone()
		{
			var result = new History();
			foreach (var move in this) result.Add((Move) move.Clone());
			return result;
		}
	}
}
