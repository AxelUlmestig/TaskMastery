# Task Mastery
This library adds some utility functions for working with .Net core Tasks.

## Added Extension Methods
### List\<Task\>.WhenAll()
**Returns**: `Task`

**Description**: Takes a list of Tasks and returns a Task that is completed when all the Tasks in the list have been completed.

**Example usage**:
```cs
List<int> numbers = new List<int> { 1, 2, 3 };

await numbers
  .Select(_ => Task.Delay(1000))
  .ToList()
  .WhenAll();

// All the delay tasks will be awaited in parallel
```
### List\<Task\<A\>\>.WhenAll()
**Returns**: `Task<List<A>>`

**Description**: Takes a list of `Task`s and returns a `Task` with the resulting list. This turns the Task with the list "inside out".

**Example usage**:
```cs
List<int> numbers = new List<int> { 1, 2, 3 };

List<int> incrementedNumbers = await numbers
  .Select(async number => {
    await Task.Delay(1000);
    return number + 1;
  })
  .ToList()
  .WhenAll();

// incrementedNumbers == new List<int> { 2, 3, 4 };
```
### Task\<A\>.Map\<A, B\>(Func\<A, B\> f)
**Returns**: `Task<B>`

**Description**: Applies a function to the content of a Task.

**Example usage**
```cs
List<int> numbers = new List<int> { 1, 2, 3 };

List<int> sumOfIncrementedNumbers = await numbers
  .Select(async number => {
    await Task.Delay(1000);
    return number + 1;
  })
  .ToList()
  .WhenAll()
  .Map(list => list.Aggregate(0, (a, b) => a + b));

// sumOfIncrementedNumbers == 2 + 3 + 4 == 9
```
### Task\<A\>.FlatMap(Func\<A, Task\<B\>\> f)
**Returns**: `Task<B>`

**Description**: Applies an asynchronous function to a value inside a Task.

**Example usage**:
```cs
var resultFromSecondHttpRequest = await SomeHttpRequestAsync()
  .FlatMap(resultFromFirstRequest => SomeOtherHttpRequestAsync(resultFromFirstRequest));
// chains two asynchronous http requests together
```
## Build package
```
$ dotnet pack --configuration release
```
A `.nupkg` file is now available under `bin/release`.


