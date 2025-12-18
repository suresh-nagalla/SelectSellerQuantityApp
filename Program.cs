// See https://aka.ms/new-console-template for more information
using SelectSellerQuantityApp;


SellerQuantity s = new SellerQuantity();
int requiredQuantity = 100;
int deviationValue = 2;
int[] SellerIds = new int[] { 1, 2, 3, 4, 5 };
int[] availableQunatities = new int[] { 50, 20, 70, 30, 90 };

Dictionary<int, int> sellersInfo = s.GetAvailableSellersWitRequiredQuantity(arrSellers: SellerIds,
    arrQuantity: availableQunatities, requiredQuantity: requiredQuantity, deviationValue: deviationValue);

Console.WriteLine($"Selected Sellers and their Quantities for the given Required Quantity - {requiredQuantity} and with Deviation - {deviationValue}");
foreach (var item in sellersInfo)
{
    Console.WriteLine($"Seller ID: {item.Key}, Quantity: {item.Value}");
}


