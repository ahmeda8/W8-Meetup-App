using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Xml.Linq;

using W8_Meetup_App.Common;

namespace W8_Meetup_App.Data
{
    class MeetupDataSource : BindableBase
    {
        String MeetupApiKey = "4b7e583f70562c7f3145724f1c5b2568";
        String MeetUpHost = "https://api.meetup.com";
        public ObservableCollection<MeetupDataGroup> AllGroups = new ObservableCollection<MeetupDataGroup>();
        public IEnumerable<MeetupDataGroup> GetOpenEvents()
        {
            
            string apiurl = this.MeetUpHost + "/2/open_events?"+"key="+this.MeetupApiKey+"&sign=true&zip=90815&radius=25&page=20&format=xml";
            XDocument xdoc = XDocument.Load(apiurl);
            AllGroups.Add(new MeetupDataGroup("0", "AllGroups", "subtitle", "nono", "nono"));
            //var items = from item in xdoc.Root.Elements("items")
            //            select new OpenEvent
            //            {
            //                Name = (string)item.Element("name")
            //            };
            //var items = xdoc.Root.Elements("items");
            foreach(XElement item in xdoc.Root.Elements("items").Elements("item"))
            {
                AllGroups.First().Items.Add(new OpenEvent{Title = (string)item.Element("name"),
                Subtitle = (string)item.Element("group").Element("name")});
            }

            return AllGroups;
        }
    }

    class MeetupDataGroup
    {
        public string Title;
        public string Subtitle;
        public string Desc;
        private object GroupTypeClass;

        public MeetupDataGroup(String uniqueId, String title, String subtitle, String imagePath, String description)
        {
            Title = title;
            Subtitle = subtitle;
            Desc = description;
            Items.CollectionChanged += ItemsCollectionChanged;
        }

        private void ItemsCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // Provides a subset of the full items collection to bind to from a GroupedItemsPage
            // for two reasons: GridView will not virtualize large items collections, and it
            // improves the user experience when browsing through groups with large numbers of
            // items.
            //
            // A maximum of 12 items are displayed because it results in filled grid columns
            // whether there are 1, 2, 3, 4, or 6 rows displayed

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex,Items[e.NewStartingIndex]);
                        if (TopItems.Count > 12)
                        {
                            TopItems.RemoveAt(12);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Move:
                    if (e.OldStartingIndex < 12 && e.NewStartingIndex < 12)
                    {
                        TopItems.Move(e.OldStartingIndex, e.NewStartingIndex);
                    }
                    else if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        TopItems.Add(Items[11]);
                    }
                    else if (e.NewStartingIndex < 12)
                    {
                        TopItems.Insert(e.NewStartingIndex, Items[e.NewStartingIndex]);
                        TopItems.RemoveAt(12);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems.RemoveAt(e.OldStartingIndex);
                        if (Items.Count >= 12)
                        {
                            TopItems.Add(Items[11]);
                        }
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    if (e.OldStartingIndex < 12)
                    {
                        TopItems[e.OldStartingIndex] = Items[e.OldStartingIndex];
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    TopItems.Clear();
                    while (TopItems.Count < Items.Count && TopItems.Count < 12)
                    {
                        TopItems.Add(Items[TopItems.Count]);
                    }
                    break;
            }
        }

        private ObservableCollection<object> _items = new ObservableCollection<object>();
        public ObservableCollection<object> Items
        {
            get { return this._items; }
        }

        private ObservableCollection<object> _topItem = new ObservableCollection<object>();
        public ObservableCollection<object> TopItems
        {
            get {return this._topItem; }
        }
    }
}
