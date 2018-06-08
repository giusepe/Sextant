using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using ReactiveUI;
using Sextant.Abstraction;
using Xamarin.Forms;

namespace Sextant.Navigation
{
    //TODO: Move this to a Sextant.Forms library and implement ReactiveMasterDetailPage<TViewModel>
    public class MasterDetailView : MasterDetailPage, IMasterView
    {
        private readonly IScheduler _backgroundScheduler;
        private readonly IScheduler _mainScheduler;
        private readonly IViewLocator _viewLocator;
        private readonly IObservable<Unit> _appearing;
        private readonly IObservable<Unit> _disappearing;
        private readonly IObservable<IPageViewModel> _onBackButton;
        private readonly IObservable<bool> _isPresented;

        /// <inheritdoc />
        public IObservable<Unit> ViewDisappearing => _disappearing;
        
        /// <inheritdoc />
        public IObservable<Unit> ViewAppearing => _appearing;
        
        /// <inheritdoc />
        public IObservable<bool> Presented => _isPresented;

        public MasterDetailView(IScheduler mainScheduler,
            IScheduler backgroundScheduler,
            IViewLocator viewLocator,
            Page rootPage,
            MasterBehavior behavior = MasterBehavior.Default,
            bool present = false)
        {
            _mainScheduler = mainScheduler;
            _backgroundScheduler = backgroundScheduler;
            _viewLocator = viewLocator;
            Detail = rootPage;
            IsPresented = present;
            MasterBehavior = behavior;

            _onBackButton = Observable
                .FromEventPattern<BackButtonPressedEventArgs>(x => BackButtonPressed += x, x => BackButtonPressed -= x)
                .Select(x => (IPageViewModel) x.Sender);

            _isPresented = Observable.FromEventPattern(x => IsPresentedChanged += x, x => IsPresentedChanged -= x)
                .Where(x => x.Sender is MasterDetailPage)
                .Select(x => ((MasterDetailPage) x.Sender).IsPresented);
            
            _appearing = Observable.FromEventPattern(x => Appearing += x, x => Appearing -= x).ToSignal();
            _disappearing = Observable.FromEventPattern(x => Disappearing += x, x => Disappearing -= x).ToSignal();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = Navigation.NavigationStack.Last().BindingContext as IViewLifeCycle;

            viewModel?.Init().SubscribeSafe(); //TODO: Determine how to pass values

            viewModel?.ViewAppearing().SubscribeSafe();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            var viewModel = Navigation.NavigationStack.Last().BindingContext as IViewLifeCycle;

            viewModel?.ViewDisappearing().SubscribeSafe();
        }

        public IObservable<Unit> BackPressed => _onBackButton.ToSignal();

        public IDetailView DetailView { get; set; }
    }
}