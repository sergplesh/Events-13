using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeometrucShapeCarLibrary;

namespace Events_лаб_13
{
    public delegate void CollectionHandler<T>(object source, CollectionHandlerEventArgs args);  // делегат для события
    public class MyObservableCollection<T> : MyCollection<T> where T : IInit, ICloneable, IComparable, new()
    {
        public string CollectionName { get; set; } // название коллекции

        /// <summary>
        /// Конструктор с параметром (название коллекции)
        /// </summary>
        /// <param name="name">название коллекции</param>
        public MyObservableCollection(string name) //
        {
            CollectionName = name;
        }
        public event CollectionHandler<T> CollectionCountChange; //Событие при изменении количества элементов
        public event CollectionHandler<T> CollectionReferenceChange; //Событие при изменении ссылки
        public void OnCollectionCountChange(object source, CollectionHandlerEventArgs args) // генерация события изменения количества элементов
        {
            CollectionCountChange?.Invoke(this, args); // вызов события
        }
        public void OnCollectionReferenceChange(object source, CollectionHandlerEventArgs args) // генерация события изменения ссылки
        {
            CollectionReferenceChange?.Invoke(this, args); // вызов события
        }
        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="item">элемент</param>
        public void Add(T item)
        {
            base.Add(item); // добавление элемента в коллекцию 
            OnCollectionCountChange(this, new CollectionHandlerEventArgs("добавлен элемент", (T)item.Clone()));
        }

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="item">элемент</param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            bool remove = base.Remove(item); //Удаление элемента из коллекции
            if (remove)
                OnCollectionCountChange(this, new CollectionHandlerEventArgs("удален элемент", (T)item.Clone()));
            return remove;
        }
    }
}
