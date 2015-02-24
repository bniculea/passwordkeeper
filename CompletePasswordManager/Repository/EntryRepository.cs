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


        private List<GroupInfoList<object>> _groupsByLetter = null;

        public List<GroupInfoList<object>> GetGroupsByLetter
        {
            get
            {
                if (_groupsByLetter == null)
                {
                    _groupsByLetter = new List<GroupInfoList<object>>();
                    var query = from item in Collection
                                orderby ((Entry)item).Name
                                group item by ((Entry)item).Name.ToUpper()[0] into g
                                select new {GroupName= ( IsNotLetter(g.Key) ? '#' : g.Key), Items = g};
                    foreach (var g in query)
                    {
                        GroupInfoList<object> info = new GroupInfoList<object>();
                        info.Key = g.GroupName;
                        foreach(var item in g.Items)
                        {
                            info.Add(item);
                        }
                        //_groupsByLetter.Add(info);
                        AddOrUpdateExistent(_groupsByLetter, info);
                    }
                }
                return _groupsByLetter;
            }
        }

        private void AddOrUpdateExistent(List<GroupInfoList<object>> _groupsByLetter, GroupInfoList<object> info)
        {
            
            // TODO convert to linq
            foreach (GroupInfoList<object> gr in _groupsByLetter)
            {
                if (gr.Key.Equals(info.Key))
                {
                    gr.AddRange(info);
                    return;
                }
            }
            _groupsByLetter.Add(info);
        }

        private List<HeaderItem> _passwordHeaders = null;
        public List<HeaderItem> PasswordHeaders
        {
            get
            {
                if (_passwordHeaders == null)
                {
                    _passwordHeaders = new List<HeaderItem>();
                    for (int i = 65; i <=90;i++)
                    {
                        char c = (char)i;
                        if (this._groupsByLetter.Exists(k => ((char)k.Key) == c))
                        {
                            _passwordHeaders.Add(new HeaderItem() { HeaderName = c, IsEnabled = true });
                        }
                        else
                        {
                            _passwordHeaders.Add(new HeaderItem() { HeaderName = c, IsEnabled = false });
                        }
                    }
                    // insert # for numbers at the front

                   // _passwordHeaders.Insert(0, new HeaderItem() { HeaderName = '#', IsEnabled = false });
                    if (this._groupsByLetter.Exists(k => ((char)k.Key) == '#'))
                    {
                        _passwordHeaders.Insert(0, new HeaderItem() { HeaderName = '#', IsEnabled = true });
                    }
                    else
                    {
                        _passwordHeaders.Insert(0, new HeaderItem() { HeaderName = '#', IsEnabled = false });
                    }
                }
                return _passwordHeaders;
            }
        }

        private bool IsNotLetter(char c)
        {
            return !Char.IsLetter(c);
        }
    }
}
