using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lupus.Chess.Exception;
using Lupus.Chess.Interface.Algorithm;

namespace Lupus.Chess.Tree
{
	public static class TreeFactory
	{
		public static ITreeSearch Create(Side aiSide)
		{
			if (aiSide != Side.White && aiSide != Side.Black) throw new ArgumentException("aiSide");

			return new TreeSearch
			{
				AiSide = aiSide
			};
		}

		public static ITreeSearch Create(Side aiSide, int depth)
		{

			if (aiSide != Side.White && aiSide != Side.Black) throw new ArgumentException("aiSide");
			if (depth <= 0) throw new ArgumentOutOfRangeException("depth");

			return new TreeSearch
			{
				AiSide = aiSide,
				Depth = depth
			};
		}
	}
}
