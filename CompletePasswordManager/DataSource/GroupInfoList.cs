using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CompletePasswordManager.DataStructures;

namespace CompletePasswordManager.DataSource
{
    public class GroupInfoList<T> : ObservableRangeCollection<object>
    {

        public object Key { get; set; }

        public new IEnumerator<object> GetEnumerator()
        {
            return (System.Collections.Generic.IEnumerator<object>)base.GetEnumerator();
        }

  
    }
}
