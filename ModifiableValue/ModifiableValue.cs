using System.Collections.Generic;
using System;

namespace Haven.Value
{
    [Serializable]
    public class ModifiableValue<T>
    {
        public T BaseValue { get; private set; }
        public T Value { get; private set; }
        // Hold a sorted list of modifiers by order
        private readonly List<IModifier<T>> _modifiers = new();

        public ModifiableValue(T baseValue)
        {
            BaseValue = baseValue;
            Value = baseValue;
        }

        public void AddModifier(IModifier<T> modifier)
        {
            _modifiers.Add(modifier);
            // Sort the list by order (low -> high)
            _modifiers.Sort((a, b) => a.Order.CompareTo(b.Order));

            UpdateValue();
        }

        public void RemoveModifier(IModifier<T> modifier)
        {
            _modifiers.Remove(modifier);
            UpdateValue();
        }

        private void UpdateValue()
        {
            Value = BaseValue;
            foreach (var modifier in _modifiers) // TODO: Optimize this
            {
                Value = modifier.Modify(Value);
            }
        }

        public void SetBaseValue(T baseValue)
        {
            BaseValue = baseValue;
            UpdateValue();
        }

        public void ClearModifiers()
        {
            _modifiers.Clear();
            UpdateValue();
        }
    }
}
