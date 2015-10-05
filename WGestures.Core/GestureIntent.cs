using System;
using WGestures.Core.Commands;

namespace WGestures.Core
{
    /// <summary>
    /// Gesture + Command + Context
    /// </summary>
    [Serializable]
    public class GestureIntent
    {
        public class ExecutionResult
        {
            public Exception Exception { get; private set; }
            public bool IsOk { get; private set; }

            public ExecutionResult(Exception e, bool isOk)
            {
                this.Exception = e;
                this.IsOk = isOk;
            }
        }

        public Gesture Gesture { get; set; }
        public AbstractCommand Command { get; set; }

        public bool ExecuteOnModifier { get; set; }

        public bool CanExecuteOnModifier()
        {
            return Gesture.Modifier != GestureModifier.None && ExecuteOnModifier;
        }


        public string Name { get; set; }

        public ExecutionResult Execute(GestureContext context, GestureParser gestureParser)
        {
            //Aware接口依赖注入
            var contextAware = Command as IGestureContextAware;
            if (contextAware != null) contextAware.Context = context;

            var parserAware = this.Command as IGestureParserAware;
            if (parserAware != null) parserAware.Parser = gestureParser;

            var shouldInit = this.Command as INeedInit;
            if (shouldInit != null && !shouldInit.IsInitialized)
            {
                shouldInit.Init();
            }
            
            //在独立线程中运行
            //new Thread似乎反应快一点，ThreadPool似乎有延迟
            //ThreadPool.QueueUserWorkItem((s) =>
            //new Thread(() =>
            {
                try
                {
                    context.ActivateTargetWindow();
                    Command.Execute();
                }
                catch (Exception)
                {
                    //ignore errors for now
#if DEBUG
                    throw;
#endif
                }
            }//) { IsBackground = false}.Start();

            return new ExecutionResult(null, true);

        }
    }
}
