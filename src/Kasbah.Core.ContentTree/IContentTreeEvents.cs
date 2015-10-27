using System;

namespace Kasbah.Core.ContentTree
{
	public interface IContentTreeEvents
	{
		delegate void OnItemSaved(object sender, ItemSavingEventsArgs e);	
	}
	
	public class ItemSavingEventsArgs {
		public ItemBase Item { get; set; }
	}
	
}