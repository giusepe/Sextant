using Sextant.Abstraction;

namespace Sextant.Tests
{
    internal class FrameViewModelMock : IPageViewModel
    {
        private readonly string _id;

        public string Id => _id ?? nameof(FrameViewModelMock);

        public FrameViewModelMock(string id = null)
        {
            _id = id;
        }
    }

    internal class PageViewModelMock : IPageViewModel
    {
        private readonly string _id;

        public string Id => _id ?? nameof(PageViewModelMock);

        public PageViewModelMock(string id = null)
        {
            _id = id;
        }
    }
}