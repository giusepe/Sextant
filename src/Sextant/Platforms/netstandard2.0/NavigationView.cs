﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using ReactiveUI;
using Xamarin.Forms;

namespace Sextant
{
    /// <summary>
    /// The main navigation view.
    /// </summary>
    public class NavigationView : NavigationPage, IView
    {
        private readonly IScheduler _backgroundScheduler;
        private readonly IScheduler _mainScheduler;
        private readonly IViewLocator _viewLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationView"/> class.
        /// </summary>
        /// <param name="mainScheduler">The main scheduler to scheduler UI tasks on.</param>
        /// <param name="backgroundScheduler">The background scheduler.</param>
        /// <param name="viewLocator">The view locator which will find views associated with view models.</param>
        /// <param name="rootPage">The starting root page.</param>
        public NavigationView(IScheduler mainScheduler, IScheduler backgroundScheduler, IViewLocator viewLocator, Page rootPage)
            : base(rootPage)
        {
            _backgroundScheduler = backgroundScheduler;
            _mainScheduler = mainScheduler;
            _viewLocator = viewLocator;

            PagePopped = Observable
                .FromEventPattern<NavigationEventArgs>(x => Popped += x, x => Popped -= x)
                .Select(ep => ep.EventArgs.Page.BindingContext as IPageViewModel)
                .WhereNotNull();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationView"/> class.
        /// </summary>
        /// <param name="mainScheduler">The main scheduler to scheduler UI tasks on.</param>
        /// <param name="backgroundScheduler">The background scheduler.</param>
        /// <param name="viewLocator">The view locator which will find views associated with view models.</param>
        public NavigationView(IScheduler mainScheduler, IScheduler backgroundScheduler, IViewLocator viewLocator)
        {
            _backgroundScheduler = backgroundScheduler;
            _mainScheduler = mainScheduler;
            _viewLocator = viewLocator;

            PagePopped = Observable
                .FromEventPattern<NavigationEventArgs>(x => Popped += x, x => Popped -= x)
                .Select(ep => ep.EventArgs.Page.BindingContext as IPageViewModel)
                .WhereNotNull();
        }

        /// <summary>
        /// Gets an observable which signals when a page is popped.
        /// </summary>
        public IObservable<IPageViewModel> PagePopped { get; }

        /// <summary>
        /// Pops the modal.
        /// </summary>
        /// <returns>An observable that signals when the pop is complete.</returns>
        public IObservable<Unit> PopModal() =>
            Navigation
                .PopModalAsync()
                .ToObservable()
                .ToSignal()
                .ObserveOn(_mainScheduler); // XF completes the pop operation on a background thread :/

        /// <summary>
        /// Pops the page.
        /// </summary>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns>An observable that signals when the pop is complete.</returns>
        public IObservable<Unit> PopPage(bool animate) =>
            Navigation
                .PopAsync(animate)
                .ToObservable()
                .ToSignal()
                .ObserveOn(_mainScheduler); // XF completes the pop operation on a background thread :/

        /// <summary>
        /// Pops to root page.
        /// </summary>
        /// <returns>The to root page.</returns>
        /// <param name="animate">If set to <c>true</c> animate.</param>
        public IObservable<Unit> PopToRootPage(bool animate) =>
             Navigation
                .PopToRootAsync(animate)
                .ToObservable()
                .ToSignal()
                .ObserveOn(_mainScheduler);

        /// <summary>
        /// Pushes the modal.
        /// </summary>
        /// <param name="modalViewModel">The modal view model.</param>
        /// <param name="contract">The contract.</param>
        /// <returns>An observable which signals when the push is complete.</returns>
        public IObservable<Unit> PushModal(IPageViewModel modalViewModel, string contract) =>
            Observable
                .Start(
                    () =>
                    {
                        var page = LocatePageFor(modalViewModel, contract);
                        SetPageTitle(page, modalViewModel.Id);

                        var navigation = LocateNavigationFor(modalViewModel);
                        navigation.PushPage(modalViewModel, contract, true, false).Subscribe();

                        return navigation as NavigationPage;
                    },
                    CurrentThreadScheduler.Instance)
                .ObserveOn(CurrentThreadScheduler.Instance)
                .SelectMany(
                    page =>
                        Navigation
                            .PushModalAsync(page)
                            .ToObservable());

        /// <summary>
        /// Pushes the page.
        /// </summary>
        /// <param name="pageViewModel">The page view model.</param>
        /// <param name="contract">The contract.</param>
        /// <param name="resetStack">if set to <c>true</c> [reset stack].</param>
        /// <param name="animate">if set to <c>true</c> [animate].</param>
        /// <returns>An observable which signals when the push is complete.</returns>
        public IObservable<Unit> PushPage(
            IPageViewModel pageViewModel,
            string contract,
            bool resetStack,
            bool animate) =>
            Observable
                .Start(
                    () =>
                    {
                        var page = LocatePageFor(pageViewModel, contract);
                        SetPageTitle(page, pageViewModel.Id);
                        return page;
                    },
                    CurrentThreadScheduler.Instance)
                .ObserveOn(CurrentThreadScheduler.Instance)
                .SelectMany(
                    page =>
                    {
                        if (resetStack)
                        {
                            if (Navigation.NavigationStack.Count == 0)
                            {
                                return Navigation.PushAsync(page, false).ToObservable();
                            }

                            // XF does not allow us to pop to a new root page. Instead, we need to inject the new root page and then pop to it.
                            Navigation
                                .InsertPageBefore(page, Navigation.NavigationStack[0]);

                            return Navigation
                                .PopToRootAsync(false)
                                .ToObservable();
                        }

                        return Navigation
                            .PushAsync(page, animate)
                            .ToObservable();
                    });

        private IView LocateNavigationFor(IPageViewModel viewModel)
        {
            var view = _viewLocator.ResolveView(viewModel, "NavigationView");
            var navigationPage = view as IView;

            if (navigationPage is null)
            {
                Debug.WriteLine($"No navigation view could be located for type '{viewModel.GetType().FullName}', using the default navigation page.");
                navigationPage = new NavigationView(_mainScheduler, _backgroundScheduler, _viewLocator);
            }

            return navigationPage;
        }

        private Page LocatePageFor(object viewModel, string contract)
        {
            var view = _viewLocator.ResolveView(viewModel, contract);
            var page = view as Page;

            if (view == null)
            {
                throw new InvalidOperationException($"No view could be located for type '{viewModel.GetType().FullName}', contract '{contract}'. Be sure Splat has an appropriate registration.");
            }

            if (view == null)
            {
                throw new InvalidOperationException($"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' does not implement IViewFor.");
            }

            if (page == null)
            {
                throw new InvalidOperationException($"Resolved view '{view.GetType().FullName}' for type '{viewModel.GetType().FullName}', contract '{contract}' is not a Page.");
            }

            view.ViewModel = viewModel;

            return page;
        }

        private void SetPageTitle(Page page, string resourceKey)
        {
            // var title = Localize.GetString(resourceKey);
            // TODO: ensure resourceKey isn't null and is localized.
            page.Title = resourceKey;
        }
    }
}
