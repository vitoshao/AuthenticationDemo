namespace Demo1.Models.DTO
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class DtoLoginUser
    {
        public int Id { get; set; }
        public string Account { get; set; } = null!;
    }
}
