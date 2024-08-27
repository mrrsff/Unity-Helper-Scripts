namespace Haven.Value
{
    public interface IModifier<T>
    {
        int Order { get; } // Lower is applied first
        T Modify(T value);
    }
}
