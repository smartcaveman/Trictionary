using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Technovation
{
    public struct OrderedPair<T1, T2> : IEquatable<OrderedPair<T1, T2>>, IOrderedEnumerable<object>
    {
        private readonly T1 first;
        private readonly T2 second;

        public OrderedPair(T1 first, T2 second)
        {
            if (ReferenceEquals(first, null)) throw new ArgumentNullException("first");
            if (ReferenceEquals(second, null)) throw new ArgumentNullException("second");
            this.first = first;
            this.second = second;
        }

        public T1 First
        {
            get { return first; }
        }

        public T2 Second
        {
            get { return second; }
        }

        public OrderedPair<T2, T1> Reverse()
        {
            return new OrderedPair<T2, T1>(Second, First);
        }

        public override bool Equals(object obj)
        {
            return !ReferenceEquals(obj, null) &&
                (
                   (obj is IEnumerable && this.SequenceEqual(((IEnumerable)obj).Cast<object>()))
                   ||
                   (obj is KeyValuePair<T1, T2> && this.First.Equals(((KeyValuePair<T1, T2>)(obj)).Key) && this.Second.Equals(((KeyValuePair<T1, T2>)(obj)).Value))
                );
        }

        public bool Equals(KeyValuePair<T1, T2> other)
        {
            return EqualityComparer<T1>.Default.Equals(First, other.Key)
                   && EqualityComparer<T2>.Default.Equals(Second, other.Value);
        }

        public bool Equals(OrderedPair<T1, T2> other)
        {
            return EqualityComparer<T1>.Default.Equals(First, other.First)
                   && EqualityComparer<T2>.Default.Equals(Second, other.Second);
        }

        IEnumerator<object> IEnumerable<object>.GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        IOrderedEnumerable<object> IOrderedEnumerable<object>.CreateOrderedEnumerable<TKey>(Func<object, TKey> keySelector, IComparer<TKey> comparer, bool @descending)
        {
            return (comparer ?? Comparer<TKey>.Default).Compare(keySelector(First), keySelector(Second)) < 0
                       ? (IOrderedEnumerable<object>)Reverse()
                       : this;
        }

        private IEnumerator<object> GetEnumerator()
        {
            yield return First;
            yield return Second;
        }

        public override int GetHashCode()
        {
            return EqualityComparer<T1>.Default.GetHashCode(First) * EqualityComparer<T2>.Default.GetHashCode(Second);
        }

        public OrderedPair<U1, U2> Cast<U1, U2>()
        {
            return this is OrderedPair<U1, U2>
                       ? (OrderedPair<U1, U2>)(object)this
                       : new OrderedPair<U1, U2>((U1)(object)First, (U2)(object)Second);
        }

        public static bool operator ==(OrderedPair<T1, T2> left, object right)
        {
            return ReferenceEquals(left, null) ? ReferenceEquals(right, null) : left.Equals(right);
        }

        public static bool operator !=(OrderedPair<T1, T2> left, object right)
        {
            return !(left == right);
        }

        public static bool operator ==(object left, OrderedPair<T1, T2> right)
        {
            return right == left;
        }

        public static bool operator !=(object left, OrderedPair<T1, T2> right)
        {
            return right != left;
        }

        public static implicit operator KeyValuePair<T1, T2>(OrderedPair<T1, T2> orderedPair)
        {
            return new KeyValuePair<T1, T2>(orderedPair.First, orderedPair.Second);
        }

        public static implicit operator OrderedPair<T1, T2>(KeyValuePair<T1, T2> keyValuePair)
        {
            return new OrderedPair<T1, T2>(keyValuePair.Key, keyValuePair.Value);
        }
    }
}