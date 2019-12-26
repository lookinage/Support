//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Support;
//using Support.Sets;
//using System;

//namespace Test.Support.Sets
//{
//	/// <summary>
//	/// Represents the tester of <see cref="ISurjection{TInput, TOutput}"/> instances.
//	/// </summary>
//	static public class ISurjectionTester
//	{
//		private struct GetTester<TInput, TOutput> where TInput : ISetElement<TInput> where TOutput : ISetElement<TOutput>
//		{
//			private readonly ISurjection<TInput, TOutput> _instance;
//			private TInput _input;
//			private TOutput _output;

//			internal GetTester(ISurjection<TInput, TOutput> instance, TInput input)
//			{
//				_instance = instance;
//				_input = input;
//				_output = default;
//			}

//			public TInput Input { get => _input; set => _input = value; }
//			public TOutput Output => _output;

//			internal void Invoke() => _output = _instance[_input];
//		}
//		private struct SetTesterTInputTOutput<TInput, TOutput> where TInput : ISetElement<TInput> where TOutput : ISetElement<TOutput>
//		{
//			private readonly ISurjectionEditor<TInput, TOutput> _instance;
//			private TInput _input;
//			private TOutput _newOutput;

//			internal SetTesterTInputTOutput(ISurjectionEditor<TInput, TOutput> instance, TInput input, TOutput newOutput)
//			{
//				_instance = instance;
//				_input = input;
//				_newOutput = newOutput;
//			}
//			internal SetTesterTInputTOutput(ISurjectionEditor<TInput, TOutput> instance, TInput input) : this(instance, input, default) { }

//			public TInput Input { get => _input; set => _input = value; }
//			public TOutput NewOutput { get => _newOutput; set => _newOutput = value; }

//			internal void Invoke() => _instance[_input] = _newOutput;
//		}
//		private struct SetTesterTInputTOutputRefTOutput<TInput, TOutput> where TInput : ISetElement<TInput> where TOutput : ISetElement<TOutput>
//		{
//			private readonly ISurjectionEditor<TInput, TOutput> _instance;
//			private TInput _input;
//			private TOutput _newOutput;
//			private TOutput _oldOutput;

//			internal SetTesterTInputTOutputRefTOutput(ISurjectionEditor<TInput, TOutput> instance, TInput input, TOutput newOutput)
//			{
//				_instance = instance;
//				_input = input;
//				_newOutput = newOutput;
//				_oldOutput = default;
//			}
//			internal SetTesterTInputTOutputRefTOutput(ISurjectionEditor<TInput, TOutput> instance, TInput input) : this(instance, input, default) { }

//			public TInput Input { get => _input; set => _input = value; }
//			public TOutput NewOutput { get => _newOutput; set => _newOutput = value; }
//			public TOutput OldOutput => _oldOutput;

//			internal void Invoke() => _instance.Set(_input, _newOutput, out _oldOutput);
//		}

//		static private Relation<TInput, TOutput> ConvertToRelation<TInput, TOutput>(TInput element) where TInput : ISetElement<TInput> where TOutput : ISetElement<TOutput> => new Relation<TInput, TOutput>(element, default);

