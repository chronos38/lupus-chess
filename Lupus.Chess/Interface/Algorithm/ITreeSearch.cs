namespace Lupus.Chess.Interface.Algorithm
{
	public interface ITreeSearch
	{
		IEvaluation Evaluation { get; set; }
		uint Depth { get; set; }
		Move Execute(Field field);
		Move Execute(Field field, uint depth);
	}
}
