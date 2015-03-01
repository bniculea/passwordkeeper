
using System.Linq;
using CompletePasswordManager.DataModel;
using CompletePasswordManager.DataStructures;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace UnitTestApp1
{
    [TestClass]
    public class DeleteFromObservableRangeCollectionTests
    {
        [TestMethod]
        public void DeleteItem_CountIsLess()
        {
            ObservableRangeCollection<Entry> entries  = new ObservableRangeCollection<Entry>();
            entries.Add(new Entry(){Name = "Bogdan", Password = "Dinassad"});
            entries.Add(new Entry(){Name = "1Bogdan", Password = "Dinassad"});
            entries.Add(new Entry(){Name = "ABogdan", Password = "Dinassad"});
            entries.Add(new Entry(){Name = "BZogdan", Password = "Dinassad"});
            entries.Add(new Entry(){Name = "Cogdan", Password = "Dinassad"});
            entries.Add(new Entry(){Name = "FBogdan", Password = "Dinassad"});
            entries.Add(new Entry(){Name = "WBogdan", Password = "Dinassad"});


            Entry entry = new Entry() {Name = "Bogdan", Password = "Dinassad"};
            
            int index = entries.IndexOf(entry);
            entries.RemoveAt(index);

            int expected = 6;
            int actual = entries.Count;

            Assert.AreEqual(expected, actual);
        }
    }
}
