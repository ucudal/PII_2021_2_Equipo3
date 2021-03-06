using System;
using Library;
using Library.Core;
using Library.Core.Distribution;
using Library.Core.Processing;
using Library.States;
using Library.InputHandlers;
using Library.InputHandlers.Abstractions;
using ProgramTests.Utils;
using NUnit.Framework;

namespace ProgramTests
{
    /// <summary>
    /// This class represents unit tests concerning the class <see cref="BaseFormProcessor{T, U}" />.
    /// </summary>
    [TestFixture]
    public class FormProcessorTest
    {
        /// <summary>
        /// Tests the functionality itself of the <see cref="BaseFormProcessor{T, U}" />.
        /// </summary>
        [Test]
        public void FormProcessorBasicTest()
        {
            Singleton<SessionManager>.Instance.RemoveUser("___");
            Console.WriteLine();
            int value = default;
            BasicUtils.CreateUser(new FormProcessorTestState(v => value = v));
            ProgramaticPlatform platform = new ProgramaticPlatform(
                "___",
                new string[]
                {
                    "Hola",
                    "35",
                    "24"
                }
            );
            platform.Run();
            foreach(string i in platform.ReceivedMessages)
            {
                Console.WriteLine($"\t--------\n{i}");
            }
            Singleton<SessionManager>.Instance.RemoveUser("___");
            Assert.AreEqual(3524, value);
        }

        private class FormProcessorTestState : InputHandlerState
        {
            public FormProcessorTestState(Action<int> f) : base(
                exitState: () => (new BasicState(), null),
                nextState: () => (new BasicState(), null),
                inputHandler: new ProcessorHandler<int>(
                    (result) =>
                    {
                        f(result);
                        Console.WriteLine($"\tIN-CODE: Result: {result}");
                        return null;
                    },
                    new BaseFormProcessor<int, (int, int)>(
                        initialStateGetter: () => default,
                        conversionFunction: state =>
                        {
                            Console.WriteLine($"\tIN-CODE: State: {state}");
                            return Result<int, string>.Ok(joinNumbers(state.Item1, state.Item2));
                        },
                        inputHandlers: new Func<Func<(int, int)>, InputProcessor<(int, int)>>[]
                        {
                            ProcessorModifier<(int, int), int>.CreateInfallibleInstanceGetter(
                                (state, v) => (v, default),
                                new UnsignedInt32Processor(() => "Item1: ")),
                            ProcessorModifier<(int, int), int>.CreateInfallibleInstanceGetter(
                                (state, v) => (state.Item1, v),
                                new UnsignedInt32Processor(() => "Item2: "))
                        }))) {}
        }

        private static int joinNumbers(int a, int b) =>
            a * (int)System.Math.Pow(10, ((int)System.Math.Log10(b) + 1)) + b;

        private class BasicState : State
        {
            public override string GetDefaultResponse() => "What do you want to do?";

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public override (State, string) ProcessMessage(string id, string msg) =>
                (this, $"Message sent: {msg}");
        }
    }
}
