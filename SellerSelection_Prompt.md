# Seller Selection
I’m developing a small .NET 8 console app and I need a helper class that can figure out which sellers can fulfil the quantity I ask for. Class naming convention should follow: the class name should end with `_AI`.
Strictly Rule to Follow : Always Create a new Class than using any existing one.. 
I need **one public class and public method** that drives the whole thing:

```
expected signature: public Dictionary<int,int> GetAvailableSellersInfo(int[] sellers, int[] qty, int required, int deviation)
```

Input meaning in simple talk:
- `sellers[i]` → seller id
- `qty[i]` → quantity that seller has
- `required` → what I want
- `deviation` → small flexibility where “close enough” is fine

I don’t want a big fat single method.  
Split things logically — validation, picking logic, logging — keep it readable.

Also, internally it’s easier to use a `Dictionary<int,int>` (sellerId → availableQty). When I pick a seller, update their entry to `-1` or something similar so that seller is never revisited again.

As a Lead SDET, I want solid input checks — throw exceptions if input validation fails.:
- null arrays → throw
- array length mismatch → throw
- `required <= 0` → invalid
- `deviation < 0` → invalid
- negative quantities → throw
- duplicate seller ids -> throw error.
If the data is wrong, stop right there and throw the right exception.

think like a seller who tries to fullfil the required qunatity asked by customer here ( effective utilization).
- if one seller has exactly what I need → done
- if a seller is close enough within deviation → accept it and finish
- otherwise, the remaining amount using sellers that fit under that number (but don’t reuse anyone)
- if only oversized quantities remain, pick the smallest overshoot and finish
- if nothing helps at all, throw instead of looping in circles

Logging — nothing fancy, just `Console.WriteLine` with useful info:
- which seller was picked
- how much quantity was used
- updated remaining quantity
- whether deviation was used
- whether an overshoot decision happened
- a final wrap-up at the end

Some rough examples to guide the behavior:

Example 1  
sellers: `[1,2,3]`  
qty: `[100,40,60]`  
required: `100`  
→ pick the 100 and finish.

Example 2  
sellers: `[11,12,13]`  
qty: `[95,98,40]`  
required: `100`, deviation `5`  
→ 95 or 98 is fine. One pick and done.

Example 3  
sellers: `[20,21,22,23]`  
qty: `[50,60,30,20]`  
required: `110`  
→ pick 60, remaining 50, then pick 50.

Example 4 (overshoot allowed)  
sellers: `[40,41,42]`  
qty: `[200,10,5]`  
required: `12`  
→ 10 + 5 works; or smallest overshoot — either way is fine. Just print what happened.

Code should stay clean, easy to read, and modern C# (.NET 8). no restrictions w.r.t the .net features which you want to use. No over-design or heavy abstractions.  
Just a simple helper that does the job.
also, generate a set of data which i can use to test this class.