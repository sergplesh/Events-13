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
        /// <summary>
        /// название коллекции
        /// </summary>
        public string CollectionName { get; set; }
        /// <summary>
        /// тип изменения
        /// </summary>
        public string ChangeType { get; set; }
        /// <summary>
        /// данные объекта
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// Конструктор с параметрами для журнальной записи
        /// </summary>
        /// <param name="collectionName">название коллекции</param>
        /// <param name="changeType">тип изменения в коллекции</param>
        /// <param name="data">данные объекта, который участвовал в изменении</param>
        public JournalEntry(string collectionName, string changeType, string data)
        {
            CollectionName = collectionName; // название коллекции
            ChangeType = changeType; // тип изменения в коллекции
            Data = data; // данные объекта, который участвовал в изменении
        }
        /// <summary>
        /// Печать журнальной записи
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Data == null ? "" : $"В коллекции {CollectionName} {ChangeType}. Обьект: {Data}";
        }
    }
}
