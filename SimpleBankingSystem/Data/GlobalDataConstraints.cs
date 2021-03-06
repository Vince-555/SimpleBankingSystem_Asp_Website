namespace SimpleBankingSystem.Data
{

    public static class GlobalDataConstraints
    {
        public const int GeneralInputFieldMaxLenght = 20;

        public const int AddressMaxLength = 150;

        public const int AddressMinLength = 5;

        public const int NewsTittleMaxLenght = 150;

        public const int NewsTittleMinLength = 2;

        public const int TransactionDescriptionMaxLength = 200;

        public const int UserNamesMaxLength = 30;

        public const int UserNamesMinLength = 2;

        public const int EmailAddressMaxLength = 320;

        public const int UserVirginPasswordMaxLength = 120;

        public const int MinPasswordLength = 6;

        public const string PaswordRegEx = @"^(?=.*\d)(?=.*[a-z])(?=.*[a-zA-Z]).{6,}$";

        public const string EmailRegEx = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";

        public const string DefaultUserPhotoLocation = "/img/avatars/default_avatar.png";

        public const int IbanMinLength = 16;

        public const int IbanMaxLength = 31;

        public const string CityRegeEx = @"^([a-zA-Z\u0080-\u024F]+(?:. |-| |'))*[a-zA-Z\u0080-\u024F]*$";

        public const int CityMinLength = 2;

        public const int NewsMaxShortenedViewLength = 250;

        public const int NewsMinDescription = 10;


    }
}
