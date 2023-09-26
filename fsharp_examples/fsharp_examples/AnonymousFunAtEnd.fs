// -------------------------------- 13. Pipeline with Anonymous Function at end -------------------------------- //

// FP allows a pipeline to be formed with an anonymous function at the end of it.
// If you think about it, this is identical to a let definition followed an expression using
// the let-defined value(s). the choice of which form is best should be made based on readability:
// it is often not clear-cut which form is better - don't assume the "more functional" one is better!

module anonymouFunAtEnd
// Without Anonymous Function
let result = [1; 2; 3; 4; 5]
                |> List.map (fun x -> x * x)
                |> List.filter (fun x -> x % 2 = 0)
                |> List.sum
printfn "The result is %d" result
// Here, the print statement can't be used as part of the pipeline on its own.
// This could be solved by using it with an anonymous function.

// With Anonymous Function
[1; 2; 3; 4; 5]
|> List.map (fun x -> x * x)
|> List.filter (fun x -> x % 2 = 0)
|> List.sum
|> (fun result -> printfn "The result is %d" result)