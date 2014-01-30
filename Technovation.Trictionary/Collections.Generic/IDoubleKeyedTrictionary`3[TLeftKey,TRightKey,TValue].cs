using System.Collections.Generic;

namespace Technovation.Collections.Generic
{
    public interface IDoubleKeyedTrictionary<TLeftKey, TRightKey, TValue> : IEnumerable<KeyValuePair<OrderedPair<TLeftKey, TRightKey>, TValue>>
    {
        bool TryGetValue(TLeftKey leftKey, TRightKey rightKey, out TValue value);

        TValue this[TLeftKey leftKey, TRightKey rightKey] { get; set; }

        IDictionary<TLeftKey, TValue> this[TRightKey rightKey] { get; }

        IDictionary<TRightKey, TValue> this[TLeftKey leftKey] { get; }

        IEnumerable<OrderedPair<TLeftKey, TRightKey>> CompositeKeys { get; }

        IEnumerable<TLeftKey> LeftKeys { get; }

        IEnumerable<TRightKey> RightKeys { get; }

        IEnumerable<TValue> Values { get; }

        int Count { get; }

        void Add(TLeftKey leftKey, TRightKey rightKey, TValue value);

        bool ContainsCompositeKey(TLeftKey leftKey, TRightKey rightKey);

        bool ContainsLeftKey(TLeftKey leftKey);

        bool ContainsRightKey(TRightKey rightKey);

        bool Remove(TLeftKey leftKey, TRightKey rightKey);

        void Clear();

        void Clear(TLeftKey leftKey);

        void Clear(TRightKey rightKey);
    }
}