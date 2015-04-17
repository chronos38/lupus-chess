namespace Lupus.Chess.Interface.Algorithm
{
	public interface IAlphaBeta
	{
		 long Execute(INode node, long alpha, long beta, int depth, IEvaluation evaluation);
	}
}