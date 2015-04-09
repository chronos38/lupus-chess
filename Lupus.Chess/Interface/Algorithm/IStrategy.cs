namespace Lupus.Chess.Interface.Algorithm
{
	public interface IStrategy
	{
		int Execute(Field field, Side side);
	}
}
