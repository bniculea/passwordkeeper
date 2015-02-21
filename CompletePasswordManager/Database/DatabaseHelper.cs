using CompletePasswordManager.DataModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompletePasswordManager.Database
{
    public class DatabaseCreator
    {
        private string DatabaseName { get; set; }
        private SQLiteAsyncConnection Connection { get; set; }
        public DatabaseCreator(string databaseName, SQLiteAsyncConnection connection)
        {
            DatabaseName = databaseName;
            Connection = connection;
        }

        //TODO  should not be here, try to receive it using injection
        //public SQLiteAsyncConnection CreateDatabase(string databaseName)
        //{
        //    SQLiteAsyncConnection connection = new SQLiteAsyncConnection(databaseName);
        //    return connection;
        //}

        public async void CreateTableAsync()
        {
           await Connection.CreateTableAsync<Entry>();
        }

        public async void InsertAllAsync(List<Entry> entries)
        {
            await Connection.InsertAllAsync(entries);
        }

        public async void InsertRecordAsync(Entry entry)
        {
            await Connection.InsertAsync(entry);
        }

        public async void DropTableAsync()
        {
          await Connection.DropTableAsync<Entry>();
        }


    }
}
