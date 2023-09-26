// -------------------------------- 1. Structural Abstraction -------------------------------- //

// Partition code into functions each doing part of the work.
// This is an example that:
// 1. Filter out numbers that are less than 10.
// 2. Square the remaining numbers.
// 3. Convert the squared integers to strings.
// 4. Concatenate them into a single string separated by commas.
module structuralAbstraction

// Before Structural Abstraction, everything is done in a single function.
let processNumbers1 numbers =
    let filtered = List.filter (fun x -> x >= 10) numbers
    let squared = List.map (fun x -> x * x) filtered
    let asStrings = List.map string squared
    let concatenated = String.concat "," asStrings
    concatenated

let numbers = [1; 11; 5; 12; 9]
let result = processNumbers1 numbers



// After Structural Abstraction
let filterNumbers numbers = 
    List.filter (fun x -> x >= 10) numbers

let squareNumbers numbers =
    List.map (fun x -> x * x) numbers

let convertNumbersToStrings numbers =
    List.map string numbers

let concatenateStrings strings =
    String.concat "," strings

let processNumbers2 numbers =
    numbers
    |> filterNumbers
    |> squareNumbers
    |> convertNumbersToStrings
    |> concatenateStrings

let numbers2 = [1; 11; 5; 12; 9]
let result2 = processNumbers2 numbers

// Even though, the second example is more lengthy,
// it is easier to understand what processnumbers2 is doing just by looking at it.