


# Task Mastery
This library adds some utility functions for working with .Net core Tasks.

## Added Extension Methods
### IEnumerable\<Task\>.WhenAll()
**Returns**: `Task`

**Description**: Takes a list of Tasks and returns a Task that is completed when all the Tasks in the list have been completed.

**Example usage**:
```cs
List<int> numbers = new List<int> { 1, 2, 3 };

await numbers
  .Select(_ => Task.Delay(1000))
  .WhenAll();

// All the delay tasks will be awaited in parallel
```
<hr>

### IEnumerable\<Task\<A\>\>.WhenAll()
**Returns**: `Task<IEnumerable<A>>`

**Description**: Takes a list of `Task`s and returns a `Task` with the resulting list. This turns the Task with the list "inside out".

**Example usage**:
```cs
List<int> numbers = new List<int> { 1, 2, 3 };

IEnumerable<int> incrementedNumbers = await numbers
  .Select(async number => {
    await Task.Delay(1000);
    return number + 1;
  })
  .WhenAll();

// incrementedNumbers == new List<int> { 2, 3, 4 };
```
<hr>

### Task\<A\>.Map\<A, B\>(Func\<A, B\> f)
**Returns**: `Task<B>`

**Description**: Applies a function to the content of a Task.

**Example usage**
```cs
List<int> numbers = new List<int> { 1, 2, 3 };

IEnumerable<int> sumOfIncrementedNumbers = await numbers
  .Select(async number => {
    await Task.Delay(1000);
    return number + 1;
  })
  .WhenAll()
  .Map(list => list.Aggregate(0, (a, b) => a + b));

// sumOfIncrementedNumbers == 2 + 3 + 4 == 9
```
<hr>

### Task\<A\>.FlatMap(Func\<A, Task\<B\>\> f)
**Returns**: `Task<B>`

**Description**: Applies an asynchronous function to a value inside a Task.

**Example usage**:
```cs
var resultFromSecondHttpRequest = await SomeHttpRequestAsync()
  .FlatMap(resultFromFirstRequest => SomeOtherHttpRequestAsync(resultFromFirstRequest));
// chains two asynchronous http requests together
```
<hr>

### IEnumerable\<Task\<A\>\>.SelectTasks(Func\<A, B\> f)
**Returns**: `IEnumerable<Task<B>>`

**Description**: Applies a function to the content of all Tasks in an IEnumerable.

**Example Usage**:
```cs
List<int> numbers = new List<int> { 1, 2, 3 };

IEnumerable<int> incrementedNumbers = await numbers
  .Select(async number => {
    await Task.Delay(1000);
    return number + 1;
  })
  .SelectTasks(number => number + 1)
  .WhenAll();

// incrementedNumbers == new List<int> { 3, 4, 5 };
```
Notice how the function `number => number + 1` is of type `int -> int` despite us working with an `IEnumerable<Task<int>>`.
<hr>

### IEnumerable\<Task\<A\>\>.SelectTasksFlatten(Func\<A, Task\<B\>\> f)
**Returns**: `IEnumerable<Task<B>>`

**Description**: Applies an asynchronous function to the content of all Tasks
in an IEnumerable.

**Example Usage**:
```cs
List<int> numbers = new List<int> { 1, 2, 3 };

IEnumerable<Task<SomeOtherResponse>> responses = numbers
  .Select(n => SomeHttpRequestAsync(n))
  .SelectTaskFlatten(response => SomeOtherHttpRequestAsync(response));
```
`responses` will now be a list of all the responses from `SomeOtherHttpRequestAsync`. If we would have used `SelectTasks` instead of `SelectTasksFlatten` then `responses` would have been `IEnumerable<Task<Task<SomeOtherResponse>>>` which is just plain silly. The "flatten" part means that we flatten the nested Tasks to one Task.

## Build package
```
$ dotnet pack --configuration release
```
A `.nupkg` file is now available under `bin/release`.


