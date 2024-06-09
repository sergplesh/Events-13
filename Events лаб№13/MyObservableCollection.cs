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
            base.Add(item); // добавление элемента в коллекцию 
            OnCollectionCountChanged(this, new CollectionHandlerEventArgs("добавлен элемент", (T)item.Clone()));  // вызов события изменения количества объектов в коллекции
        }

        /// <summary>
        /// Удаление элемента
        /// </summary>
        /// <param name="item">элемент</param>
        /// <returns></returns>
        public new bool Remove(T item)
        {
            bool remove = base.Remove(item); //Удаление элемента из коллекции
            if (remove)
                OnCollectionCountChanged(this, new CollectionHandlerEventArgs("удален элемент", (T)item.Clone())); // вызов события изменения количества объектов в коллекции
            return remove; // возвращаем результат удаления
        }

        /// <summary>
        /// Индексатор для коллекции
        /// </summary>
        /// <param name="index">индекс элемента</param>
        /// <returns>элемент по индексу</returns>
        /// <exception cref="IndexOutOfRangeException">выход за пределы индексирования</exception>
        public T this[int index]
        {
            get
            {
                if (index >= 0 && index < table.Length) // заданный индекс находится в пределах индексирования
                    return table[index]; // возвращаем значение элемента по индексу
                else throw new IndexOutOfRangeException("Индекс выходит за пределы коллекции"); // вышли за пределы индексации
            }
            set
            {
                if (index >= 0 && index < table.Length) // заданный индекс находится в пределах индексирования
                {
                    table[index] = value; // меняем ссылку на другой объект
                    OnCollectionReferenceChanged(this, new CollectionHandlerEventArgs("изменён элемент на", (T)value.Clone())); // вызов события изменения ссылки на другой объект
                }
                else throw new IndexOutOfRangeException("Индекс выходит за пределы коллекции"); // вышли за пределы индексации
            }
        }
    }
}
