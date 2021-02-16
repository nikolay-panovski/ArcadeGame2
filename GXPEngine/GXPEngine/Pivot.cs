using System;

namespace GXPEngine
{
	/// <summary>
	/// This is an 'empty' GameObject. You can use it as a container for other sprites (parent).
	/// </summary>
	public class Pivot : GameObject
	{
		public string _marker { get; private set; }		// but usually I should extend Pivot for my own needs,
														// since technically it is part of the engine itself
		public Pivot (string marker = "") : base()
		{
			_marker = marker;
		}
	}
}

