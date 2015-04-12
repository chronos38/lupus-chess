using System.Threading.Tasks;

namespace Lupus.Chess.Interface.Algorithm
{
	public interface ITreeSearch
	{
		IEvaluation Evaluation { get; set; }
		Task<Move> Execute(Field field);
		Task<Move> Execute(Field field, int depth);
	}
}
