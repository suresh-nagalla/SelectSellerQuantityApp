using System;
using System.Collections.Generic;
using System.Linq;

namespace SelectSellerQuantityApp
{
    public class SellerSelection_AI
    {
        /// <summary>
        /// Main public method to get available sellers that can fulfill the required quantity
        /// </summary>
        public Dictionary<int, int> GetAvailableSellersInfo(int[] sellers, int[] qty, int required, int deviation)
        {
            ValidateInputs(sellers, qty, required, deviation);
            
            // Convert to internal dictionary for easier manipulation
            var sellerQtyMap = BuildSellerDictionary(sellers, qty);
            var selectedSellers = new Dictionary<int, int>();
            
            LogSelectionStart(sellerQtyMap, required, deviation);
            
            int remaining = required;
            
            // Try to fulfill the requirement
            remaining = ProcessSellerSelection(sellerQtyMap, selectedSellers, remaining, deviation);
            
            // Final summary
            LogFinalSummary(selectedSellers, required);
            
            return selectedSellers;
        }

        #region Validation

        private void ValidateInputs(int[] sellers, int[] qty, int required, int deviation)
        {
            // Null checks
            if (sellers == null)
            {
                throw new ArgumentNullException(nameof(sellers), "Sellers array cannot be null");
            }
            
            if (qty == null)
            {
                throw new ArgumentNullException(nameof(qty), "Quantity array cannot be null");
            }
            
            // Array length mismatch
            if (sellers.Length != qty.Length)
            {
                throw new ArgumentException(
                    $"Array length mismatch: sellers has {sellers.Length} elements, qty has {qty.Length} elements");
            }
            
            // Required quantity validation
            if (required <= 0)
            {
                throw new ArgumentException($"Required quantity must be greater than zero. Provided: {required}", nameof(required));
            }
            
            // Deviation validation
            if (deviation < 0)
            {
                throw new ArgumentException($"Deviation cannot be negative. Provided: {deviation}", nameof(deviation));
            }
            
            // Negative quantities check
            for (int i = 0; i < qty.Length; i++)
            {
                if (qty[i] < 0)
                {
                    throw new ArgumentException($"Negative quantity found at index {i}: {qty[i]}", nameof(qty));
                }
            }
            
            // Duplicate seller IDs check
            if (sellers.Length != sellers.Distinct().Count())
            {
                var duplicates = sellers.GroupBy(s => s)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key);
                throw new ArgumentException($"Duplicate seller IDs found: {string.Join(", ", duplicates)}", nameof(sellers));
            }
        }

        #endregion

        #region Helper Methods

        private Dictionary<int, int> BuildSellerDictionary(int[] sellers, int[] qty)
        {
            var dict = new Dictionary<int, int>();
            for (int i = 0; i < sellers.Length; i++)
            {
                dict[sellers[i]] = qty[i];
            }
            return dict;
        }

        private int ProcessSellerSelection(Dictionary<int, int> sellerQtyMap, Dictionary<int, int> selectedSellers, 
            int remaining, int deviation)
        {
            while (remaining > 0)
            {
                // Step 1: Try exact match
                var exactMatch = FindExactMatch(sellerQtyMap, remaining);
                if (exactMatch.HasValue)
                {
                    int sellerId = exactMatch.Value;
                    int sellerQty = sellerQtyMap[sellerId];
                    SelectSeller(sellerQtyMap, selectedSellers, sellerId, sellerQty, remaining, "Exact match");
                    remaining = 0;
                    break;
                }
                
                // Step 2: Try deviation match (close enough)
                if (deviation > 0)
                {
                    var deviationMatch = FindDeviationMatch(sellerQtyMap, remaining, deviation);
                    if (deviationMatch.HasValue)
                    {
                        int sellerId = deviationMatch.Value;
                        int sellerQty = sellerQtyMap[sellerId];
                        SelectSeller(sellerQtyMap, selectedSellers, sellerId, sellerQty, remaining, 
                            $"Deviation match (within ±{deviation})");
                        remaining = 0;
                        break;
                    }
                }
                
                // Step 3: Try to use sellers with quantities less than or equal to remaining
                var bestFitUnder = FindBestFitUnder(sellerQtyMap, remaining);
                if (bestFitUnder.HasValue)
                {
                    int sellerId = bestFitUnder.Value;
                    int sellerQty = sellerQtyMap[sellerId];
                    SelectSeller(sellerQtyMap, selectedSellers, sellerId, sellerQty, remaining, 
                        "Best fit under remaining");
                    remaining -= sellerQty;
                    continue;
                }
                
                // Step 4: Only oversized quantities remain - pick smallest overshoot
                var smallestOvershoot = FindSmallestOvershoot(sellerQtyMap, remaining);
                if (smallestOvershoot.HasValue)
                {
                    int sellerId = smallestOvershoot.Value;
                    int sellerQty = sellerQtyMap[sellerId];
                    int overshoot = sellerQty - remaining;
                    SelectSeller(sellerQtyMap, selectedSellers, sellerId, sellerQty, remaining, 
                        $"Smallest overshoot (+{overshoot} extra)");
                    remaining = 0;
                    break;
                }
                
                // Step 5: No suitable sellers found
                throw new InvalidOperationException(
                    $"Unable to fulfill the required quantity. Remaining: {remaining}, Available sellers: {GetAvailableSellersCount(sellerQtyMap)}");
            }
            
            return remaining;
        }

