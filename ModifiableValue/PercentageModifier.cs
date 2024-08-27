namespace Haven.Value
{
    public class PercentageModifier : IModifier<float>
    {
        public int Order => 1; // Before flat modifiers
        public float Value { get; private set; }

        public PercentageModifier(float value)
        {
            Value = value;
        }

        public float Modify(float value)
        {
            return value * (1 + Value);
        }
    }
}