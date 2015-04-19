using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Lupus.Chess.Interface;

namespace Lupus.Chess
{
	public sealed class TranspositionTable : Dictionary<Field, Tuple<INode, ICollection<Move>>>
	{
		private static readonly Lazy<TranspositionTable> Singleton =
			new Lazy<TranspositionTable>(() => new TranspositionTable());

		public static TranspositionTable Instance { get { return Singleton.Value; } }

		private TranspositionTable()
		{
		}

		public static void Add(INode node)
		{
			if (Instance.ContainsKey(node.Field)) return;
			lock (Instance)
			{
				if (Instance.ContainsKey(node.Field)) return;
				Instance[node.Field] = new Tuple<INode, ICollection<Move>>(node, new Collection<Move>());
			}
		}
	}
}