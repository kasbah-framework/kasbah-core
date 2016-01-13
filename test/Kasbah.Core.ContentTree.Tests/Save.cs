using Moq;
using Xunit;

namespace Kasbah.Core.ContentTree.Tests
{
    public class Save
    {
        #region Public Methods

        [Fact]
        public void Noop()
        {
            var service = new ContentTreeService(Mock.Of<IContentTreeProvider>());
        }

        #endregion
    }
}