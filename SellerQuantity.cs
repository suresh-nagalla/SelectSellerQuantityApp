using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelectSellerQuantityApp
{
    public class SellerQuantity
    {


        public Dictionary<int, int> GetAvailableSellersWitRequiredQuantity(int[] arrSellers, int[] arrQuantity, int requiredQuantity, int deviationValue)
        {
            ValidateInputs(arrSellers, arrQuantity, requiredQuantity);
            Dictionary<int, int> selectedSellersWithAvailableQuantity = new Dictionary<int, int>();
            int remaining = requiredQuantity;
            while (remaining > 0)
            {
                // if we have exact match found, we make the corresponding quantity as -1 and then return the seller id..
                int getMatchingIndex = GetIndexByValue(arrQuantity, remaining);
                if (getMatchingIndex != -1 && arrQuantity[getMatchingIndex] > 0)
                {
                    MakeSelectedSellerQuantityAsAlreadyVisited(arrQuantity, getMatchingIndex, arrSellers, selectedSellersWithAvailableQuantity);
                    break;
                }
                // lets check if any value is there with in the range of given deviation
                int getMatchingIndexWithDeviation = GetIndexByValueWithDeviation(arrQuantity, remaining, deviationValue);
                if (getMatchingIndexWithDeviation != -1 && arrQuantity[getMatchingIndexWithDeviation] > 0)
                {
                    MakeSelectedSellerQuantityAsAlreadyVisited(arrQuantity, getMatchingIndexWithDeviation, arrSellers, selectedSellersWithAvailableQuantity);
                    break;
                }
                // still if we dont find any matching seller with required quanity then we try to find the nearest max value for the required remaining quantity
                // and then we will try to find the if there is any seller with exact quantity as remaining .. if not, we try with deviation range
                int maxQty = GetMaxQuantityForRemainingValue(arrQuantity, remaining);
                if (maxQty != -1)
                {
                    int maxIndex = GetIndexByValue(arrQuantity, maxQty);
                    MakeSelectedSellerQuantityAsAlreadyVisited(arrQuantity, maxIndex, arrSellers, selectedSellersWithAvailableQuantity);
                    remaining = remaining - maxQty;
                    continue;
                }
                // as a last option, if we dont find any suitable values, we try to find the nearest minmum value for remaining and select that seller and mark that seller and move on.
                int minGreaterQty = GetMinGreaterQuantityForRemaining(arrQuantity, remaining);
                if (minGreaterQty != -1)
                {
                    int biggerIndex = GetIndexByValue(arrQuantity, minGreaterQty);
                    MakeSelectedSellerQuantityAsAlreadyVisited(arrQuantity, biggerIndex, arrSellers, selectedSellersWithAvailableQuantity);
                    break;
                }
                throw new Exception("Unable to find suitable sellers for the requested required quantity");

            }
            return selectedSellersWithAvailableQuantity;
        }

        private void ValidateInputs(int[] arrSellers, int[] arrQuantity, int requiredQuantity)
        {
            // there are lot of if conditions here to handle.. we can improve this further.
            if (arrSellers == null || arrQuantity == null)
            {
                throw new ArgumentNullException("Provided input ( sellers or qunatity info is zero/invalid, Please verify again");
            }
            if (arrSellers.Length != arrQuantity.Length)
            {
                throw new ArgumentException($"Provided input ( sellers or qunatity info length is not matching) - No of Sellers Count - {arrSellers.Length}, correseponding Quantities - {arrQuantity.Length}  ");
            }
            if (requiredQuantity <= 0)
            {
                throw new ArgumentException($"Expected Required quantityto be greater than zero ; Actual - {requiredQuantity}");
            }
            if (arrSellers.Distinct().Count() != arrSellers.Length)
            {
                throw new ArgumentException("Seller Ids should be unique in the input array");
            }
        }

        private int GetIndexByValue(int[] array, int value)
        {
            Console.WriteLine($"Searching for Exact Value Index - {value}");
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == value)
                    return i;
            }
            Console.WriteLine($"No Exact match found for the value - {value}");
            return -1; // always return -1 as index if no match found
        }

        private int GetIndexByValueWithDeviation(int[] array, int value, int deviation)
        {
            Console.WriteLine($"Searching for value - {value} with deviation - {deviation}");
            int minValue = value - deviation;
            int maxValue = value + deviation;
            if (minValue < 1) { minValue = 1; }

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] >= minValue && array[i] <= maxValue)
                {
                    return i;
                }

            }
            Console.WriteLine($"No Exact match found for the value - {value} with deviation - {deviation} with Min Value : {minValue} and Max Value - {maxValue}");
            return -1; // always return -1 as index if no match found
        }

        private int GetMaxQuantityForRemainingValue(int[] arrQuantity, int remaining)
        {
            Console.WriteLine($"Getting max (near by) quantity for remaining value - {remaining}");
            var remainingQunatities = arrQuantity.Where(q => q > 0 && q <= remaining);
            if (!remainingQunatities.Any())
            {
                Console.WriteLine($"No max (near by ) quantity found for the remaining value - {remaining}");
                return -1; // if no value found , we are returning the value as -1
            }
            return remainingQunatities.Max();
        }

        private int GetMinGreaterQuantityForRemaining(int[] arrQuantity, int remaining)
        {
            Console.WriteLine($"Getting min greater (near by) quantity for remaining value - {remaining}");
            var remainingNearByMaxQuantities = arrQuantity.Where(q => q > 0 && q > remaining);
            if (!remainingNearByMaxQuantities.Any())
            {
                Console.WriteLine($"No min greater (near by ) quantity found for the remaining value - {remaining}");
                return -1; // if no value found , we are selecting the value as -1
            }
            return remainingNearByMaxQuantities.Min();
        }

        private void MakeSelectedSellerQuantityAsAlreadyVisited(int[] arrQuantity, int selectedIndex, int[] arrSellers, Dictionary<int, int> result)
        {
            Console.WriteLine($"Found : Selecting Seller - {arrSellers[selectedIndex]} with Quantity - {arrQuantity[selectedIndex]}");
            result.Add(arrSellers[selectedIndex], arrQuantity[selectedIndex]); // i am not checking if the key already exists, as i am expecting unique seller ids in the input array.
            // marking the selected seller's quantity as -1 to avoid revisiting the same seller again.
            Console.WriteLine($"Marking the selected seller - {arrSellers[selectedIndex]} quantity as -1 to avoid revisiting again");
            arrQuantity[selectedIndex] = -1;

        }
    }
}
