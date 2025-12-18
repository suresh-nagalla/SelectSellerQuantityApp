// See https://aka.ms/new-console-template for more information
using SelectSellerQuantityApp;

Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗");
Console.WriteLine("║        SELLER SELECTION AI - TEST SCENARIOS                   ║");
Console.WriteLine("╔═══════════════════════════════════════════════════════════════╗\n");

var sellerSelectionAI = new SellerSelection_AI();

// Test Case 1: Exact Match
Console.WriteLine("\n█████ TEST CASE 1: Exact Match █████");
RunTest(sellerSelectionAI, 
    sellers: new int[] { 1, 2, 3 }, 
    qty: new int[] { 100, 40, 60 }, 
    required: 100, 
    deviation: 0,
    testName: "Exact Match - Seller 1 has exactly 100");

// Test Case 2: Deviation Match
Console.WriteLine("\n█████ TEST CASE 2: Deviation Match █████");
RunTest(sellerSelectionAI, 
    sellers: new int[] { 11, 12, 13 }, 
    qty: new int[] { 95, 98, 40 }, 
    required: 100, 
    deviation: 5,
    testName: "Deviation Match - 95 or 98 is close enough");

// Test Case 3: Multiple Sellers (Exact Sum)
Console.WriteLine("\n█████ TEST CASE 3: Multiple Sellers █████");
RunTest(sellerSelectionAI, 
    sellers: new int[] { 20, 21, 22, 23 }, 
    qty: new int[] { 50, 60, 30, 20 }, 
    required: 110, 
    deviation: 0,
    testName: "Multiple Sellers - 60 + 50 = 110");

// Test Case 4: Overshoot Scenario
Console.WriteLine("\n█████ TEST CASE 4: Overshoot Scenario █████");
RunTest(sellerSelectionAI, 
    sellers: new int[] { 40, 41, 42 }, 
    qty: new int[] { 200, 10, 5 }, 
    required: 12, 
    deviation: 0,
    testName: "Overshoot - 10 + 5 = 15 or smallest overshoot 200");

// Test Case 5: Complex Multi-Seller
Console.WriteLine("\n█████ TEST CASE 5: Complex Multi-Seller █████");
RunTest(sellerSelectionAI, 
    sellers: new int[] { 101, 102, 103, 104, 105 }, 
    qty: new int[] { 25, 35, 50, 75, 100 }, 
    required: 150, 
    deviation: 0,
    testName: "Complex - 100 + 50 = 150");

// Test Case 6: Deviation with Overshoot
Console.WriteLine("\n█████ TEST CASE 6: Deviation with Overshoot █████");
RunTest(sellerSelectionAI, 
    sellers: new int[] { 201, 202, 203 }, 
    qty: new int[] { 88, 92, 105 }, 
    required: 90, 
    deviation: 3,
    testName: "Deviation Match - 88 or 92 within ±3");

// Test Case 7: Large Quantity Test
Console.WriteLine("\n█████ TEST CASE 7: Large Quantity █████");
RunTest(sellerSelectionAI, 
    sellers: new int[] { 301, 302, 303, 304, 305, 306 }, 
    qty: new int[] { 1000, 500, 300, 200, 150, 50 }, 
    required: 2000, 
    deviation: 50,
    testName: "Large Quantity - 1000 + 500 + 300 + 200 = 2000");

// Test Case 8: Small Deviation Use
Console.WriteLine("\n█████ TEST CASE 8: Small Deviation █████");
RunTest(sellerSelectionAI, 
    sellers: new int[] { 401, 402, 403, 404 }, 
    qty: new int[] { 47, 53, 25, 30 }, 
    required: 50, 
    deviation: 4,
    testName: "Deviation - 47 or 53 within ±4 of 50");

// Test Case 9: Multiple Small Sellers
Console.WriteLine("\n█████ TEST CASE 9: Multiple Small Sellers █████");
RunTest(sellerSelectionAI, 
    sellers: new int[] { 501, 502, 503, 504, 505 }, 
    qty: new int[] { 10, 15, 20, 25, 30 }, 
    required: 75, 
    deviation: 0,
    testName: "Multiple Small - 30 + 25 + 20 = 75");

