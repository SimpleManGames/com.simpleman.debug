namespace Simplicity.Debug.Runtime
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class AllocCounter
    {
        private UnityEngine.Profiling.Recorder _recorder;
        
        private readonly string _className;
        
        private readonly string _memberName;

        public AllocCounter(string className, [CallerMemberName] string memberName = "")
        {
            _className = className;
            _memberName = memberName;
            _recorder = UnityEngine.Profiling.Recorder.Get("GC.Alloc");
            _recorder.enabled = false;

#if !UNITY_WEBGL
            _recorder.FilterToCurrentThread();
#endif

            _recorder.enabled = true;
        }

        ~AllocCounter()
        {
            if (_recorder == null)
                throw new InvalidOperationException("AllocCounter was not started.");

            _recorder.enabled = false;

#if !UNITY_WEBGL
            _recorder.CollectFromAllThreads();
#endif

            int result = _recorder.sampleBlockCount;
            _recorder = null;

            LogAllocCounterToConsole(result);
        }
        
        [Conditional("DEBUG_ALLOC_COUNTER_AMOUNT")]
        private void LogAllocCounterToConsole(int allocCount)
        {
            UnityEngine.Debug.Log($"{_className}::{_memberName} AllocCounter: allocCount={allocCount}");
        }
    }
}
