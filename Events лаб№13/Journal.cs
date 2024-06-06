using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometrucShapeCarLibrary;

namespace Events_лаб_13
{
    /// <summary>
    /// Журнал с записями
    /// </summary>
    /// <typeparam name="T">обобщённый тип</typeparam>
    public class Journal<T> where T : IInit, ICloneable, IComparable, new()
    {
        /// <summary>
        /// Список записей
        /// </summary>
        List<JournalEntry> journal = new List<JournalEntry>();
        public int Count => journal.Count;
        public void WriteRecord(object source, CollectionHandlerEventArgs args) //Обработчик событий
        {
            MyObservableCollection<T>? collection = source as MyObservableCollection<T>; // приведение source к типу MyObservableCollection
            string? collectionName = collection?.CollectionName;
            journal.Add(new JournalEntry(collectionName, args.ChangeType, args.Item.ToString()));
        }
        public void PrintJournal()
        {
            if (journal.Count == 0)
            {
                Console.WriteLine("Журнал пустой");
                return;
            }
            foreach (JournalEntry item in journal)
            {
                Console.WriteLine(item);
            }
        }
    }
}
