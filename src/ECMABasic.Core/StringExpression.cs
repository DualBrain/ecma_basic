﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECMABasic.Core
{
	class StringExpression : IExpression
	{
		public StringExpression(string text)
			: base()
		{
			Text = text;
		}

		public string Text { get; }

		public override string ToString()
		{
			return Text;
		}
	}
}
