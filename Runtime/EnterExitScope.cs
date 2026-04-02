namespace Simplicity.Debug.Runtime
{
    using System;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;

    public sealed class EnterExitScope : IDisposable
    {
        private readonly string _className;
        
        private readonly string _memberName;
        
        private bool _isDisposed;

        public EnterExitScope(string className, [CallerMemberName] string memberName = "")
        {
            _className = className;
            _memberName = memberName;
            Log("Enter");
        }
        
        ~EnterExitScope()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_isDisposed)
                return;

            Log("Exit");
            _isDisposed = true;
        }

        [Conditional("DEBUG_ENTER_EXIT_SCOPE")]
        private void Log(string point)
        {
            UnityEngine.Debug.Log($"[{_className}.{_memberName}] {point}");
        }
    }
}