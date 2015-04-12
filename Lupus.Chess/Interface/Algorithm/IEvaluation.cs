using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Lupus.Chess.Interface.Algorithm
{
	public interface IEvaluation
	{
		/*Tuple<int, IStrategy> BishopPosition { get; set; }
		Tuple<int, IStrategy> CenterControl { get; set; }
		Tuple<int, IStrategy> Connectivity { get; set; }
		Tuple<int, IStrategy> KnightPosition { get; set; }
		Tuple<int, IStrategy> KingSafety { get; set; }
		Tuple<int, IStrategy> Material { get; set; }
		Tuple<int, IStrategy> Mobility { get; set; }
		Tuple<int, IStrategy> PawnStructure { get; set; }
		Tuple<int, IStrategy> QueenPosition { get; set; }
		Tuple<int, IStrategy> RookPosition { get; set; }*/
		ICollection<Tuple<float, IStrategy>> Strategies { get; set; }  
		int Execute(Field field, Side side);
	}
}
