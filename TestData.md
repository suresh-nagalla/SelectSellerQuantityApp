# Test Data for SellerSelection_AI

## Basic Test Scenarios

### Test 1: Exact Match
```csharp
int[] sellers = new int[] { 1, 2, 3 };
int[] qty = new int[] { 100, 40, 60 };
int required = 100;
int deviation = 0;
// Expected: Seller 1 with quantity 100
```

### Test 2: Deviation Match
```csharp
int[] sellers = new int[] { 11, 12, 13 };
int[] qty = new int[] { 95, 98, 40 };
int required = 100;
int deviation = 5;
// Expected: Seller 11 (95) or Seller 12 (98) - both within deviation
```

### Test 3: Multiple Sellers
```csharp
int[] sellers = new int[] { 20, 21, 22, 23 };
int[] qty = new int[] { 50, 60, 30, 20 };
int required = 110;
int deviation = 0;
// Expected: Seller 21 (60) + Seller 20 (50) = 110
```

### Test 4: Overshoot Allowed
```csharp
int[] sellers = new int[] { 40, 41, 42 };
int[] qty = new int[] { 200, 10, 5 };
int required = 12;
int deviation = 0;
// Expected: Seller 41 (10) + Seller 42 (5) = 15 OR Seller 40 (200) as smallest overshoot
```

## Advanced Test Scenarios

### Test 5: Complex Multi-Seller
```csharp
int[] sellers = new int[] { 101, 102, 103, 104, 105 };
int[] qty = new int[] { 25, 35, 50, 75, 100 };
int required = 150;
int deviation = 0;
// Expected: Seller 105 (100) + Seller 103 (50) = 150
```

### Test 6: Deviation with Overshoot
```csharp
int[] sellers = new int[] { 201, 202, 203 };
int[] qty = new int[] { 88, 92, 105 };
int required = 90;
int deviation = 3;
// Expected: Seller 201 (88) or Seller 202 (92) within ±3
```

### Test 7: Large Quantity Test
```csharp
int[] sellers = new int[] { 301, 302, 303, 304, 305, 306 };
int[] qty = new int[] { 1000, 500, 300, 200, 150, 50 };
int required = 2000;
int deviation = 50;
// Expected: Multiple sellers summing to 2000 or within deviation
```

### Test 8: Small Deviation Use
```csharp
int[] sellers = new int[] { 401, 402, 403, 404 };
int[] qty = new int[] { 47, 53, 25, 30 };
int required = 50;
int deviation = 4;
// Expected: Seller 401 (47) or Seller 402 (53) - both within ±4
```

### Test 9: Multiple Small Sellers
```csharp
int[] sellers = new int[] { 501, 502, 503, 504, 505 };
int[] qty = new int[] { 10, 15, 20, 25, 30 };
int required = 75;
int deviation = 0;
// Expected: Seller 505 (30) + Seller 504 (25) + Seller 503 (20) = 75
```

### Test 10: Minimal Overshoot
```csharp
int[] sellers = new int[] { 601, 602, 603 };
int[] qty = new int[] { 101, 105, 110 };
int required = 100;
int deviation = 0;
// Expected: Seller 601 (101) - smallest overshoot by 1
```

## Validation Test Scenarios (Should Throw Exceptions)

### Test 11: Negative Quantity
```csharp
int[] sellers = new int[] { 701, 702 };
int[] qty = new int[] { 50, -10 };
int required = 50;
int deviation = 0;
// Expected: ArgumentException - "Negative quantity found"
```

### Test 12: Duplicate Sellers
```csharp
int[] sellers = new int[] { 801, 802, 801 };
int[] qty = new int[] { 50, 60, 70 };
int required = 100;
int deviation = 0;
// Expected: ArgumentException - "Duplicate seller IDs found"
```

### Test 13: Array Length Mismatch
```csharp
int[] sellers = new int[] { 901, 902, 903 };
int[] qty = new int[] { 50, 60 };
int required = 100;
int deviation = 0;
// Expected: ArgumentException - "Array length mismatch"
```

### Test 14: Null Array
```csharp
int[] sellers = null;
int[] qty = new int[] { 50, 60 };
int required = 100;
int deviation = 0;
// Expected: ArgumentNullException - "Sellers array cannot be null"
```

### Test 15: Invalid Required Quantity
```csharp
int[] sellers = new int[] { 1001, 1002 };
int[] qty = new int[] { 50, 60 };
int required = 0;
int deviation = 0;
// Expected: ArgumentException - "Required quantity must be greater than zero"
```

### Test 16: Negative Deviation
```csharp
int[] sellers = new int[] { 1101, 1102 };
int[] qty = new int[] { 50, 60 };
int required = 100;
int deviation = -5;
// Expected: ArgumentException - "Deviation cannot be negative"
```

### Test 17: Unable to Fulfill
```csharp
int[] sellers = new int[] { 1201, 1202, 1203 };
int[] qty = new int[] { 10, 15, 20 };
int required = 1000;
int deviation = 0;
// Expected: InvalidOperationException - "Unable to fulfill the required quantity"
```

## Edge Cases

### Test 18: Single Seller Exact Match
```csharp
int[] sellers = new int[] { 1301 };
int[] qty = new int[] { 250 };
int required = 250;
int deviation = 0;
// Expected: Seller 1301 (250)
```

### Test 19: All Sellers Too Small
```csharp
int[] sellers = new int[] { 1401, 1402, 1403 };
int[] qty = new int[] { 10, 20, 30 };
int required = 100;
int deviation = 0;
// Expected: Combination of all sellers (60 total) won't work - picks smallest overshoot or fails
```

### Test 20: Zero Quantities
```csharp
int[] sellers = new int[] { 1501, 1502, 1503 };
int[] qty = new int[] { 0, 0, 100 };
int required = 100;
int deviation = 0;
// Expected: Seller 1503 (100) - zeros are ignored
```

### Test 21: Large Deviation
```csharp
int[] sellers = new int[] { 1601, 1602, 1603 };
int[] qty = new int[] { 50, 75, 120 };
int required = 100;
int deviation = 25;
// Expected: Any seller within 75-125 range (Seller 1602 or 1603)
```

### Test 22: Perfect Multi-Seller Split
```csharp
int[] sellers = new int[] { 1701, 1702, 1703, 1704 };
int[] qty = new int[] { 25, 25, 25, 25 };
int required = 100;
int deviation = 0;
// Expected: All 4 sellers (25+25+25+25 = 100)
```

## Performance Test Data

### Test 23: Many Sellers
```csharp
int[] sellers = new int[] { 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010 };
int[] qty = new int[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
int required = 200;
int deviation = 10;
// Expected: Optimal combination of sellers
```

### Test 24: All Same Quantity
```csharp
int[] sellers = new int[] { 2101, 2102, 2103, 2104, 2105 };
int[] qty = new int[] { 50, 50, 50, 50, 50 };
int required = 150;
int deviation = 0;
// Expected: Any 3 sellers (50+50+50 = 150)
```

### Test 25: Descending Quantities
```csharp
int[] sellers = new int[] { 2201, 2202, 2203, 2204, 2205 };
int[] qty = new int[] { 100, 80, 60, 40, 20 };
int required = 140;
int deviation = 0;
// Expected: Seller 2201 (100) + Seller 2203 (40) = 140
```
