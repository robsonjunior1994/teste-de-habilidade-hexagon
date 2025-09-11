namespace UserCRUD.Services.Interfaces
{
    public interface IEncryptionPasswordService
    {
        public string EncryptPassword(string openPassword);
        public bool ValidatePassword(string encryptedPassword, string openPassword);
    }
}
