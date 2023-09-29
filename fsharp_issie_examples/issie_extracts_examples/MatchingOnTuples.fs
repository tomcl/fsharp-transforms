// -------------------------------------- MatchingOnTuples -------------------------------------- //

// Matching on tuples can lead to very concise and readable code becaue 
// wildcards can be used where a tuple is not matched. 
// Unlike when guards, which break the automatic pattern completion check
// because the compiler cannot work out when a when guard will allow a pattern to match 
// tuple matching makes it easy to see if you have left a case out. (NEP, RSN)

module MatchingOnTuples

// setting up types for extract to compile
type LineId = LineId of int
with member this.Index = match this with | LineId i -> i

let rec usingIfElseVer (searchStart: LineId) (dir: int) (predicate: 'T -> bool) (giveUp: 'T -> bool) (arr: 'T array) =
    if searchStart.Index < 0 || searchStart.Index > arr.Length - 1 then
        None
    elif giveUp arr[searchStart.Index] then
        None 
    elif predicate arr[searchStart.Index] then 
        Some searchStart
    else usingIfElseVer (LineId(searchStart.Index + dir)) dir predicate giveUp arr

// extracted from Issie
// cleaner version, easier to read, follow the logic and identify missing cases 
let rec extractedUsingMatch (searchStart: LineId) (dir: int) (predicate: 'T -> bool) (giveUp: 'T -> bool) (arr: 'T array) =
    if searchStart.Index < 0 || searchStart.Index > arr.Length - 1 then
        None
    else
        match predicate arr[searchStart.Index], giveUp arr[searchStart.Index] with
        | _, true -> None
        | true, _ -> Some searchStart
        | false, _ -> extractedUsingMatch (LineId(searchStart.Index + dir)) dir predicate giveUp arr