using System;

namespace MissingLinq
{
  public class ObservableListEventArgs<T> : EventArgs
  {
    public ObservableListEventArgs(T element)
    {
      Element = element;
    }

    public T Element { get; private set; }
  }
}