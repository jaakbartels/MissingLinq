# MissingLinq #

### What is this repository for? ###
Want code that is easier to write, easier to read and executes faster, all at the same time? Welcome to MissingLinq library.

This library bundles a number of useful extensions (most of them extend IEnumerable<T>) that allow you to write code by just telling **what** you want to do, not **how** it should be done (functional programming).


---
##### example 1 #####

Suppose you want to filter a list of customers, keeping only those who live in a EU country. Frequently, I see this done by code like :

```
var result = customers.Where(c => euCountryCodes.Any(s => c.Country== s));
```
compare that to this code, written using MissingLinq :

```
var result = customers.WhereKey(c => c.Country).In(euCountryCodes);
``` 
The latter is not only easier to read (and write), it will also perform much faster when the number of items in the customers list grows. This is because it will build a hashtable to perform the lookups.

---
##### example 2 #####

Ever written something like :

```
var success = processedItems.All(item => !item.HasErrors);
```
and ever struggled understanding what it did? Never missed the exclamation point?

Compare to:

```
var success = processedItems.None(item => item.HasErrors);
``` 
The difference might seem subtle, yet it makes the code easier to read and better conveys the intention of the programmer.

---
These are only a few examples. Of course, there's a lot more.

All methods in this package are created with readability and (hence) maintainability in mind. Not all of them will result in better performance than their standard Linq counterparts (if any). Please measure performance if and when performance is an issue for you.

##### All methods #####

```
items.ForEach(item => DoSomething(item));
```
Executes an action on each element of the list.  This is a "fluent API" (dotted syntax) version of foreach

---
```	
items.ForEach((item, index) => DoSomething(item, index);
```
Executes an action on each element of the list. Action gets an item from the list together with the index of that item

---
```
items.IsEmpty()
```
Returns true if list contains no elements. Same as None(), added for readability

---
```
items.IsNullOrEmpty()
```
Returns true if the list is null or contains no elements.

---
```
items.None(item => item.Children.IsEmpty())
```
Returns true if list contains no elements for which the predicate is true.

---
```
items.None()
```
Returns true if list contains no elements.

---
```
items.IsNotEmpty()
```
Returns true if list contains at least one element.

---
```
items.Contains(item => item.IsNumeric);
```
Returns true if the list contains at least one element for which the predicate is true

---
```
items.SelectAdjacentPairs()
```
Returns pairs of values that appear next to each other in the original list

---
```
items.SelectAllCombinations()
```
Returns all possible combinations of 2 values from the list   (order is not important, i.e. {A,B} = {B,A} and only one of them is returned)

---
```
items.SelectAllCombinations(secondList)
```
Returns all possible combinations of 2 lists   SelectAllCombinations({1,2}, {A,B}) will return {1A, 1B, 2A, 2B}.

---
```
items.Slice(long size)
```
Divides the list in chunks of (at most) sliceSize elements. The last chunk can contain fewer elements

