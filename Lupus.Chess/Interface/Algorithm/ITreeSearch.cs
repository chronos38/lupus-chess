namespace Lupus.Chess.Interface.Algorithm
{
	public interface ITreeSearch
	{
		Side AiSide { get; set; }
		IEvaluation Evaluation { get; set; }
		int Depth { get; set; }
		Move Execute(Field field);
		Move Execute(Field field, int depth);
	}
}