        #endregion

        #region Picking Logic

        private int? FindExactMatch(Dictionary<int, int> sellerQtyMap, int target)
        {
            Console.WriteLine($"? Looking for exact match: {target}");
            
            foreach (var seller in sellerQtyMap)
            {
                if (seller.Value == target)
                {
                    Console.WriteLine($"  ? Found exact match: Seller {seller.Key} with quantity {seller.Value}");
                    return seller.Key;
                }
            }
            
            Console.WriteLine($"  ? No exact match found");
            return null;
        }

        private int? FindDeviationMatch(Dictionary<int, int> sellerQtyMap, int target, int deviation)
        {
            Console.WriteLine($"? Looking for deviation match: {target} ± {deviation} (range: {target - deviation} to {target + deviation})");
            
            int minAcceptable = target - deviation;
            int maxAcceptable = target + deviation;
            
            foreach (var seller in sellerQtyMap)
            {
                if (seller.Value >= minAcceptable && seller.Value <= maxAcceptable)
                {
                    Console.WriteLine($"  ? Found deviation match: Seller {seller.Key} with quantity {seller.Value}");
                    return seller.Key;
                }
            }
            
            Console.WriteLine($"  ? No deviation match found");
            return null;
        }

        private int? FindBestFitUnder(Dictionary<int, int> sellerQtyMap, int remaining)
        {
            Console.WriteLine($"? Looking for best fit under remaining: {remaining}");
            
            var candidates = sellerQtyMap
                .Where(s => s.Value > 0 && s.Value <= remaining)
                .ToList();
            
            if (!candidates.Any())
            {
                Console.WriteLine($"  ? No sellers with quantity ? {remaining}");
                return null;
            }
            
            // Pick the largest quantity that fits under remaining (effective utilization)
            var best = candidates.OrderByDescending(s => s.Value).First();
            Console.WriteLine($"  ? Found best fit: Seller {best.Key} with quantity {best.Value}");
            return best.Key;
        }

        private int? FindSmallestOvershoot(Dictionary<int, int> sellerQtyMap, int remaining)
        {
            Console.WriteLine($"? Looking for smallest overshoot (quantity > {remaining})");
            
            var candidates = sellerQtyMap
                .Where(s => s.Value > 0 && s.Value > remaining)
                .ToList();
            
            if (!candidates.Any())
            {
                Console.WriteLine($"  ? No sellers with quantity > {remaining}");
                return null;
            }
            
            var smallest = candidates.OrderBy(s => s.Value).First();
            Console.WriteLine($"  ? Found smallest overshoot: Seller {smallest.Key} with quantity {smallest.Value}");
            return smallest.Key;
        }

        private void SelectSeller(Dictionary<int, int> sellerQtyMap, Dictionary<int, int> selectedSellers, 
            int sellerId, int sellerQty, int currentRemaining, string reason)
        {
            Console.WriteLine($"\n*** SELECTED Seller {sellerId} ***");
            Console.WriteLine($"  Quantity used: {sellerQty}");
            Console.WriteLine($"  Reason: {reason}");
            Console.WriteLine($"  Remaining before: {currentRemaining}");
            Console.WriteLine($"  Remaining after: {Math.Max(0, currentRemaining - sellerQty)}");
            
            selectedSellers[sellerId] = sellerQty;
            sellerQtyMap[sellerId] = -1; // Mark as visited
            
            Console.WriteLine($"  Seller {sellerId} marked as used (set to -1)\n");
        }

        private int GetAvailableSellersCount(Dictionary<int, int> sellerQtyMap)
        {
            return sellerQtyMap.Count(s => s.Value > 0);
        }

        #endregion

        #region Logging

        private void LogSelectionStart(Dictionary<int, int> sellerQtyMap, int required, int deviation)
        {
            Console.WriteLine($"\n{'=',50}");
            Console.WriteLine("  SELLER SELECTION PROCESS STARTED");
            Console.WriteLine($"{'=',50}");
            Console.WriteLine($"Required Quantity: {required}");
            Console.WriteLine($"Allowed Deviation: {deviation}");
            Console.WriteLine($"Available Sellers: {string.Join(", ", sellerQtyMap.Select(s => $"[Seller {s.Key}: {s.Value}]"))}");
            Console.WriteLine($"{'=',50}\n");
        }

        private void LogFinalSummary(Dictionary<int, int> selectedSellers, int required)
        {
            int totalFulfilled = selectedSellers.Values.Sum();
            int difference = totalFulfilled - required;
            
            Console.WriteLine($"\n{'=',50}");
            Console.WriteLine("  FINAL SELECTION SUMMARY");
            Console.WriteLine($"{'=',50}");
            Console.WriteLine($"Required Quantity: {required}");
            Console.WriteLine($"Total Fulfilled: {totalFulfilled}");
            Console.WriteLine($"Difference: {(difference >= 0 ? "+" : "")}{difference}");
            Console.WriteLine($"Number of Sellers Used: {selectedSellers.Count}");
            Console.WriteLine("\nSelected Sellers:");
            
            foreach (var seller in selectedSellers)
            {
                Console.WriteLine($"  • Seller {seller.Key}: {seller.Value} units");
            }
            
            Console.WriteLine($"{'=',50}\n");
        }

        #endregion
    }
}