---
```
items.SliceBy(item => item.Name[0])
```
Divides the list in chunks of elements that have the same key value. Unlike grouping, non-adjacent elements are not  regrouped. i.e. the list aaaabbbaacccc wil result in a list of four groups: {aaaa}{bbb}{aa} and {cccc}. (GroupBy would  merge the second group of a's with the first group)

---
```
items.BuildIndex(item => item.Name)
```
Builds an Index to speed up subsequent Find() actions. Groups elements with the same key.

---
```
items.BuildUniqueIndex(item => item.Id)
```
Builds an Index to speed up subsequent Find() actions. Throws if the same key is encountered twice.

---
```
items.WhereKey(item => item.Country).In(allowedCountries)
items.WhereKey(item => item.Country).NotIn(allowedCountries)
```
Returns the elements of a list whose key appears (use with .In) or does not appear (use with .NotIn) in another list. WhereKey(...) should allways be followed by either .In(...) or .NotIn(...) 

---
```
customers.JoinLeft(countries).On(c => c.Country).Equals(country => country.Code)
```
Performs an left outer join of two lists and returns a list of paired values (tuples) of the left and right lists. The second element of the returned tuple can be default(T), if the right list does not contain a matching element

---
```
customers.Join(countries).On(c => c.Country).Equals(country => country.Code)
```
Performs an inner join of two lists and returns a list of paired values (tuples) of the left and right lists. Same as standard Linq method, but with more readable syntax

---
```
items.SelectDuplicates(item => item.Name)
```
Returns all elements whose key values occur more than once.

---
```
items.CountIsGreaterThan(1)
```
Checks if list contains more than count elements.  Does not enumerate the whole list, as opposed to using IEnumerable.Count()

---
```
items.CountIsLessThan(10)
```
Checks if list contains fewer than count elements.  Does not enumerate the whole list, as opposed to using IEnumerable.Count()

---
```
items.FirstOrThrow(() => new meaningfulException())
```
Returns the first element in the list or executes the exceptionCreator if the list is empty.  Used to throw a more meaningful exception instead of the standard "empty list does not have a first element..."

---
```
items.SingleOrThrow(() => new meaningfulExceptiong())
```
Returns the single element in the list  Throws exception created by the exceptionCreator if either the list is empty or contains more than one element.  Used to throw a more meaningful exception instead of the standard "empty list does not have a first element..."

---
```
items.SingleOrThrow(() =>) new emptyListException(), () => new moreThanOneElementException())
```
Returns the single element in the list  Throws exception created by emptyListExceptionCreator if the list is empty.  Throws exception created by moreThanOneElementExceptionCreator if the list contains more than one element.  Used to throw a more meaningful exception instead of the standard "empty list does not have a first element..."

---
```
items.Prepend(extraItem)
```
Adds an item to the beginning of the list

---
```
items.Append(extraItem)
```
Adds an item to the end of the list

---
```
students.MaxBy(s => s.Score);
```
Find the item with the highest whatever-you-want-to-find.  If two keys are equal, the first object with that key will be returned.

---
```
days.MinBy(d => d.Temperature)
```
Find the item with the lowest whatever-you-want-to-find.  If two keys are equal, the first object with that key will be returned.

---
```
items.Zip(otherList)
```
returns pairs of elements that have the same position in left and right lists   (element 0 of left is paired with element 0 of right a.s.o.)

---
```
items.SequenceEquivalent(otherList)
```
```
items.SequenceEquivalent(otherList, itemComparer)
```
Returns true if the two list contain the same elements (not necessarily in the same order)  (e.g. {1,2} is equivalent to {2,1}), but {1,1,2} is NOT equivalent to {1,2,2}) 

---
```
items.Sum(Func<T, TimeSpan> selector)
```
Overload of Sum that allows to calculate the sum of a collection of TimeSpans

---
```
items.MaxOrDefault(Func<TData, TValue> getValueFunc)
```

```
items.MinOrDefault(Func<TData, TValue> getValueFunc)
```

---
```
items.SelectSlidingSubsets(int subsetSize)
```

---
```
items.Flatten(Func<T, IEnumerable<T>> getChildren)
```
Flattens a tree-structured list. Returns parents and children, and their children, and ... in one flat list

---

```
listOfTuples.Where((a,b) => a>b)
```
Overload of Where with 'built-in' decomposition of tuple. 

---
```
listOfTuples.Select((a,b) => a)
```
Overload of Select with 'built-in' decomposition of tuple. 

---

### Installation ###
#### Source code ####
- Copy or clone this repository in a subdirectory or your solution
- Add the MissingLinq project to your solution
- Reference MissingLinq project from other projects in your solution

#### Nuget package ####
coming soon.

### License ###
This library is provided "as is" with no explicit or implied liabilities.
You can use this library free of charge for both non-commercial and commercial projects.

### Author ###
Created by Jaak Bartels. Inspired by the many good developers I met over the last ten years. :-)
