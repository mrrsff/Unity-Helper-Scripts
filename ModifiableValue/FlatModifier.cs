namespace Haven.Value
{
    public class FlatModifier : IModifier<float>
    {
        public int Order => 2; // After percentage modifiers
        public float Value { get; private set; }

        public FlatModifier(float value)
        {
            Value = value;
        }

        public float Modify(float value)
        {
            return value + Value;
        }
    }
}