using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events_лаб_13
{
    /// <summary>
    /// Данные, передаваемые в событие (обёртка-конверт для данных)
    /// </summary>
    public class CollectionHandlerEventArgs : EventArgs
    {
        /// <summary>
        /// тип изменения в коллекции
        /// </summary>
        public string ChangeType { get; set; } // тип изменения
        /// <summary>
        /// ссылка на обьект
        /// </summary>
        public object Item { get; set; } // ссылка на обьект

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="changeType">тип изменения в коллекции</param>
        /// <param name="item">ссылка на обьект,</param>
        public CollectionHandlerEventArgs(string changeType, object item) 
        {
            ChangeType = changeType; // тип изменения
            Item = item; // ссылка на обьект
        }
    }
}
