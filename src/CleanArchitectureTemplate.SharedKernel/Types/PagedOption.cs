namespace CleanArchitectureTemplate.SharedKernel.Types
{
    public record PagedOptions<TSorting> where TSorting : Enum
    {
        public PagedOptions(int pageNumber, int pageSize, TSorting sorting)
        {
            if (pageNumber <= 0)
                throw new ArgumentException("Invalid page number");

            if (pageSize <= 0)
                throw new ArgumentException("Invalid page size");

            PageNumber = pageNumber;
            PageSize = pageSize;
            Sorting = sorting;
        }

        public int PageNumber { get; private set; }
        
        public int PageSize { get; private set; }
        
        public TSorting Sorting { get; private set; }
    }
}
