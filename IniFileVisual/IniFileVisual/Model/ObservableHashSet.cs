using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace IniFileVisual.Model
{
    public class ObservableHashSet<T> : ObservableCollection<T>
    {
        private readonly HashSet<T> dataSet;

        public ObservableHashSet(IEqualityComparer<T> comparer)
        {
            dataSet = new HashSet<T>(comparer);
        }

        public new void Add(T item)
        {
            if (dataSet.Add(item))
            {
                base.Add(item);
            }
        }

        public new void Remove(T item)
        {
            if (dataSet.Remove(item))
            {
                base.Remove(item);
            }
        }
    }

    public class ToolTipEqualityComparer : IEqualityComparer<Control>
    {
        public bool Equals(Control x, Control y)
        {
            return x != null
                && y != null
                && x.ToolTip.ToString() == y.ToolTip.ToString();
        }

        public int GetHashCode(Control obj)
        {
            return obj.ToolTip.GetHashCode();
        }
    }

}
