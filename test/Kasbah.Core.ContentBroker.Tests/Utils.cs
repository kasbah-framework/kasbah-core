﻿using Kasbah.Core.Cache;
using Kasbah.Core.Tree;
using Kasbah.Core.Events;
using Kasbah.Core.Index;
using Microsoft.Extensions.Logging;
using Moq;

namespace Kasbah.Core.ContentBroker.Tests
{
    public static class Utils
    {
        #region Public Methods

        public static ContentBroker MockContentBroker()
        {
            return new ContentBroker(new TreeService(Mock.Of<ITreeProvider>()), new IndexService(Mock.Of<IIndexProvider>()), new EventService(Mock.Of<IEventBusProvider>()), Mock.Of<CacheService>(), Mock.Of<ILoggerFactory>());
        }

        #endregion
    }
}