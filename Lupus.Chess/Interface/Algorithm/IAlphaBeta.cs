using System.Collections.Generic;

namespace Lupus.Chess.Interface.Algorithm
{
	public interface IAlphaBeta
	{
		long Execute(INode node, Side plySide, long alpha, long beta, int depth, History history);
	}
}