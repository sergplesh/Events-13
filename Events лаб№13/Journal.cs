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
        /// <summary>
        /// Количество записей в журнале
        /// </summary>
        public int Count => journal.Count;

        /// <summary>
        /// Создание записи в журнале
        /// </summary>
        /// <param name="source">ссылка на объект, генерирующий событие</param>
        /// <param name="args">данные для обработки события</param>
        public void WriteRecord(object source, CollectionHandlerEventArgs args) //Обработчик событий
        {
            MyObservableCollection<T>? collection = source as MyObservableCollection<T>; // приведение source к типу MyObservableCollection
            string collectionName = collection.CollectionName; // достаём название коллекции
            journal.Add(new JournalEntry(collectionName, args.ChangeType, args.Item.ToString()));
        }

        /// <summary>
        /// Печать журнала
        /// </summary>
        public void PrintJournal()
        {
            if (journal.Count == 0) // если в журнале нет записей
            {
                Console.WriteLine("Журнал пустой"); // выводим сообщение об этом
                return; // выходим из печати
            }
            foreach (JournalEntry item in journal) // проходим по записям в журнале
            {
                Console.WriteLine(item); // печатаем запись
            }
        }
    }
}
