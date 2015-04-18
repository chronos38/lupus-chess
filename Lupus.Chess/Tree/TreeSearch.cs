using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Mime;
using Lupus.Chess.Algorithm;
using Lupus.Chess.Algorithm.Evaluation;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Tree
{
	internal class TreeSearch : ITreeSearch
	{
		public INode Root { get; set; }
		public IEvaluation Evaluation { get; set; }
		public IAlphaBeta AlphaBeta { get; set; }
		public IDictionary<Field, INode> TranspositionTable { get; set; }

		internal TreeSearch(INode root)
		{
			if (root == null) throw new ArgumentNullException("root");

			Root = root;
			TranspositionTable = new Dictionary<Field, INode>();
			Evaluation = new Evaluation
			{
				Strategies = new Collection<IStrategy>
				{
					new Material(),
					new BishopPosition(),
					new KnightPosition(),
					new RookPosition(),
					new QueenPosition(),
					new PawnStructure()
				}
			};
			AlphaBeta = new AlphaBetaFailSoft
			{
				Evaluation = Evaluation,
				TranspositionTable = TranspositionTable
			};
		}

		internal TreeSearch(INode root, IEvaluation evaluation, IAlphaBeta alphaBeta)
		{
			if (root == null) throw new ArgumentNullException("root");
			if (evaluation == null) throw new ArgumentNullException("evaluation");
			if (alphaBeta == null) throw new ArgumentNullException("alphaBeta");

			Root = root;
			TranspositionTable = new Dictionary<Field, INode>();
			Evaluation = evaluation;
			AlphaBeta = alphaBeta;
		}

		public Move Execute(Field currentField, int depth)
		{
			var node = IterativeDeepening(FindCorrespondingNode(Root, currentField), AlphaBeta, Evaluation, depth);
			return FindBestValue(node).Move;
		}

		/// <summary>
		/// Finds the node that corresponds to the given field.
		/// </summary>
		/// <param name="root">Root node.</param>
		/// <param name="currentField">Field one ply ahead of root.</param>
		/// <returns>The new root node.</returns>
		public INode FindCorrespondingNode(INode root, Field currentField)
		{
			return TranspositionTable[currentField] ?? root;
		}

		/// <summary>
		/// Searches for the best possible move.
		/// </summary>
		/// <param name="root">The current root node.</param>
		/// <returns>Node with the best evaluated move.</returns>
		public static INode FindBestValue(INode root)
		{
			return root.Aggregate((current, node) => current == null ? node : (node.Value > current.Value ? node : current));
		}

		public static INode IterativeDeepening(INode root, IAlphaBeta alphaBeta, IEvaluation evaluation, int depth)
		{
			throw new NotImplementedException();
			/*
			 * Best-case:
			 *   Nachfolger von Max sind absteigend sortiert.
			 *   Nachfolger von Min sind aufsteigend sortiert.
			 */
			while (depth > 0)
			{
				alphaBeta.Execute(root, int.MinValue, int.MaxValue, 2);
				depth -= 1;
			}
		}
	}
}
