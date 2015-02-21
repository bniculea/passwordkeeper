using CompletePasswordManager.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompletePasswordManager.DataSource;

namespace CompletePasswordManager.Repository
{
    public class EntryRepository
    {
        private ObservableCollection<Entry> _collection = new ObservableCollection<Entry>();
        public ObservableCollection<Entry> Collection
        {
            get { return _collection;}
        }

        public EntryRepository()
        {
            
        }

        internal List<GroupInfoList<object>> GetGroupsByLetter()
        {
            List<GroupInfoList<object>> groups = new List<GroupInfoList<object>>();
            var query = from entry in Collection
                        orderby ((Entry)entry).Name
                        group entry by ((Entry)entry).Name[0] into g
                        select new { GroupName = g.Key, Items = g };
            foreach(var g in query)
            {
                GroupInfoList<object> info = new GroupInfoList<object>();
                info.Key = g.GroupName;
                foreach(var item in g.Items)
                {
                    info.Add(item);
                }
                groups.Add(info);
            }
            return groups;
        }
    }
}
