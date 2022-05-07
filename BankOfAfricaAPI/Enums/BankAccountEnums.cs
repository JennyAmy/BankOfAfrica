namespace BankOfAfricaAPI.Enums
{
    public class BankAccountEnums
    {
        public enum AccountTypeEnum
        {
            SavingsAccount = 1,
            CurrentAccount = 2,
            KiddiesAccount = 3,

        }

        public enum MaritalStatusEnum
        {
            Single = 1,
            Married = 2,
            Other = 3,
            PreferNotToSay = 4,
        }


        public enum TitleEnum
        {
            Miss = 1,
            Mrs = 2,
            Mr = 3,
        }

        public enum GenderEnum
        {
            Female = 1,
            Male = 2,
            Other = 3,
        }
    }
}
