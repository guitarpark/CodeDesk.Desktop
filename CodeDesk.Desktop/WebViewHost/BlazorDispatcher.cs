using Microsoft.AspNetCore.Components;

namespace CodeDesk.Desktop.WebViewHost
{
    internal class BlazorDispatcher : Dispatcher
    {
        IWindow window;
        public BlazorDispatcher(IWindow window)
        {
            this.window = window;
        }
        public override bool CheckAccess() => window.CheckAccess();

        public override async Task InvokeAsync(Action workItem)
        {
            try
            {
                if (CheckAccess())
                    workItem();
                else
                    await window.InvokeAsync(workItem);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override async Task InvokeAsync(Func<Task> workItem)
        {
            try
            {
                if (CheckAccess())
                    await workItem();
                else
                    await window.InvokeAsync(workItem);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<TResult> InvokeAsync<TResult>(Func<TResult> workItem)
        {
            return await InvokeAsync(workItem);
        }

        public override async Task<TResult> InvokeAsync<TResult>(Func<Task<TResult>> workItem)
        {
            return await InvokeAsync(workItem);
        }

    }
}
