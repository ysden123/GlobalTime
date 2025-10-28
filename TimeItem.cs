namespace GlobalTime
{
    /// <summary>
    /// Represents a time entry for a specific city, including the current time in that location. Used for representation purposes.
    /// </summary>
    /// <remarks>This record is used to store and access the current time information for a given city. It is
    /// immutable, meaning that once an instance is created, its properties cannot be changed.</remarks>
    internal record TimeItem
    {
        public string City { get; init; }
        public string CurrentTime { get; init; }
        public TimeItem(string city, string currentTime)
        {
            City = city;
            CurrentTime = currentTime;
        }
    }
}
