﻿// Copyright (c) 2019 .NET Foundation and Contributors. All rights reserved.
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Sextant.Tests
{
    /// <summary>
    /// A mock of a page view model.
    /// </summary>
    internal class PageViewModelMock : IPageViewModel
    {
        private readonly string _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageViewModelMock"/> class.
        /// </summary>
        /// <param name="id">The id of the page.</param>
        public PageViewModelMock(string id = null)
        {
            _id = id;
        }

        /// <summary>
        /// Gets the mock.
        /// </summary>
        public static PageViewModelMock Mock { get; } = new PageViewModelMock();

        /// <summary>
        /// Gets the ID of the page.
        /// </summary>
        public string Id => _id ?? nameof(PageViewModelMock);
    }
}
