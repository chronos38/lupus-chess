using System;
using System.Collections.ObjectModel;
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
			Root = FindCorrespondingNode(Root, currentField);
			var node = IterativeDeepening(Root, AlphaBeta, Evaluation, depth);
			return FindBestValue(node);
		}

		public static INode FindCorrespondingNode(INode root, Field currentField)
		{
			throw new NotImplementedException();
		}

		public static Move FindBestValue(INode node)
		{
			throw new NotImplementedException();
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
				alphaBeta.Execute(root, int.MinValue, int.MaxValue, 2, evaluation);
				depth -= 1;
			}
		}
	}
}
