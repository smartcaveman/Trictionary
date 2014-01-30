using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Technovation.Collections.Generic
{
    public class DoubleKeyedTrictionary<TLeftKey, TRightKey, TValue> :
        IDictionary<OrderedPair<TLeftKey, TRightKey>, TValue>, IDoubleKeyedTrictionary<TLeftKey, TRightKey, TValue>
    {
        private readonly Dictionary<OrderedPair<TLeftKey, TRightKey>, TValue> dictionary;
        private readonly IEqualityComparer<OrderedPair<TLeftKey, TRightKey>> equalityComparer;

        public DoubleKeyedTrictionary() : this(null, null)
        {
        }

        public DoubleKeyedTrictionary(IDoubleKeyedTrictionary<TLeftKey, TRightKey, TValue> trictionary)
            : this(trictionary, null)
        {
        }

        public DoubleKeyedTrictionary(IDoubleKeyedTrictionary<TLeftKey, TRightKey, TValue> trictionary,
                                      IEqualityComparer<OrderedPair<TLeftKey, TRightKey>> equalityComparer)
        {
            this.equalityComparer = equalityComparer;
            dictionary = trictionary == null
                             ? new Dictionary<OrderedPair<TLeftKey, TRightKey>, TValue>(Comparer)
                             : new Dictionary<OrderedPair<TLeftKey, TRightKey>, TValue>(
                                   trictionary.ToDictionary(_ => _.Key, _ => _.Value), Comparer);
        }

        public IEqualityComparer<OrderedPair<TLeftKey, TRightKey>> Comparer
        {
            get { return equalityComparer ?? EqualityComparer<OrderedPair<TLeftKey, TRightKey>>.Default; }
        }

        protected IDictionary<OrderedPair<TLeftKey, TRightKey>, TValue> Dictionary
        {
            get { return dictionary; }
        }

        IEnumerator<KeyValuePair<OrderedPair<TLeftKey, TRightKey>, TValue>>
            IEnumerable<KeyValuePair<OrderedPair<TLeftKey, TRightKey>, TValue>>.GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Dictionary.GetEnumerator();
        }

        void ICollection<KeyValuePair<OrderedPair<TLeftKey, TRightKey>, TValue>>.Add(
            KeyValuePair<OrderedPair<TLeftKey, TRightKey>, TValue> item)
        {
            Dictionary.Add(item);
        }

        void ICollection<KeyValuePair<OrderedPair<TLeftKey, TRightKey>, TValue>>.Clear()
        {
            Dictionary.Clear();
        }

        bool ICollection<KeyValuePair<OrderedPair<TLeftKey, TRightKey>, TValue>>.Contains(
            KeyValuePair<OrderedPair<TLeftKey, TRightKey>, TValue> item)
        {
            return Dictionary.Contains(item);
        }

        void ICollection<KeyValuePair<OrderedPair<TLeftKey, TRightKey>, TValue>>.CopyTo(
            KeyValuePair<OrderedPair<TLeftKey, TRightKey>, TValue>[] array, int arrayIndex)
        {
            Dictionary.CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<OrderedPair<TLeftKey, TRightKey>, TValue>>.Remove(
            KeyValuePair<OrderedPair<TLeftKey, TRightKey>, TValue> item)
        {
            return Dictionary.Remove(item);
        }

        int ICollection<KeyValuePair<OrderedPair<TLeftKey, TRightKey>, TValue>>.Count
        {
            get { return Dictionary.Count; }
        }

        bool ICollection<KeyValuePair<OrderedPair<TLeftKey, TRightKey>, TValue>>.IsReadOnly
        {
            get { return Dictionary.IsReadOnly; }
        }

        bool IDictionary<OrderedPair<TLeftKey, TRightKey>, TValue>.ContainsKey(OrderedPair<TLeftKey, TRightKey> key)
        {
            return Dictionary.ContainsKey(key);
        }

        void IDictionary<OrderedPair<TLeftKey, TRightKey>, TValue>.Add(OrderedPair<TLeftKey, TRightKey> key,
                                                                       TValue value)
        {
            Dictionary.Add(key, value);
        }

        bool IDictionary<OrderedPair<TLeftKey, TRightKey>, TValue>.Remove(OrderedPair<TLeftKey, TRightKey> key)
        {
            return Dictionary.Remove(key);
        }

        bool IDictionary<OrderedPair<TLeftKey, TRightKey>, TValue>.TryGetValue(OrderedPair<TLeftKey, TRightKey> key,
                                                                               out TValue value)
        {
            return Dictionary.TryGetValue(key, out value);
        }

        TValue IDictionary<OrderedPair<TLeftKey, TRightKey>, TValue>.this[OrderedPair<TLeftKey, TRightKey> key]
        {
            get { return Dictionary[key]; }
            set { Dictionary[key] = value; }
        }

        ICollection<OrderedPair<TLeftKey, TRightKey>> IDictionary<OrderedPair<TLeftKey, TRightKey>, TValue>.Keys
        {
            get { return Dictionary.Keys; }
        }

        ICollection<TValue> IDictionary<OrderedPair<TLeftKey, TRightKey>, TValue>.Values
        {
            get { return Dictionary.Values; }
        }

        public bool TryGetValue(TLeftKey leftKey, TRightKey rightKey, out TValue value)
        {
            return Dictionary.TryGetValue(new OrderedPair<TLeftKey, TRightKey>(leftKey, rightKey), out value);
        }

        public virtual TValue this[TLeftKey leftKey, TRightKey rightKey]
        {
            get { return Dictionary[new OrderedPair<TLeftKey, TRightKey>(leftKey, rightKey)]; }
            set { Dictionary[new OrderedPair<TLeftKey, TRightKey>(leftKey, rightKey)] = value; }
        }

        IDictionary<TLeftKey, TValue> IDoubleKeyedTrictionary<TLeftKey, TRightKey, TValue>.this[TRightKey rightKey]
        {
            get { return new StaticRightKey(this, rightKey); }
        }

        IDictionary<TRightKey, TValue> IDoubleKeyedTrictionary<TLeftKey, TRightKey, TValue>.this[TLeftKey leftKey]
        {
            get { return new StaticLeftKey(this, leftKey); }
        }

        public IEnumerable<OrderedPair<TLeftKey, TRightKey>> CompositeKeys
        {
            get { return Dictionary.Keys; }
        }

        public IEnumerable<TLeftKey> LeftKeys
        {
            get
            {
                foreach (var key in CompositeKeys)
                    yield return key.First;
            }
        }

        public IEnumerable<TRightKey> RightKeys
        {
            get
            {
                foreach (var key in CompositeKeys)
                    yield return key.Second;
            }
        }

        public IEnumerable<TValue> Values
        {
            get
            {
                foreach (TValue value in dictionary.Values)
                    yield return value;
            }
        }

        public int Count
        {
            get { return Dictionary.Count; }
        }

        public void Add(TLeftKey leftKey, TRightKey rightKey, TValue value)
        {
            if (leftKey == null) throw new ArgumentNullException("leftKey");
            if (rightKey == null) throw new ArgumentNullException("rightKey");
            Dictionary[new OrderedPair<TLeftKey, TRightKey>(leftKey, rightKey)] = value;
        }

        public bool ContainsCompositeKey(TLeftKey leftKey, TRightKey rightKey)
        {
            if (leftKey == null) throw new ArgumentNullException("leftKey");
            if (rightKey == null) throw new ArgumentNullException("rightKey");
            return Dictionary.ContainsKey(new OrderedPair<TLeftKey, TRightKey>(leftKey, rightKey));
        }

        public bool ContainsLeftKey(TLeftKey leftKey)
        {
            if (leftKey == null) throw new ArgumentNullException("leftKey");
            return LeftKeys.Contains(leftKey);
        }

        public bool ContainsRightKey(TRightKey rightKey)
        {
            if (rightKey == null) throw new ArgumentNullException("rightKey");
            return RightKeys.Contains(rightKey);
        }

        public bool Remove(TLeftKey leftKey, TRightKey rightKey)
        {
            if (leftKey == null) throw new ArgumentNullException("leftKey");
            if (rightKey == null) throw new ArgumentNullException("rightKey");
            return Dictionary.Remove(new OrderedPair<TLeftKey, TRightKey>(leftKey, rightKey));
        }

        public void Clear()
        {
            Dictionary.Clear();
        }

        public void Clear(TLeftKey leftKey)
        {
            if (leftKey == null) throw new ArgumentNullException("leftKey");
            var queue =
                new Queue<OrderedPair<TLeftKey, TRightKey>>(from key in CompositeKeys
                                                            where key.First.Equals(leftKey)
                                                            select key);
            while (queue.Count > 0)
            {
                Dictionary.Remove(queue.Dequeue());
            }
            if (ContainsLeftKey(leftKey))
            {
                Clear(leftKey: leftKey);
            }
        }

        public void Clear(TRightKey rightKey)
        {
            if (rightKey == null) throw new ArgumentNullException("rightKey");
            var queue =
                new Queue<OrderedPair<TLeftKey, TRightKey>>(from key in CompositeKeys
                                                            where key.Second.Equals(rightKey)
                                                            select key);
            while (queue.Count > 0)
            {
                Dictionary.Remove(queue.Dequeue());
            }
            if (ContainsRightKey(rightKey))
            {
                Clear(rightKey: rightKey);
            }
        }

        #region Partial Key Dictionaries

        protected abstract class Component : IEquatable<Component>
        {
            private readonly DoubleKeyedTrictionary<TLeftKey, TRightKey, TValue> trictionary;

            internal Component(DoubleKeyedTrictionary<TLeftKey, TRightKey, TValue> trictionary)
            {
                if (trictionary == null) throw new ArgumentNullException("trictionary");
                this.trictionary = trictionary;
            }

            protected DoubleKeyedTrictionary<TLeftKey, TRightKey, TValue> Trictionary
            {
                get { return trictionary; }
            }

            /// <summary>
            ///     Indicates whether the current object is equal to another object of the same type.
            /// </summary>
            /// <returns>
            ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
            /// </returns>
            /// <param name="other">An object to compare with this object.</param>
            public bool Equals(Component other)
            {
                if (ReferenceEquals(null, other)) return false;
                if (ReferenceEquals(this, other)) return true;
                return Equals(other.trictionary, trictionary) && GetType() == other.GetType();
            }

            public override bool Equals(object obj)
            {
                return obj is Component && GetType() == obj.GetType() &&
                       ((Component) obj).Trictionary.Equals(Trictionary);
            }

            /// <summary>
            ///     Serves as a hash function for a particular type.
            /// </summary>
            /// <returns>
            ///     A hash code for the current <see cref="T:System.Object" />.
            /// </returns>
            /// <filterpriority>2</filterpriority>
            public override int GetHashCode()
            {
                return GetType().GetHashCode() ^ (trictionary != null ? trictionary.GetHashCode() : 0);
            }
        }

        protected sealed class StaticLeftKey : Component, IDictionary<TRightKey, TValue>
        {
            private readonly TLeftKey leftKey;

            internal StaticLeftKey(DoubleKeyedTrictionary<TLeftKey, TRightKey, TValue> trictionary, TLeftKey leftKey)
                : base(trictionary)
            {
                if (trictionary == null) throw new ArgumentNullException("trictionary");
                if (leftKey == null) throw new ArgumentNullException("leftKey");
                this.leftKey = leftKey;
            }

            private IEnumerable<KeyValuePair<TRightKey, TValue>> Enumerable
            {
                get
                {
                    return Trictionary
                        .Where(x => x.Key.First.Equals(leftKey))
                        .Select(x => new KeyValuePair<TRightKey, TValue>(x.Key.Second, x.Value));
                }
            }

            public IEnumerator<KeyValuePair<TRightKey, TValue>> GetEnumerator()
            {
                return Enumerable.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(KeyValuePair<TRightKey, TValue> item)
            {
                Add(item.Key, item.Value);
            }

            public void Clear()
            {
                Trictionary.Clear(leftKey);
            }

            public bool Contains(KeyValuePair<TRightKey, TValue> item)
            {
                return Trictionary.ContainsCompositeKey(leftKey, item.Key) &&
                       Equals(Trictionary[leftKey, item.Key], item.Value);
            }

            public void CopyTo(KeyValuePair<TRightKey, TValue>[] array, int arrayIndex)
            {
                Enumerable.ToArray().CopyTo(array, arrayIndex);
            }

            public bool Remove(KeyValuePair<TRightKey, TValue> item)
            {
                return Trictionary.Remove(leftKey, item.Key);
            }

            public int Count
            {
                get { return Enumerable.Count(); }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public bool ContainsKey(TRightKey key)
            {
                return Trictionary.ContainsCompositeKey(leftKey, key);
            }

            public void Add(TRightKey key, TValue value)
            {
                Trictionary.Add(leftKey, key, value);
            }

            public bool Remove(TRightKey key)
            {
                return Trictionary.Remove(leftKey, key);
            }

            public bool TryGetValue(TRightKey key, out TValue value)
            {
                return Trictionary.TryGetValue(leftKey, key, out value);
            }

            public TValue this[TRightKey key]
            {
                get { return Trictionary[leftKey, key]; }
                set { Trictionary[leftKey, key] = value; }
            }

            public ICollection<TRightKey> Keys
            {
                get { throw new NotSupportedException(); }
            }

            public ICollection<TValue> Values
            {
                get { throw new NotSupportedException(); }
            }
        }

        protected sealed class StaticRightKey : Component, IDictionary<TLeftKey, TValue>
        {
            private readonly TRightKey rightKey;

            internal StaticRightKey(DoubleKeyedTrictionary<TLeftKey, TRightKey, TValue> trictionary, TRightKey rightKey)
                : base(trictionary)
            {
                if (trictionary == null) throw new ArgumentNullException("trictionary");
                if (rightKey == null) throw new ArgumentNullException("rightKey");
                this.rightKey = rightKey;
            }

            private IEnumerable<KeyValuePair<TLeftKey, TValue>> Enumerable
            {
                get
                {
                    return Trictionary
                        .Where(x => x.Key.Second.Equals(rightKey))
                        .Select(x => new KeyValuePair<TLeftKey, TValue>(x.Key.First, x.Value));
                }
            }

            public IEnumerator<KeyValuePair<TLeftKey, TValue>> GetEnumerator()
            {
                return Enumerable.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public void Add(KeyValuePair<TLeftKey, TValue> item)
            {
                Add(item.Key, item.Value);
            }

            public void Clear()
            {
                Trictionary.Clear(rightKey);
            }

            public bool Contains(KeyValuePair<TLeftKey, TValue> item)
            {
                return Trictionary.ContainsCompositeKey(item.Key, rightKey) &&
                       Equals(Trictionary[item.Key, rightKey], item.Value);
            }

            public void CopyTo(KeyValuePair<TLeftKey, TValue>[] array, int arrayIndex)
            {
                Enumerable.ToArray().CopyTo(array, arrayIndex);
            }

            public bool Remove(KeyValuePair<TLeftKey, TValue> item)
            {
                return Trictionary.Remove(item.Key, rightKey);
            }

            public int Count
            {
                get { return Enumerable.Count(); }
            }

            public bool IsReadOnly
            {
                get { return false; }
            }

            public bool ContainsKey(TLeftKey key)
            {
                return Trictionary.ContainsCompositeKey(key, rightKey);
            }

            public void Add(TLeftKey key, TValue value)
            {
                Trictionary.Add(key, rightKey, value);
            }

            public bool Remove(TLeftKey key)
            {
                return Trictionary.Remove(key, rightKey);
            }

            public bool TryGetValue(TLeftKey key, out TValue value)
            {
                return Trictionary.TryGetValue(key, rightKey, out value);
            }

            public TValue this[TLeftKey key]
            {
                get { return Trictionary[key, rightKey]; }
                set { Trictionary[key, rightKey] = value; }
            }

            public ICollection<TLeftKey> Keys
            {
                get { throw new NotSupportedException(); }
            }

            public ICollection<TValue> Values
            {
                get { throw new NotSupportedException(); }
            }
        }

        #endregion Partial Key Dictionaries
    }
}