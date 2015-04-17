using System.Threading.Tasks;

namespace Lupus.Chess.Interface.Algorithm
{
	public interface ITreeSearch
	{
		INode Root { get; set; }
		IEvaluation Evaluation { get; set; }
		IAlphaBeta AlphaBeta { get; set; }
		Move Execute(Field currentField, int depth);
	}
}
