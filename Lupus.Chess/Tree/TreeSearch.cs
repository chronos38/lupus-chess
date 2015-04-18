using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Lupus.Chess.Algorithm;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Tree
{
	internal class TreeSearch : ITreeSearch
	{
		public INode Root { get; set; }
		public IAlphaBeta AlphaBeta { get; set; }

		internal TreeSearch(INode root, IAlphaBeta alphaBeta)
		{
			if (root == null) throw new ArgumentNullException("root");
			if (alphaBeta == null) throw new ArgumentNullException("alphaBeta");
			AlphaBeta = alphaBeta;
			Root = root;
		}

		public Move Execute(Field currentField, int depth)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Finds the node that corresponds to the given field.
		/// </summary>
		/// <param name="root">Root node.</param>
		/// <param name="currentField">Field one ply ahead of root.</param>
		/// <returns>The new root node.</returns>
		public INode FindCorrespondingNode(INode root, Field currentField)
		{
			return TranspositionTable.Instance.ContainsKey(currentField)
				? TranspositionTable.Instance[currentField]
				: root;
		}

		/// <summary>
		/// Searches for the best possible move.
		/// </summary>
		/// <param name="root">The current root node.</param>
		/// <returns>Node with the best evaluated move.</returns>
		public static Move RootSearch(INode root)
		{
			throw new NotImplementedException();
		}

		public static void IterativeDeepening(INode root, IAlphaBeta alphaBeta, int depth)
		{
			/*
			 * Best-case:
			 *   Nachfolger von Max sind absteigend sortiert.
			 *   Nachfolger von Min sind aufsteigend sortiert.
			 */
			while (depth > 0)
			{


				foreach (var node in root)
				{
					
				}

				depth -= 1;
			}
		}
	}
}
