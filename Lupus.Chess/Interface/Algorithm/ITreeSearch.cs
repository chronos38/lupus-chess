using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lupus.Chess.Interface.Algorithm
{
	public interface ITreeSearch
	{
		ICollection<Task> Tasks { get; }
		INode Root { get; }
		IAlphaBeta AlphaBeta { get; }
		IEnumerable<Move> Execute(int depth);
		IEnumerable<Move> Execute(int depth, int timeout);
		IEnumerable<Move> Execute(int depth, int timeout, INode root);
	}
}
