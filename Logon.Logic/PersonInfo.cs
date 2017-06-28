namespace Logon.Logic
{
    /// <summary>
    /// Класс данных о пользователе
    /// </summary>
    public class PersonInfo
    {
        public string login { get; set; } //имя
        public string name { get; set; } //имя
        public string surname { get; set; } //фамилия
        public string middlename { get; set; } //отчество
        public string password { get; set; } //пароль
        public string birthDateDay { get; set; } //день рождения
        public string birthDateMonth { get; set; } //месяц рождения
        public string birthDateYear { get; set; } //год рождения
        public string gender { get; set; } //пол
        public string avatarAddres { get; set; } //адрес картинки профиля
    }
}
