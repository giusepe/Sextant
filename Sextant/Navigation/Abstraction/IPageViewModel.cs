namespace Sextant.Abstraction
{
    /// <summary>
    /// Interface that defines a view model for a page for the navigation stack.
    /// </summary>
    public interface IPageViewModel
    {
        /// <summary>
        /// Gets the identifier for this instance.
        /// </summary>
        string Id
        {
            get;
        }
    }
}