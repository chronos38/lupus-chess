using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lupus.Chess.Algorithm;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Tree
{
	internal class TreeSearch : ITreeSearch
	{
		private ICollection<INode> _prediction; 
		private Side _aiSide = Side.White;
		// TODO: Implement missing values
		private IEvaluation _evaluation = new Evaluation
		{
			Strategies = new Collection<Tuple<float, IStrategy>>
			{
				Tuple.Create(1.0f, (IStrategy) new Material()),
				Tuple.Create(0.1f, (IStrategy) new BishopPosition()),
				Tuple.Create(0.1f, (IStrategy) new KnightPosition()),
				Tuple.Create(0.1f, (IStrategy) new RookPosition()),
				Tuple.Create(0.1f, (IStrategy) new QueenPosition()),
				Tuple.Create(0.1f, (IStrategy) new PawnStructure())
			}
		};
		// TODO: Evaluate good default value
		private int _depth = 4;
		private int _moveTimeout = 30;

		public Side AiSide
		{
			get { return _aiSide; }
			set { _aiSide = value; }
		}

		public int Timeout
		{
			get { return _moveTimeout; }
			set { _moveTimeout = value; }
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

			_prediction = ProgressiveDeepening(field, depth);
			if (_prediction.Count == 0) throw new ChessEvaluationException("Could not evaluate new move.");
			return _prediction.First().Move;
		}

		private ICollection<INode> ProgressiveDeepening(Field field, int depth)
		{
			var result = new List<INode>();
			var start = DateTime.UtcNow;

			while (true)
			{
				if (result.Count > 0) field = result.Last().Field;
				var side = result.Count == 0 ? AiSide : (result.Last().PlySide == Side.White ? Side.Black : Side.White);
				var moves = Node.AllowedMoves(side, field);
				var tasks = new Collection<Tuple<Task<long>, Node>>();
				long value = Int32.MinValue;
				Node swap = null;

				foreach (var move in moves)
				{
					var fieldClone = (Field) field.Clone();
					var piece = fieldClone.GetPiece(move.From);
					if (piece == null) continue;
					piece.Move(fieldClone, move.To);

					var node = new Node
					{
						Field = fieldClone,
						Move = move,
						PlySide = side
					};

					var task = Task<long>.Factory.StartNew(() => AlphaBeta(node, Int32.MinValue, Int32.MaxValue, depth));
					tasks.Add(Tuple.Create(task, node));
				}

				foreach (var task in tasks)
				{
					if (task.Item1.Result < value && swap != null) continue;
					value = task.Item1.Result;
					swap = task.Item2;
				}

				result.Add(swap);
				if (DateTime.UtcNow - start >= new TimeSpan(0, 0, 0, Timeout)) break;
			}

			return result;
		}

		private long AlphaBeta(INode node, long alpha, long beta, int depth)
		{
			if (depth == 0 || node.Terminal) return Evaluation.Execute(node.Field, node.PlySide);

			long best = Int32.MinValue;

			foreach (var child in node.Where(n => n != null))
			{
				var value = -AlphaBeta(child, -beta, -alpha, depth - 1);

				if (value >= beta) return value;
				if (value <= best) continue;
				best = value;
				if (value > alpha) alpha = value;
			}

			return best;
		}

		private long AlphaBetaMax(INode node, long alpha, long beta, int depth)
		{
			if (depth <= 0 || node.Terminal) return Evaluation.Execute(node.Field, AiSide);

			foreach (var next in node.Where(next => next != null))
			{
				alpha = Math.Max(alpha, AlphaBetaMin(next, alpha, beta, depth - 1));
				if (alpha >= beta) return beta;
			}

			return alpha;
		}

		private long AlphaBetaMin(INode node, long alpha, long beta, int depth)
		{
			if (depth <= 0 || node.Terminal) return Evaluation.Execute(node.Field, AiSide);

			foreach (var next in node.Where(next => next != null))
			{
				beta = Math.Min(beta, AlphaBetaMax(next, alpha, beta, depth - 1));
				if (beta <= alpha) return alpha;
			}

			return beta;
		}
	}
}
