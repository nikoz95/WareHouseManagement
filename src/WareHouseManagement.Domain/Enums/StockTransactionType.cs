namespace WareHouseManagement.Domain.Enums;

/// <summary>
/// საწყობის ტრანზაქციის ტიპი
/// </summary>
public enum StockTransactionType
{
    /// <summary>
    /// შემოსვლა - პროდუქტის დამატება საწყობში
    /// </summary>
    In = 1,
    
    /// <summary>
    /// გასვლა - პროდუქტის გაყიდვა/გაცემა
    /// </summary>
    Out = 2,
    
    /// <summary>
    /// დაბრუნება - გაყიდული პროდუქტის უკან დაბრუნება
    /// </summary>
    Return = 3,
    
    /// <summary>
    /// კორექტირება - ინვენტარიზაციის შედეგად
    /// </summary>
    Adjustment = 4,
    
    /// <summary>
    /// დაზიანება/ხარვეზი
    /// </summary>
    Damage = 5,
    
    /// <summary>
    /// გადატანა სხვა საწყობში
    /// </summary>
    Transfer = 6
}

