namespace Vkr.Application.Filters
{
    public class FileFilterOptions
    {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = _pageSize = (value > MaxPageSize) ? MaxPageSize : value; ;
            }
        }

        public int PageNumber { get; set; } = 1;

        public string? Name { get; set; }
        public CreatorFilterOptions? Creator { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTill { get; set; }
    }

    public class CreatorFilterOptions
    {
        public string? Name { get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public string? Email { get; set; }
    }
}
