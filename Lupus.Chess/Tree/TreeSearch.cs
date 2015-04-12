using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Algorithm;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Tree
{
	internal class TreeSearch : ITreeSearch
	{
		private Side _aiSide = Side.None;
		// TODO: Implement missing values
		private IEvaluation _evaluation = new Evaluation
		{
			Strategies = new Collection<Tuple<float, IStrategy>>
			{
				Tuple.Create(1.0f, (IStrategy) new Material())
			}
		};
		// TODO: Evaluate good default value
		private int _depth = 7;

		public Side AiSide
		{
			get { return _aiSide; }
			set { _aiSide = value; }
		}

		public IEvaluation Evaluation
		{
			get { return _evaluation; }
			set { _evaluation = value; }
		}

		public int Depth
		{
			get { return _depth; }
			set { _depth = value; }
		}

		public Move Execute(Field field)
		{
			return Execute(field, Depth);
		}

		public Move Execute(Field field, int depth)
		{
			if (AiSide != Side.White && AiSide != Side.Black) throw new ChessEvaluationException("AI Side for tree search is missing or invalid.");
			var moves = Node.AllowedMoves(AiSide, field);
			var result = Tuple.Create((INode) null, Int32.MinValue);
			var tasks = moves.Select(move => new Node
			{
				Field = field, Move = move, PlySide = AiSide
			}).Select(node => Tuple.Create((INode) node, AlphaBetaMax(node, Int32.MinValue, Int32.MaxValue, depth))).ToList();

			foreach (var tuple in tasks)
			{
				tuple.Item2.Wait();
				result = tuple.Item2.Result > result.Item2 ? Tuple.Create(tuple.Item1, tuple.Item2.Result) : result;
			}

			return result.Item1 == null ? null : result.Item1.Move;
		}

		private async Task<int> AlphaBetaMax(INode node, int alpha, int beta, int depth)
		{
			if (depth <= 0) return Evaluation.Execute(node.Field, AiSide);
			node.Expand();

			foreach (var next in node)
			{
				alpha = Math.Max(alpha, await AlphaBetaMin(next.First(), alpha, beta, depth - 1));
				if (alpha >= beta) return beta;
			}

			return alpha;
		}

		private async Task<int> AlphaBetaMin(INode node, int alpha, int beta, int depth)
		{
			if (depth <= 0) return Evaluation.Execute(node.Field, AiSide);
			node.Expand();

			foreach (var next in node)
			{
				beta = Math.Min(beta, await AlphaBetaMax(next.First(), alpha, beta, depth - 1));
				if (beta <= alpha) return alpha;
			}

			return beta;
		}
	}
}
