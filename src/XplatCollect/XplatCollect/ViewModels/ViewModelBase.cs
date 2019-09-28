using Prism;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XplatCollect.ViewModels
{
    public abstract class ViewModelBase : BindableBase, IInitializeAsync, IActiveAware
    {
        protected readonly INavigationService navigationService;
        protected readonly IPageDialogService pageDialogService;

        protected ViewModelBase(INavigationService navigationService
            , IPageDialogService pageDialogService)
        {
            this.navigationService = navigationService;
            this.pageDialogService = pageDialogService;
            IsActiveChanged += OnIsActiveChanged;
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        public bool IsNotBusy => !IsBusy;

        public event EventHandler IsActiveChanged;

        private bool isActive;
        public bool IsActive
        {
            get { return isActive; }
            set { SetProperty(ref isActive, value, RaiseIsActiveChanged); }
        }

        private void RaiseIsActiveChanged()
            => IsActiveChanged?.Invoke(this, EventArgs.Empty);

        private async void OnIsActiveChanged(object sender, EventArgs e)
        {
            if (IsActive)
            {
                await OnTabActive();
            }
        }

        protected virtual Task OnTabActive()
            => Task.CompletedTask;

        public virtual Task InitializeAsync(INavigationParameters parameters)
            => Task.CompletedTask;

        protected async Task ExecuteBusyAction(Func<Task> theBusyAction)
        {
            try
            {
                if (IsBusy)
                    return;

                IsBusy = true;

                await theBusyAction();
            }
            catch (Exception ex)
            {
#if DEBUG
                System.Diagnostics.Debug.WriteLine(ex);
#endif
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}
