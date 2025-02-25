namespace Demo2.Models.DTO
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class LoginUserDto
    {
        public int Id { get; set; }
        public string Account { get; set; } = null!;
    }
}
