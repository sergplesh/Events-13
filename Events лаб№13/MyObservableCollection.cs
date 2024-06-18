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
    /// Делегат для создаваемых нами событий для коллекции
    /// </summary>
    /// <typeparam name="T">Обобщённый тип данных</typeparam>
    /// <param name="source">ссылка на объект, генерирующий событие</param>
    /// <param name="args">данные для обработки события</param>
    public delegate void CollectionHandler<T>(object source, CollectionHandlerEventArgs args);  // делегат для событий

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MyObservableCollection<T> : MyCollection<T> where T : IInit, ICloneable, IComparable, new()
    {
        public string CollectionName { get; set; } // название коллекции

        /// <summary>
        /// Конструктор с параметром (название коллекции)
        /// </summary>
        /// <param name="name">название коллекции</param>
        public MyObservableCollection(string name, int length, double fillRatio = 0.72): base(length, fillRatio)
        {
            CollectionName = name;
        }

        /// <summary>
        /// Событие при изменении количества элементов
        /// </summary>
        public event CollectionHandler<T>? CollectionCountChanged; //Событие при изменении количества элементов
        /// <summary>
        /// Событие при изменении ссылки
        /// </summary>
        public event CollectionHandler<T>? CollectionReferenceChanged; //Событие при изменении ссылки

        /// <summary>
        /// генерация события изменения количества элементов
        /// </summary>
        /// <param name="source">ссылка на объект, генерирующий событие</param>
        /// <param name="args">данные для обработки события</param>
        public void OnCollectionCountChanged(object source, CollectionHandlerEventArgs args) // генерация события изменения количества элементов
        {
            CollectionCountChanged?.Invoke(this, args); // вызов события
        }

        /// <summary>
        /// генерация события изменения ссылки на другой объект
        /// </summary>
        /// <param name="source">ссылка на объект, генерирующий событие</param>
        /// <param name="args">данные для обработки события</param>
        public void OnCollectionReferenceChanged(object source, CollectionHandlerEventArgs args) // генерация события изменения ссылки
        {
            CollectionReferenceChanged?.Invoke(this, args); // вызов события
        }

        /// <summary>
        /// Добавление элемента
        /// </summary>
        /// <param name="item">элемент</param>
        public new void Add(T item)
        {
            bool add = AddItem(item); // добавление элемента в коллекцию 
            if (add) OnCollectionCountChanged(this, new CollectionHandlerEventArgs("добавлен элемент", (T)item.Clone()));  // вызов события изменения количества объектов в коллекции
        }

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="item">удаляемый элемент</param>
        /// <returns></returns>
        public new bool Remove(T item)
        {
            bool remove = base.Remove(item); //Удаление элемента из коллекции
            if (remove)
                OnCollectionCountChanged(this, new CollectionHandlerEventArgs("удален элемент", (T)item.Clone())); // вызов события изменения количества объектов в коллекции
            return remove; // возвращаем результат удаления
        }

        /// <summary>
        /// Индексатор для коллекции.
        /// </summary>
        /// <param name="item">значение искомого злемента.</param>
        /// <returns>Найденный элемент или исключение, если элемент не найден.</returns>
        public T this[T item]
        {
            get // чтение значения элемента коллекции
            {
                int index = FindItem(item); // Поиск индекса
                if (index < 0) // элемента нет в коллекции
                {
                    throw new ArgumentException("Изменяемый элемент не найден в коллекции");
                }
                else return table[index]; // возвращаем значения элемента
            }
            set // установка другого значения элемента коллекции
            {
                if (!Contains(value)) // элемента, на который хотим поменять, ещё нет в коллекции
                {
                    int index = FindItem(item); // Поиск индекса старого элемента
                    if (index >= 0) // изменяемый элемент найден
                    {
                        Remove(table[index]); // удаляем старый элемент
                        Add(value);
                        OnCollectionReferenceChanged(this, new CollectionHandlerEventArgs("изменён элемент", item.Clone())); // вызов события изменения ссылки на другой объект
                    }
                    else // изменяемый элемент не найден
                    {
                        throw new ArgumentException("Изменяемый элемент не найден в коллекции");
                    }
                }
                else // элемент, на который хотим поменять, уже есть в коллекции
                {
                    throw new ArgumentException("Элемент, на который хотите поменять заданный, уже есть в коллекции");
                }
            }
        }
    }
}
