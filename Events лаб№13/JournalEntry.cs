using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events_лаб_13
{
    /// <summary>
    /// Запись для журнала
    /// </summary>
    public class JournalEntry
    {
        public string CollectionName { get; set; } // название коллекции
        public string ChangeType { get; set; } // тип изменения
        public string Data { get; set; } // данные объекта

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="collectionName"></param>
        /// <param name="changeType"></param>
        /// <param name="data"></param>
        public JournalEntry(string collectionName, string changeType, string data)
        {
            CollectionName = collectionName; // название коллекции
            ChangeType = changeType; // тип изменения
            Data = data; // данные объекта
        }
        public override string ToString()
        {
            return $"В коллекции {CollectionName} {ChangeType}. Обьект: {Data}";
        }
    }
}
