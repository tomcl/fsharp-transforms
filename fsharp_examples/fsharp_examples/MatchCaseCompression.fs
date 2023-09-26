// -------------------------------- 4. Match Case Compression 1 -------------------------------- //

// Match case compression means moving common code outside match expressions to reduce duplication.
// Move common code inside match case expressions, outside, before the match.
// Note that this may require the match case expressions (or part of them)
// to be turned into functions so that the “variable” bits can be factored out.
// But often all that is needed is let definitions before.
// Imagine a function processStringOpt that judges the length of a string.

module matchCaseComp

// Before Compression
let processStringOpt1 strOpt =
    match strOpt with
    | Some str ->
        let len = String.length str
        if len > 5 then
            printfn "Long string: %s" str
        else
            printfn "Short string: %s" str
    | None ->
        let len = 0
        printfn "No string provided, length is %d" len


// After Compression
let processStringOpt2 strOpt =
    // len is a common variable that is used in both cases of the match. It can be calculted before the match statement.
    let len = match strOpt with Some str -> String.length str | None -> 0
    match strOpt with
    | Some str ->
        if len > 5 then
            printfn "Long string: %s" str
        else
            printfn "Short string: %s" str
    | None ->
        printfn "No string provided, length is %d" len



// -------------------------------- 4. Match Case Compression 2 -------------------------------- //

// Move common code in match case expressions after the match – pipelining the match result into the common function(s).
// Let's consider a function that takes an int option and either squares the integer or reports that it is None.
// In both cases, we then convert the result to a string for further processing.

// Before Compression
let processIntOpt1 intOpt =
    match intOpt with
    | Some x ->
        let result = x * x
        let strResult = string result
        printfn "Result: %s" strResult
    | None ->
        let strResult = "None"
        printfn "Result: %s" strResult


// After Compression
let convertAndPrint strResult = printfn "Result: %s" strResult

let processIntOpt2 intOpt =
    match intOpt with
    | Some x -> string (x * x)
    | None -> "None"

    |> convertAndPrint
// Match result is pipelined into convertAndPrint