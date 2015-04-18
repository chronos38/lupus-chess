using System;
using System.Collections.ObjectModel;
using System.Linq;
using Lupus.Chess.Algorithm;
using Lupus.Chess.Algorithm.Evaluation;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Tree
{
	internal class TreeSearch : ITreeSearch
	{
		// TODO: Dependency inject this code
		private IEvaluation _evaluation = new Evaluation
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

		// TODO: Dependency inject this code
		private IAlphaBeta _alphaBeta = new AlphaBetaFailSoft();

		public INode Root { get; set; }

		public IEvaluation Evaluation
		{
			get { return _evaluation; }
			set { _evaluation = value; }
		}

		public IAlphaBeta AlphaBeta
		{
			get { return _alphaBeta; }
			set { _alphaBeta = value; }
		}

		public Move Execute(Field currentField, int depth)
		{
			IterativeDeepening(FindCorrespondingNode(Root, currentField), AlphaBeta, Evaluation, depth);
			Root = FindBestValue(Root);
			return Root.Move;
		}

		/// <summary>
		/// Finds the child node for the move the opponent made.
		/// </summary>
		/// <param name="root">Current root node.</param>
		/// <param name="currentField">Field one ply ahead of root.</param>
		/// <returns>The new root node.</returns>
		public static INode FindCorrespondingNode(INode root, Field currentField)
		{
			if (currentField.History.Count == 0) return root;
			var lastMove = currentField.History[currentField.History.Count - 1];
			var node = root.FirstOrDefault(n => n.Move == lastMove);
			return node ?? root;
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

		public static void IterativeDeepening(INode root, IAlphaBeta alphaBeta, IEvaluation evaluation, int depth)
		{
			throw new NotImplementedException();
			/*
			 * Best-case:
			 *   Nachfolger von Max sind absteigend sortiert.
			 *   Nachfolger von Min sind aufsteigend sortiert.
			 */
			while (depth > 0)
			{
				alphaBeta.Execute(root, int.MinValue, int.MaxValue, 2, evaluation);
				depth -= 1;
			}
		}
	}
}
