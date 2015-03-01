using SQLite;
using System;
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


        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            Entry entry = obj as Entry;
            if ((System.Object)entry == null)
            {
                return false;
            }
            return (Name.Equals(entry.Name)) && (Password.Equals(entry.Password));
        }

        public override int GetHashCode()
        {
            int hashcode = 13;
            hashcode += Name.GetHashCode();
            hashcode += Password.GetHashCode();
            return hashcode;
        }

        public static bool operator== (Entry e1, Entry e2)
        {
            if (object.ReferenceEquals(e1, e2)) return true;
            if (((object)e1 == null) || ((object)e2 == null))
            {
                return false;
            }
            return e1.Name.Equals(e2.Name) && e1.Password.Equals(e2.Password);
        }

        public static bool operator !=(Entry e1, Entry e2)
        {
            return !(e1 == e2);
        }
    }
}
