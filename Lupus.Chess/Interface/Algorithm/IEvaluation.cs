using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Lupus.Chess.Interface.Algorithm
{
	public interface IEvaluation
	{
		ICollection<IStrategy> Strategies { get; set; }  
		int Execute(Field field, Side side);
	}
}
