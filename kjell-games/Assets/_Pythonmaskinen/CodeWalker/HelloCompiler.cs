﻿using UnityEngine;
using System.Collections.Generic;
using System;
using Compiler;
using System.Collections.ObjectModel;

namespace PM
{
	public class HelloCompiler : MonoBehaviour
	{
		public bool isRunning { get; private set; }

		public CodeWalker theCodeWalker;
		public VariableWindow theVarWindow;

		[NonSerialized]
		public List<Compiler.Function> addedFunctions = new List<Compiler.Function>();

		public readonly ReadOnlyCollection<Function> globalFunctions = new ReadOnlyCollection<Function>(new Function[] {
			new GlobalFunctions.AbsoluteValue(),
			new GlobalFunctions.ConvertToBinary(),
			new GlobalFunctions.ConvertToBoolean(),
			new GlobalFunctions.ConvertToFloat(),
			new GlobalFunctions.ConvertToHexadecimal(),
			new GlobalFunctions.ConvertToInt("int"),
			new GlobalFunctions.ConvertToInt("long"),
			new GlobalFunctions.ConvertToString(),
			new GlobalFunctions.LengthOf(),
			new GlobalFunctions.RoundedValue(),
			new GlobalFunctions.MinimumValue(),
			new GlobalFunctions.MaximumValue(),
			new GlobalFunctions.GetTime(),
		});

		public List<Function> allAddedFunctions
		{
			get
			{
				List<Function> allFunctions = new List<Function>(globalFunctions);
				allFunctions.AddRange(addedFunctions);
				return allFunctions;
			}
		}

		void Start()
		{
			Runtime.Print.printFunction = prettyPrint;
		}

		public void compileCode()
		{
			if (isRunning) return;

			isRunning = true;

			foreach (var ev in UISingleton.FindInterfaces<IPMCompilerStarted>())
				ev.OnPMCompilerStarted();

			try
			{
				Runtime.VariableWindow.setVariableWindowFunctions(theVarWindow.addVariable, theVarWindow.resetList);
				ErrorHandler.ErrorMessage.setLanguage();
				ErrorHandler.ErrorMessage.setErrorMethod(PMWrapper.RaiseError);

				GameFunctions.setGameFunctions(allAddedFunctions);

				theCodeWalker.ActivateWalker(stopCompiler);
			}
			catch
			{
				stopCompiler(StopStatus.RuntimeError);
				throw;
			}
		}

		public void prettyPrint(string dasMessage)
		{
			if (UISingleton.instance.textField.devBuild)
				print(dasMessage);
		}

		#region stop methods
		public void stopCompilerButton()
		{
			stopCompiler(StopStatus.UserForced);
		}

		public void stopCompiler(StopStatus status = StopStatus.CodeForced)
		{
			isRunning = false;

			theCodeWalker.StopWalker();

			// Call stop events
			foreach (var ev in UISingleton.FindInterfaces<IPMCompilerStopped>())
				ev.OnPMCompilerStopped(status);
		}
		#endregion

		public enum StopStatus
		{
			/// <summary>
			/// The compiler was stopped by user via pressing the stop button.
			/// </summary>
			UserForced,
			/// <summary>
			/// The compiler was stopped by code via e.g. PMWrapper.
			/// </summary>
			CodeForced,
			/// <summary>
			/// The compiler finished successfully.
			/// </summary>
			Finished,
			/// <summary>
			/// The compiler had an error during runtime. For example some missing variable or syntax error.
			/// </summary>
			RuntimeError,
		}
	}

}