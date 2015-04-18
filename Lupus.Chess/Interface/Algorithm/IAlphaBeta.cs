using System.Collections.Generic;

namespace Lupus.Chess.Interface.Algorithm
{
	public interface IAlphaBeta
	{
		IEvaluation Evaluation { get; set; }
		IDictionary<Field, INode> TranspositionTable { get; set; }
		long Execute(INode node, long alpha, long beta, int depth);
	}
}