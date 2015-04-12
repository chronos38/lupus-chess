using System.Threading.Tasks;

namespace Lupus.Chess.Interface.Algorithm
{
	public interface ITreeSearch
	{
		int Timeout { get; set; }
		IEvaluation Evaluation { get; set; }
		Move Execute(Field field);
		Move Execute(Field field, int depth);
	}
}