// Test Case 10: Minimal Overshoot
Console.WriteLine("\n█████ TEST CASE 10: Minimal Overshoot █████");
RunTest(sellerSelectionAI, 
    sellers: new int[] { 601, 602, 603 }, 
    qty: new int[] { 101, 105, 110 }, 
    required: 100, 
    deviation: 0,
    testName: "Minimal Overshoot - Pick 101 (smallest overshoot)");

// Test Case 11: Validation Test - Negative Quantity (Should Fail)
Console.WriteLine("\n█████ TEST CASE 11: Validation - Negative Quantity █████");
RunTestWithExpectedException(sellerSelectionAI,
    sellers: new int[] { 701, 702 },
    qty: new int[] { 50, -10 },
    required: 50,
    deviation: 0,
    testName: "Should throw ArgumentException for negative quantity");

// Test Case 12: Validation Test - Duplicate Sellers (Should Fail)
Console.WriteLine("\n█████ TEST CASE 12: Validation - Duplicate Sellers █████");
RunTestWithExpectedException(sellerSelectionAI,
    sellers: new int[] { 801, 802, 801 },
    qty: new int[] { 50, 60, 70 },
    required: 100,
    deviation: 0,
    testName: "Should throw ArgumentException for duplicate seller IDs");

// Test Case 13: Validation Test - Array Length Mismatch (Should Fail)
Console.WriteLine("\n█████ TEST CASE 13: Validation - Array Length Mismatch █████");
RunTestWithExpectedException(sellerSelectionAI,
    sellers: new int[] { 901, 902, 903 },
    qty: new int[] { 50, 60 },
    required: 100,
    deviation: 0,
    testName: "Should throw ArgumentException for array length mismatch");

// Test Case 14: Validation Test - Null Array (Should Fail)
Console.WriteLine("\n█████ TEST CASE 14: Validation - Null Array █████");
RunTestWithExpectedException(sellerSelectionAI,
    sellers: null,
    qty: new int[] { 50, 60 },
    required: 100,
    deviation: 0,
    testName: "Should throw ArgumentNullException for null sellers array");

// Test Case 15: Validation Test - Invalid Required (Should Fail)
Console.WriteLine("\n█████ TEST CASE 15: Validation - Invalid Required Quantity █████");
RunTestWithExpectedException(sellerSelectionAI,
    sellers: new int[] { 1001, 1002 },
    qty: new int[] { 50, 60 },
    required: 0,
    deviation: 0,
    testName: "Should throw ArgumentException for required <= 0");

Console.WriteLine("\n╔═══════════════════════════════════════════════════════════════╗");
Console.WriteLine("║                 ALL TESTS COMPLETED                           ║");
Console.WriteLine("╚═══════════════════════════════════════════════════════════════╝\n");

static void RunTest(SellerSelection_AI ai, int[] sellers, int[] qty, int required, int deviation, string testName)
{
    try
    {
        Console.WriteLine($"\nTest: {testName}");
        Console.WriteLine($"Input: Sellers={string.Join(",", sellers)}, Qty={string.Join(",", qty)}, Required={required}, Deviation={deviation}");
        
        var result = ai.GetAvailableSellersInfo(sellers, qty, required, deviation);
        
        Console.WriteLine($"✅ TEST PASSED: {testName}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ TEST FAILED: {testName}");
        Console.WriteLine($"   Error: {ex.Message}");
    }
}

static void RunTestWithExpectedException(SellerSelection_AI ai, int[] sellers, int[] qty, int required, int deviation, string testName)
{
    try
    {
        Console.WriteLine($"\nTest: {testName}");
        Console.WriteLine($"Input: Sellers={sellers?.Length ?? 0} items, Qty={qty?.Length ?? 0} items, Required={required}, Deviation={deviation}");
        
        var result = ai.GetAvailableSellersInfo(sellers, qty, required, deviation);
        
        Console.WriteLine($"❌ TEST FAILED: {testName} - Expected exception but none was thrown!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"✅ TEST PASSED: {testName}");
        Console.WriteLine($"   Expected exception caught: {ex.GetType().Name} - {ex.Message}");
    }
}


#region Non AI generated code execution sample
/* 
 //Non AI generted code execution sample.. 
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
 */
#endregion