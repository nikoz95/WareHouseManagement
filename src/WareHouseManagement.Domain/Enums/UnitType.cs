namespace WareHouseManagement.Domain.Enums;

/// <summary>
/// პროდუქტის საზომი ერთეულის ტიპი
/// </summary>
public enum UnitType
{
    /// <summary>
    /// ცალი - რაოდენობა ითვლება ცალების მიხედვით
    /// </summary>
    Piece = 0,
    
    /// <summary>
    /// ლიტრი - მოცულობით გაზომვა
    /// </summary>
    Liter = 1,
    
    /// <summary>
    /// მილილიტრი - მოცულობით გაზომვა
    /// </summary>
    Milliliter = 2,
    
    /// <summary>
    /// კილოგრამი - წონით გაზომვა
    /// </summary>
    Kilogram = 3,
    
    /// <summary>
    /// გრამი - წონით გაზომვა
    /// </summary>
    Gram = 4,
    
    /// <summary>
    /// ყუთი - შეფუთვის ერთეული
    /// </summary>
    Box = 5,
    
    /// <summary>
    /// პაკეტი - შეფუთვის ერთეული
    /// </summary>
    Package = 6
}

