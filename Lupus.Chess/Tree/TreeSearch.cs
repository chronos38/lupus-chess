using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Lupus.Chess.Algorithm;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Tree
{
	internal class TreeSearch : ITreeSearch
	{
		private Side _aiSide = Side.White;
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

		public Task<Move> Execute(Field field)
		{
			return Execute(field, Depth);
		}

		public Task<Move> Execute(Field field, int depth)
		{
			if (AiSide != Side.White && AiSide != Side.Black) throw new ChessEvaluationException("AI Side for tree search is missing or invalid.");
			return Task<Move>.Factory.StartNew(() =>
			{
				var moves = Node.AllowedMoves(AiSide, field);
				var result = Tuple.Create((INode) null, Int32.MinValue);
				var nodes = new Collection<INode>();

				foreach (var move in moves)
				{
					var fieldClone = (Field) field.Clone();
					var piece = fieldClone.GetPiece(move.From);
					if (piece == null) return null;
					piece.Move(fieldClone, move.To);
					nodes.Add(new Node
					{
						Field = fieldClone,
						Move = move,
						PlySide = AiSide
					});
				}
				var tasks = nodes.Select(node => Tuple.Create(node, AlphaBetaMax(node, Int32.MinValue, Int32.MaxValue, depth))).ToList();
				result = tasks.Aggregate(result,
					(current, tuple) => tuple.Item2.Result > current.Item2 ? Tuple.Create(tuple.Item1, tuple.Item2.Result) : current);
				return result.Item1 == null ? null : result.Item1.Move;
			});
		}

		private async Task<int> AlphaBetaMax(INode node, int alpha, int beta, int depth)
		{
			if (depth <= 0 || node.Terminal) return Evaluation.Execute(node.Field, AiSide);

			foreach (var next in node.Where(next => next != null))
			{
				alpha = Math.Max(alpha, await AlphaBetaMin(next, alpha, beta, depth - 1));
				if (alpha >= beta) return beta;
			}

			return alpha;
		}

		private async Task<int> AlphaBetaMin(INode node, int alpha, int beta, int depth)
		{
			if (depth <= 0 || node.Terminal) return Evaluation.Execute(node.Field, AiSide);

			foreach (var next in node.Where(next => next != null))
			{
				beta = Math.Min(beta, await AlphaBetaMax(next, alpha, beta, depth - 1));
				if (beta <= alpha) return alpha;
			}

			return beta;
		}
	}
}
