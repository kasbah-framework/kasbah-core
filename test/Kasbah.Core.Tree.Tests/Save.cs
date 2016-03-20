using Moq;
using Xunit;

namespace Kasbah.Core.Tree.Tests
{
    public class Save
    {
        #region Public Methods

        [Fact]
        public void Noop()
        {
            var service = new TreeService(Mock.Of<ITreeProvider>());
        }

        #endregion
    }
}