﻿using CompletePasswordManager.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CompletePasswordManager.DataSource;
using CompletePasswordManager.DataStructures;

namespace CompletePasswordManager.Repository
{
    public class EntryRepository
    {
        private ObservableRangeCollection<Entry> _collection = new ObservableRangeCollection<Entry>();
        public ObservableRangeCollection<Entry> Collection
        {
            get { return _collection;}
        }

        public EntryRepository()
        {
        }

        private ObservableRangeCollection<GroupInfoList<object>> _groupsByLetter = null;

        public ObservableRangeCollection<GroupInfoList<object>> GetGroupsByLetter
        {
            get {
                return SetGroupsByLetter();
            }
        }

        private ObservableRangeCollection<GroupInfoList<object>> SetGroupsByLetter()
        {
            if (_groupsByLetter == null)
            {
                _groupsByLetter = new ObservableRangeCollection<GroupInfoList<object>>();
                var query = from item in Collection
                    orderby ((Entry) item).Name
                    group item by ((Entry) item).Name.ToUpper()[0]
                    into g
                    select new {GroupName = (IsNotLetter(g.Key) ? '#' : g.Key), Items = g};
                foreach (var g in query)
                {
                    GroupInfoList<object> info = new GroupInfoList<object>();
                    info.Key = g.GroupName;
                    foreach (var item in g.Items)
                    {
                        info.Add(item);
                    }
                    //_groupsByLetter.Add(info);
                    AddOrUpdateExistent(_groupsByLetter, info);
                }
            }
            return _groupsByLetter;
        }

        private void AddOrUpdateExistent(ObservableRangeCollection<GroupInfoList<object>> _groupsByLetter, GroupInfoList<object> info)
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

        private ObservableRangeCollection<HeaderItem> _passwordHeaders = null;
        public ObservableRangeCollection<HeaderItem> PasswordHeaders
        {
            get
            {
                if (_passwordHeaders == null)
                {
                    _passwordHeaders = new ObservableRangeCollection<HeaderItem>();
                    for (int i = 65; i <=90;i++)
                    {
                        char c = (char)i;
                        if (this._groupsByLetter.Any(k => ((char)k.Key) == c))
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
                    if (this._groupsByLetter.Any(k => ((char)k.Key) == '#'))
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

        public void UpdateRepository()
        {
            _groupsByLetter = null;
            _passwordHeaders = null;
            SetGroupsByLetter();
        }
    }
}
