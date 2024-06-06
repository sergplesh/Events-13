using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events_лаб_13
{
    /// <summary>
    /// Данные, передаваемые в событие (обёртка для данных)
    /// </summary>
    public class CollectionHandlerEventArgs : EventArgs
    {
        public string ChangeType { get; set; } // тип изменения
        public object Item { get; set; } // ссылка на обьект

        /// <summary>
        /// Конструктор с параметрами
        /// </summary>
        /// <param name="changeType"></param>
        /// <param name="item"></param>
        public CollectionHandlerEventArgs(string changeType, object item) 
        {
            ChangeType = changeType; // тип изменения
            Item = item; // ссылка на обьект
        }
    }
}
