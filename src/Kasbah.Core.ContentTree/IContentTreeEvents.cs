using System;
using Kasbah.Core.Models;

namespace Kasbah.Core.ContentTree
{
	public interface IContentTreeEvents
	{
        event EventHandler<ItemBase> OnItemSaved;
	}
}