namespace Lupus.Chess.Interface.Algorithm
{
	public interface IMoveGenerator
	{
		IEvaluation Evaluation { get; set; }
		uint Depth { get; set; }
		Move Execute(Field field);
		Move Execute(Field field, uint depth);
	}
}
