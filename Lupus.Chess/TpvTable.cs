using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace Lupus.Chess
{
	public sealed class TpvTable
	{
		private static readonly Lazy<TpvTable> Singleton =
			new Lazy<TpvTable>(() => new TpvTable());

		public static TpvTable Instance { get { return Singleton.Value; } }

		private TpvTable()
		{
		}

		public Move this[int index]
		{
			get { return _moves[index]; }
			set { _moves[index] = value; }
		}

		public void MoveCopy(int targetIndex, int sourceIndex, int n)
		{
			while (n-- > 0)
			{
				if (_moves[sourceIndex + 1] == null) break;
				_moves[targetIndex++] = _moves[sourceIndex++];
			}
		}

		public int Count
		{
			get { return _moves.Count; }
		}

		public static int N { get { return 100; } }

		private readonly List<Move> _moves = new List<Move>((N * N + N) / 2); 
	}
}