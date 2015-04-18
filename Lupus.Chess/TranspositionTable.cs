using System;
using System.Collections.Generic;
using Lupus.Chess.Interface;

namespace Lupus.Chess
{
	public sealed class TranspositionTable : Dictionary<Field, INode>
	{
		private static readonly Lazy<TranspositionTable> Singleton =
			new Lazy<TranspositionTable>(() => new TranspositionTable());

		public static TranspositionTable Instance { get { return Singleton.Value; } }

		private TranspositionTable()
		{
		}
	}
}