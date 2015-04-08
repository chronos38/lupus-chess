namespace Lupus.Chess.Interface.Algorithm
{
	public interface IMoveGenerator
	{
		IEvaluation Evaluation { get; set; }
		uint Depth { get; set; }
		Move Execute(IField field);
		Move Execute(IField field, uint depth);
	}
}
