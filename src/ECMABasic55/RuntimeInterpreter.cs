﻿using ECMABasic.Core;
using ECMABasic.Core.Configuration;
using ECMABasic.Core.Exceptions;
using ECMABasic55.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECMABasic55
{
	/// <summary>
	/// The core interpreter with immediate-mode statements tacked on.
	/// </summary>
	public class RuntimeInterpreter : Interpreter
	{
		private readonly List<StatementParser> _immediateStatements;

		public RuntimeInterpreter(IBasicConfiguration config = null)
			: base(config)
		{
			_immediateStatements = new List<StatementParser>()
			{
				new RunStatementParser(),
				new NewStatementParser(),
				new ContinueStatementParser(),
				new LoadStatementParser(),
				new ListStatementParser(),
				new SaveStatementParser(),
			};
		}

		public IStatement ProcessImmediate(IEnvironment env, string text)
		{
			try
			{
				_reader = ComplexTokenReader.FromText(text);

				if (ProcessBlock(env, null))
				{
					return null;
				}
				else
				{
					ProcessSpace(false);
					var statement = ProcessStatement(null, false);
					if (statement == null)
					{
						statement = ProcessImmediateStatement();
					}

					ProcessSpace(false);
					ProcessEndOfLine();
					return statement;
				}
			}
			catch (SyntaxException)
			{
				throw;
			}
			catch (Exception)
			{
				throw ExceptionFactory.Syntax();
			}
		}

		private IStatement ProcessImmediateStatement()
		{
			foreach (var parser in _immediateStatements)
			{
				var stmt = parser.Parse(_reader);
				if (stmt != null)
				{
					return stmt;
				}
			}
			return null;
		}
	}
}
