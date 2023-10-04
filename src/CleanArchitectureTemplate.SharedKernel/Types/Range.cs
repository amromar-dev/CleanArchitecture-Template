namespace CleanArchitectureTemplate.SharedKernel.Types
{
    public record Range<T> where T : struct, IComparable, IComparable<T>, IConvertible, IEquatable<T>, IFormattable
    {
        public T? From { get; private set; }

        public T? To { get; private set; }

        public Range(T? from, T? to)
        {
            if (from.HasValue && to.HasValue && from.Value.CompareTo(to.Value) > 0)
                throw new ArgumentException("Invalid range: 'From' value should be less than or equal to 'To' value.");

            From = from;
            To = to;
        }

        public bool HasValue => From != null || To != null;
    }

}