//		/// <summary>
//		/// Tests an <see cref="ISurjection{TInput, TOutput}"/>.
//		/// </summary>
//		/// <typeparam name="TInput">The type of elements of the input set.</typeparam>
//		/// <typeparam name="TOutput">The type of elements of the output set.</typeparam>
//		/// <param name="instance">The <see cref="ISurjection{TInput, TOutput}"/>.</param>
//		/// <param name="content">An <see cref="ISequence{T}"/> of relations that <paramref name="instance"/> contains.</param>
//		/// <param name="complement">An <see cref="ISequence{T}"/> of input elements that <paramref name="instance"/> does not contain.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentNullException"><paramref name="content"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentNullException"><paramref name="complement"/> is <see langword="null"/>.</exception>
//		static public void TestISurjection<TInput, TOutput>(this ISurjection<TInput, TOutput> instance, ISequence<Relation<TInput, TOutput>> content, ISequence<TInput> complement) where TInput : ISetElement<TInput> where TOutput : ISetElement<TOutput>
//		{
//			if (instance == null)
//				throw new ArgumentNullException(nameof(instance));
//			if (content == null)
//				throw new ArgumentNullException(nameof(content));
//			if (complement == null)
//				throw new ArgumentNullException(nameof(complement));
//			foreach (Relation<TInput, TOutput> relation in content)
//			{
//				Assert.IsTrue(instance.Contains(relation.Input));
//				Assert.IsTrue(instance.TryGet(relation.Input, out TOutput gottenOutputElement));
//				Assert.IsTrue(relation.Output == null ? gottenOutputElement == null : relation.Output.Compare(gottenOutputElement));
//				Assert.IsTrue(relation.Output == null ? instance[relation.Input] == null : relation.Output.Compare(instance[relation.Input]));
//			}
//			foreach (TInput input in complement)
//			{
//				Assert.IsFalse(instance.Contains(input));
//				Assert.IsFalse(instance.TryGet(input, out TOutput gottenOutputElement));
//				Assert.IsTrue(gottenOutputElement == default);
//				_ = Assert.ThrowsException<ArgumentException>(new GetTester<TInput, TOutput>(instance, input).Invoke);
//			}
//		}
//		/// <summary>
//		/// Tests an <see cref="ISurjection{TInput, TOutput}"/>.
//		/// </summary>
//		/// <typeparam name="TInput">The type of elements of the input set.</typeparam>
//		/// <typeparam name="TOutput">The type of elements of the output set.</typeparam>
//		/// <param name="instance">The the <see cref="ISurjection{TInput, TOutput}"/>.</param>
//		/// <param name="editor">The the <see cref="ISurjectionEditor{TInput, TOutput}"/> of <paramref name="instance"/>.</param>
//		/// <param name="content">An <see cref="ISequence{T}"/> of <see cref="Relation{TInput, TOutput}"/> instances that <paramref name="instance"/> contains.</param>
//		/// <param name="complement">An <see cref="ISequence{T}"/> of input elements that <paramref name="instance"/> does not contain.</param>
//		/// <exception cref="ArgumentNullException"><paramref name="instance"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentNullException"><paramref name="content"/> is <see langword="null"/>.</exception>
//		/// <exception cref="ArgumentNullException"><paramref name="complement"/> is <see langword="null"/>.</exception>
//		static public void TestISurjectionEditor<TInput, TOutput>(this ISurjection<TInput, TOutput> instance, ISurjectionEditor<TInput, TOutput> editor, ISequence<Relation<TInput, TOutput>> content, ISequence<TInput> complement) where TInput : ISetElement<TInput> where TOutput : ISetElement<TOutput>
//		{
//			if (instance == null)
//				throw new ArgumentNullException(nameof(instance));
//			if (content == null)
//				throw new ArgumentNullException(nameof(content));
//			if (complement == null)
//				throw new ArgumentNullException(nameof(complement));
//			instance.TestISequence();
//			instance.TestISubset(content, complement.GetConvertedSequence(ConvertToRelation<TInput, TOutput>));
//			foreach (Relation<TInput, TOutput> relation in content)
//			{
//				Assert.IsTrue(instance.Contains(relation.Input));
//				Assert.IsTrue(instance.TryGet(relation.Input, out TOutput gottenOutputElement));
//				Assert.IsTrue(relation.Output == null ? gottenOutputElement == null : relation.Output.Compare(gottenOutputElement));
//				Assert.IsTrue(relation.Output == null ? instance[relation.Input] == null : relation.Output.Compare(instance[relation.Input]));
//			}
//			foreach (TInput input in complement)
//			{
//				Assert.IsFalse(instance.Contains(input));
//				Assert.IsFalse(instance.TryGet(input, out TOutput gottenOutputElement));
//				Assert.IsTrue(gottenOutputElement == default);
//				_ = Assert.ThrowsException<ArgumentException>(new GetTester<TInput, TOutput>(instance, input).Invoke);
//			}
//			foreach (Relation<TInput, TOutput> relation in content)
//			{
//				if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
//				{
//					if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
//					{
//						Assert.IsTrue(editor.TrySet(relation.Input, default, out TOutput gottenOutputElement));
//						Assert.IsTrue(relation.Output == null ? gottenOutputElement == null : relation.Output.Compare(gottenOutputElement));
//						continue;
//					}
//					Assert.IsTrue(editor.TrySet(relation.Input, default));
//					continue;
//				}
//				else if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
//				{
//					editor.Set(relation.Input, default, out TOutput gottenOutputElement);
//					Assert.IsTrue(relation.Output == null ? gottenOutputElement == null : relation.Output.Compare(gottenOutputElement));
//				}
//				else
//					editor[relation.Input] = default;
//				Assert.IsTrue(default(TOutput) == null ? instance[relation.Input] == null : default(TOutput).Compare(instance[relation.Input]));
//			}
//			foreach (TInput input in complement)
//			{
//				if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
//				{
//					if (PseudoRandomManager.GetInt32Remainder(0x2) == 0x0)
//					{
//						Assert.IsFalse(editor.TrySet(input, default, out TOutput gottenOutputElement));
//						Assert.IsTrue(gottenOutputElement == default);
//						continue;
//					}
//					Assert.IsFalse(editor.TrySet(input, default));
//					continue;
//				}
//				else
//					_ = Assert.ThrowsException<ArgumentException>(PseudoRandomManager.GetInt32Remainder(0x2) == 0x0 ? new SetTesterTInputTOutputRefTOutput<TInput, TOutput>(editor, input).Invoke : (Action)new SetTesterTInputTOutput<TInput, TOutput>(editor, input).Invoke);
//			}
//		}
//	}
//}