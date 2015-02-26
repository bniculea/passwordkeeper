using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompletePasswordManager.DataModel
{
    public class Entry
    {
        [PrimaryKey, AutoIncrement, Column("Id")]
        public int Id { get; set; }
        [Column("Name")]
        public String Name { get; set; }
        [Column("Password")]
        public String Password { get; set; }

    }
}
