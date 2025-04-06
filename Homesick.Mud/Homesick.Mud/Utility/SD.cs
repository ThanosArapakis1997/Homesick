namespace Homesick.Mud.Utility
{
    public class SD
    {
        public static string WebAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string ListingAPIBase { get; set; }

        //ROLES
        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "BUYER";

        //LISTING STATUS
        public const string StatusCreated = "Αρχικό";
        public const string StatusInReview = "Υπό Επεξεργασία";
        public const string StatusApproved = "Εγκεκριμένη";

        //PROPERTY TYPE
        public const string PropertyTypeHouse = "Κατοικία";
        public const string PropertyTypeLand = "Οικόπεδο";

        //HOUSE TYPES
        public const string HouseTypeFlat = "Διαμέρισμα";
        public const string HouseTypeStudio = "Studio";
        public const string HouseTypeLoft = "Loft";
        public const string HouseTypeMaisonette = "Μεζονέτα";
        public const string HouseTypeVilla = "Βίλλα";

        //LISTING TYPE
        public const string ListingTypeBuy = "Αγορά";
        public const string ListingTypeRent = "Ενοικίαση";

        public const string TokenCookie = "JWTToken";

        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}
