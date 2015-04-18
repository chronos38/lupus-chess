using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lupus.Chess.Interface.Algorithm
{
	public interface ITreeSearch
	{
		INode Root { get; }
		IAlphaBeta AlphaBeta { get; }
		Move Execute(Field currentField, int depth);
	}
}
